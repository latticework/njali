using System.Linq;
using System.Security;
using Jose;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jali.Secure
{
    /// <summary>
    ///     Provides authentication utility functions.
    /// </summary>
    internal static class AuthenticationOperations
    {
        // TODO: AuthenticationOperations: Encode/Decode Impersonator and Deputy claims also.
        // TODO: AuthenticationOperations: Nest encrypted full token inside signed client-side claims.
        // TODO: AuthenticationOperations: Support multiple claims of the same type.
        /// <summary>
        ///     Creates a JSON Web Token (JWT) for the specified user using the specified key.
        /// </summary>
        /// <param name="user">
        ///     The user to encode.
        /// </param>
        /// <param name="key">
        ///     A 256 byte encryption key.
        /// </param>
        /// <returns>
        ///     The JWT token.
        /// </returns>
        public static string Encode(ISecurityContext user, byte[] key)
        {
            var json = new JObject();

            foreach (var claim in user.User.Claims)
            {
                json.Add(new JProperty(claim.Type, claim.Value));
            }

            var jsonString = json.ToString(Formatting.None);

            var token = JWT.Encode(
                jsonString, key, JweAlgorithm.A256KW, JweEncryption.A256CBC_HS512, JweCompression.DEF);

            return token;
        }

        /// <summary>
        ///     Decodes a JSON Web Token (JWT) using the specified key
        /// </summary>
        /// <param name="token">
        ///     The JWT token.
        /// </param>
        /// <param name="key">
        ///     A 256 byte encryption key.
        /// </param>
        /// <returns>
        ///     The decoded user.
        /// </returns>
        public static ISecurityContext Decode(string token, byte[] key)
        {
            // See https://github.com/dvsekhvalnov/jose-jwt
            // Seealso https://github.com/jwt-dotnet/jwt

            string jsonPayload;
            try
            {
                jsonPayload = JWT.Decode(token, key);
            }
            catch (IntegrityException exception)
            {
                // TODO: JaliHttpRequestMessageExtensions.GetSecurityToken: Thow corrupted token crticial error.
                var message = $"Jali Server is unable to decode security token '{token}'.";
                throw new SecurityException(message, exception);
            }

            var json = JObject.Parse(jsonPayload);

            var claims = json.Properties().Select(p => new Claim(p.Name, p.Value.ToString()));
            var user = new SecurityContext(new SecurityIdentity(claims));

            return user;
        }
    }

}