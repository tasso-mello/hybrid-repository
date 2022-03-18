namespace core.hybrid.repository.Utilities
{
    using Newtonsoft.Json;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Reflection;

    public static class Extensions
    {
        #region public attributes

        public static Guid _userId;

        #endregion

        #region String Conversions

        public static string ToStringObject(this object value, string property)
        {
            PropertyInfo pi = value.GetType().GetProperty(property);
            return (string)(pi.GetValue(value, null));
        }

        public static object ToJasonObject(this string value)
        {
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value.Replace('=', ':')));
        }

        public static string ToSerializableObject(this string value, string oldString, string newString)
        {
            return value.Replace(oldString, newString);
        }

        public static string ToSplitSecontObject(this string value, string spliter)
        {
            try
            {
                return value.Split(spliter)[1];
            }
            catch (Exception)
            {
                return value.Split(spliter)[0];
            }

        }

        public static string ReadAuthorizationToken(this string token, string tag)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var stream = token.Split(" ")[1];
                var handler = new JwtSecurityTokenHandler();
                var jsonValue = handler.ReadJwtToken(stream).Claims.ToList().FirstOrDefault(c => c.Type == tag)?.Value;
                return jsonValue;
            }
            else
                return string.Empty;
        }

        #endregion
    }
}
