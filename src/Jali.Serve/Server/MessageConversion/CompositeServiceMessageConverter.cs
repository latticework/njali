using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Note;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     Implementation of <see cref="IServiceMessageConverter"/> that delegates to converters for each
    /// </summary>
    public sealed class CompositeServiceMessageConverter : IServiceMessageConverter
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CompositeServiceMessageConverter"/> class with the 
        ///     specified options.
        /// </summary>
        /// <param name="options">
        ///     The converter's options or <see langword="null"/> if defaults will be used.
        /// </param>
        public CompositeServiceMessageConverter(CompositeServiceMessageConverterOptions options = null)
        {
            var overrideOptions = options ?? new CompositeServiceMessageConverterOptions();

            this.Options = new CompositeServiceMessageConverterOptions
            {
                Serializer = overrideOptions.Serializer ?? new DefaultServiceMessageSerializer(),
                ContractConverter = overrideOptions.ContractConverter ?? new DefaultMessageContractConverter(),
                CredentialsConverter = overrideOptions.CredentialsConverter ?? new DefaultMessageCredentialConverter(), 
                DataConverter = overrideOptions.DataConverter ?? new DefaultServiceMessageDataConverter(),
                NotificationConverter =
                    overrideOptions.NotificationConverter ?? new DefaultNotificationMessageConverter(),
                IdentityConverter = overrideOptions.IdentityConverter ?? new DefaultMessageIdentityConverter(),
                ConnectionConverter = overrideOptions.ConnectionConverter ?? new DefaultMessageConnectionConverter(),
                TenantConverter = overrideOptions.TenantConverter ?? new DefaultServiceMessageTenantConverter(),
            };
        }

        /// <summary>
        ///     The converter's options.
        /// </summary>
        public CompositeServiceMessageConverterOptions Options { get; }

        /// <summary>
        ///     Converts from a <see cref="HttpRequestMessage"/> to a <see cref="ServiceMessage{JObject}"/>.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <returns>
        ///     The request service message.
        /// </returns>
        public async Task<ServiceMessage<JObject>> FromRequest(
            IExecutionContext context, MessageConversionContext conversionContext, HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = request.Method.Method;

            string jsonString;

            if (method == "GET" || method == "DELETE")
            {
                var query = request.GetQueryStrings();

                if (query.Keys.Count != 1 || !query.Keys.Contains("json"))
                {
                    var errorMessage =
                        $"Jali Server requires that all '{method}' methods have a single query parameter named 'json'";

                    // TODO: JaliHttpRequestMessageExtensions.AsServiceMessage: Replace with BusinessDomainException.
                    throw new Exception(errorMessage);
                }

                jsonString = query["json"];
            }
            else
            {
                jsonString = await request.Content.ReadAsStringAsync();
            }

            var json = JObject.Parse(jsonString);

            var message = await this.Options.Serializer.ToServiceMessage(context, conversionContext, json);

            var contract = await this.Options.ContractConverter.FromRequest(
                context, conversionContext, request, message);

            if (contract != null)
            {
                message.Contract = contract;
            }

            var credentials = await this.Options.CredentialsConverter.FromRequest(
                context, conversionContext, request, message);

            if (contract != null)
            {
                message.Credentials = credentials;
            }

            var messages = await this.Options.NotificationConverter.FromRequest(context, conversionContext, request, message);
            if (contract != null)
            {
                // Eplicit interface implementation replaces all elements.
                ((IServiceMessage) message).Messages = messages;
            }

            var identity = await this.Options.IdentityConverter.FromRequest(context, conversionContext, request, message);
            if (identity != null)
            {
                message.Identity = identity;
            }

            var connection = await this.Options.ConnectionConverter.FromRequest(context, conversionContext, request, message);
            if (connection != null)
            {
                message.Connection = connection;
            }

            var tenant = await this.Options.TenantConverter.FromRequest(context, conversionContext, request, message);
            if (tenant != null)
            {
                message.Tenant = tenant;
            }

            var data = await this.Options.DataConverter.FromRequest(context, conversionContext, request, message);
            if (data != null)
            {
                message.Data = data;
            }

            return message;
        }


        /// <summary>
        ///     Converts from an <see cref="IServiceMessage"/> to an <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
        /// <param name="message">
        ///     The response service message.
        /// </param>
        /// <param name="request">
        ///     The initial http request.
        /// </param>
        /// <returns>
        ///     The http response.
        /// </returns>
        public async Task<HttpResponseMessage> ToResponse(
            IExecutionContext context, MessageConversionContext conversionContext, IServiceMessage message, HttpRequestMessage request)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // TODO: CompositeServiceMessageConverter.ToResponse: Determine how to support HttpStatus Created.
            var status = HttpStatusCode.OK;
            var reason = "OK";

            var messages = message.Messages?.ToArray();
            if (messages != null)
            {
                var severity = messages.GetSeverity();

                if (severity == MessageSeverity.Critical)
                {
                    status = HttpStatusCode.InternalServerError;
                    reason = "Internal Server Error";
                }
                else if (severity == MessageSeverity.Error)
                {
                    status = HttpStatusCode.BadRequest;
                    reason = "Bad Request";
                }
            }

            var response = new HttpResponseMessage(status)
            {
                RequestMessage = request,
                ReasonPhrase = reason,
            };

            var outgoingMessage = message.Clone();

            // TODO: CompositeMessageConverter.ToResponse: Use 'changed' for diagnostics.
            var changed = false;
            changed |= await this.Options.ContractConverter.ToResponse(
                context, conversionContext, message.Contract, request, outgoingMessage, response);

            changed |= await this.Options.CredentialsConverter.ToResponse(
                context, conversionContext, message.Credentials, request, outgoingMessage, response);

            changed |= await this.Options.NotificationConverter.ToResponse(
                context, conversionContext, message.Messages, request, outgoingMessage, response);

            changed |= await this.Options.IdentityConverter.ToResponse(
                context, conversionContext, message.Identity, request, outgoingMessage, response);

            changed |= await this.Options.ConnectionConverter.ToResponse(
                context, conversionContext, message.Connection, request, outgoingMessage, response);

            changed |= await this.Options.TenantConverter.ToResponse(
                context, conversionContext, message.Tenant, request, outgoingMessage, response);

            var json = await this.Options.Serializer.FromServiceMessage(context, conversionContext, outgoingMessage);

            var jsonData = json["data"];

            if (jsonData != null && !(jsonData is JObject || jsonData is JArray))
            {
                var errorMessage =
                    $"The Jali service routine implementation that handled url '{request.RequestUri.ToString()}' " +
                    $"returned an invalid data type. Value must convert either to a JavaScript object or an array " +
                    $"of JavaScript objects.";

                throw new InternalErrorException(errorMessage);
            }


            changed |= await this.Options.DataConverter.ToResponse(context, conversionContext, jsonData, message, request, response);

            var jsonString = await this.Options.Serializer.Serialize(context, conversionContext, json);

            response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            return response;
        }
    }
}
