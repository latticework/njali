using System;
using System.Collections;
using System.Linq;
using Jali.Core;
using Jali.Note;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jali.Serve
{
    /// <summary>
    ///     Converts a service message to and from JSON.
    /// </summary>
    public class ServiceMesssageConverter : JsonConverter
    {
        /// <summary>
        ///     Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">
        ///     The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="serializer">
        ///     The calling serializer.
        /// </param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            var message = value as IServiceMessage;

            if (message == null)
            {
                throw new ArgumentException($"'{nameof(value)}' is not a service message.", nameof(value));
            }

            var contract = message.Contract;
            var credentials = message.Credentials;
            var data = message.Data;
            var messages = message.Messages;
            var identity = message.Identity;
            var connection = message.Connection;
            var tenant = message.Tenant;

            var json = new JObject();

            if (contract != null)
            {
                json.Add("contract", new JObject()
                {
                    ["url"] = contract.Url,
                    ["consumerId"] = contract.ConsumerId,
                    ["version"] = contract.Version,
                });
            }

            if (credentials != null)
            {
                json.Add("credentials", new JObject()
                {
                    ["usrid"] = credentials.UserId,
                    ["impid"] = credentials.ImpersonatorId,
                    ["depid"] = credentials.DeputyId,
                });
            }

            if (data != null)
            {
                if (data is IEnumerable)
                {
                    json.Add("data", JArray.FromObject(data, serializer));
                }
                else
                {
                    json.Add("data", JObject.FromObject(data, serializer));
                }
            }

            messages = messages ?? Enumerable.Empty<NotificationMessage>();

            var messageArray =
                new JArray(messages.Select(nm => JObject.FromObject(nm, serializer)).Cast<object>().ToArray());

            json.Add("messages", messageArray);


            if (identity != null)
            {
                json.Add("identity", new JObject()
                {
                    ["tid"] = identity.TransactionId,
                    ["sid"] = identity.SessionId,
                    ["cid"] = identity.ConversationId,
                    ["mid"] = identity.MessageId,
                    ["mtid"] = identity.MessageTransmissionId,
                });
            }

            if (connection != null)
            {
                json.Add("connection", new JObject
                {
                    ["sender"] = new JObject
                    {
                        ["scid"] = connection.Sender?.ServiceCenterId,
                        ["dcid"] = connection.Sender?.DataCenterId,
                    },
                    ["receiver"] = new JObject
                    {
                        ["scid"] = connection.Receiver?.ServiceCenterId,
                        ["dcid"] = connection.Receiver?.DataCenterId,
                    },
                });
            }

            if (tenant != null)
            {
                json.Add("tenant", new JObject
                {
                    ["tnid"] = tenant.TenantId,
                    ["tnoid"] = tenant.TenantOrgId,
                });
            }

            serializer.Serialize(writer, json);
        }

        /// <summary>
        ///     Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.
        /// </param>
        /// <param name="objectType">
        ///     Type of the object.
        /// </param>
        /// <param name="existingValue">
        ///     The existing value of object being read.
        /// </param>
        /// <param name="serializer">
        ///     The calling serializer.
        /// </param>
        /// <returns>
        ///     The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // TODO: ServiceMessageConverter: See if there is a way to use the error handling or tracer from the serializer.
            var json = JObject.Load(reader);

            VerifyElementType("service message", JTokenType.Object, json.Type);

            var message = (IServiceMessage)Activator.CreateInstance(objectType);
            message.Contract = new MessageContract();
            message.Credentials = new MessageCredentials();
            message.Identity = new MessageIdentity();
            message.Connection = new MessageConnection
            {
                Sender = new EndpointPartition(),
                Receiver = new EndpointPartition(),
            };
            message.Tenant = new TenantIdentity();

            var contract = GetObjectProperty(json, "contract");
            if (contract != null)
            {
                // TODO: ServiceMessageConverter.ReadJson: Ensure good error message for bad URL.
                message.Contract.Url = new Uri(GetJsonProperty<string>(contract, "url", JTokenType.String));
                message.Contract.ConsumerId = GetJsonProperty<string>(contract, "consumerId", JTokenType.String);
                message.Contract.Version = GetJsonProperty<string>(contract, "consumerId", JTokenType.String);
            }

            var credentials = GetObjectProperty(json, "credentials");
            if (credentials != null)
            {
                message.Credentials.UserId = GetJsonProperty<string>(credentials, "usrid", JTokenType.String);
                message.Credentials.ImpersonatorId = GetJsonProperty<string>(credentials, "impid", JTokenType.String);
                message.Credentials.DeputyId = GetJsonProperty<string>(credentials, "depid", JTokenType.String);
            }

            var data = GetObjectProperty(json, "data");
            var dataType = objectType.GenericTypeArguments[0];

            message.Data = dataType == typeof (JObject) ? data : serializer.Deserialize(reader, dataType);

            var messages = GetJsonProperty<JArray>(json, "messages", JTokenType.Array);

            if (messages != null)
            {
                var messageCollection = new NotificationMessageCollection();
                var index = 0;
                foreach (var notification in messages)
                {
                    ++index;
                    VerifyElementType($"message[{index}]", JTokenType.Object, notification.Type);

                    messageCollection.Add(serializer.Deserialize<NotificationMessage>(new JTokenReader(notification)));
                }

                message.Messages = messageCollection;
            }

            var identity = GetObjectProperty(json, "identity");
            if (identity != null)
            {
                message.Identity.TransactionId = GetJsonProperty<string>(identity, "tid", JTokenType.String);
                message.Identity.SessionId = GetJsonProperty<string>(identity, "sid", JTokenType.String);
                message.Identity.ConversationId = GetJsonProperty<string>(identity, "cid", JTokenType.String);
                message.Identity.MessageId = GetJsonProperty<string>(identity, "mid", JTokenType.String);

                message.Identity.MessageTransmissionId = GetJsonProperty<string>(
                    identity, "mtid", JTokenType.String);
            }

            var connection = GetObjectProperty(json, "connection");
            if (connection != null)
            {
                var sender = GetObjectProperty(connection, "sender");
                if (sender != null)
                {
                    message.Connection.Sender.ServiceCenterId = GetJsonProperty<string>(
                        sender, "scid", JTokenType.String);

                    message.Connection.Sender.ServiceCenterId = GetJsonProperty<string>(
                        sender, "dcid", JTokenType.String);
                }

                var receiver = GetObjectProperty(connection, "receiver");
                if (receiver != null)
                {
                    message.Connection.Sender.ServiceCenterId = GetJsonProperty<string>(
                        receiver, "scid", JTokenType.String);

                    message.Connection.Sender.ServiceCenterId = GetJsonProperty<string>(
                        receiver, "dcid", JTokenType.String);
                }
            }

            var tenant = GetObjectProperty(json, "tenant");
            if (tenant != null)
            {
                message.Tenant.TenantId = GetJsonProperty<string>(tenant, "tnid", JTokenType.String);
                message.Tenant.TenantOrgId = GetJsonProperty<string>(tenant, "tnoid", JTokenType.String);
            }

            return message;
        }

        /// <summary>
        ///     Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">
        ///     Type of the object.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if this instance can convert the specified object type; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            if (objectType == null) throw new ArgumentNullException(nameof(objectType));

            return objectType.GetGenericTypeDefinition() == typeof (ServiceMessage<>);
        }

        private static JObject GetObjectProperty(JObject container, string propertyName)
        {
            return GetJsonProperty<JObject>(container, propertyName, JTokenType.Object);
        }

        private static T GetJsonProperty<T>(JObject container, string propertyName, JTokenType expectedType)
            where T : class
        {
            var propertyToken = container[propertyName];

            if (propertyToken == null) { return null; }


            VerifyElementType(propertyName, expectedType, propertyToken.Type);
            return propertyToken.Value<T>();
        }

        private static void VerifyElementType(string elementName, JTokenType expectedType, JTokenType actualType)
        {
            if (actualType == expectedType) { return; }

            var message = $"Jali Server requires that '{elementName}' be '{expectedType}'. Yours is '{actualType}'";
            throw new InternalErrorException(message);
        }
    }
}