using System.ComponentModel;

namespace TaskManager.Api
{
    public static class EnvironmentVariables
    {
        public static readonly string JwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        public static readonly string JwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCER");
        public static readonly string JwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        public static readonly int TokenLifeTimeInMinutes = ParseEnvironmentVariable("TOKEN_LIFE_TIME_IN_MINUTES", 60);
        public static readonly string DbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        public static readonly string SwaggerEndpointUrl = "/swagger/v1/swagger.json";
        public static readonly string SwaggerEndpointTitle = "Task Manager API V1";

        public static T ParseEnvironmentVariable<T>(string envName, T defaultValue) where T : IConvertible
        {
            var env = Environment.GetEnvironmentVariable(envName);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            return string.IsNullOrEmpty(env)
                ? defaultValue
                : (T)converter.ConvertFromString(env);
        }
    }
}
