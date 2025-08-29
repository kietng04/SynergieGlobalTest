namespace News.Api.Utils;

public static class Constants
{
    public static class ApiVersions
    {
        public const string V1 = "1.0";
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Editor = "Editor";
    }

    public static class ErrorMessages
    {
        public const string NotFound = "Resource not found";
        public const string Unauthorized = "Unauthorized access";
        public const string BadRequest = "Invalid request";
        public const string InternalServerError = "An internal server error occurred";
    }

    public static class DefaultValues
    {
        public const string DefaultHelloMessage = "Hello World! Welcome to News API 🌍";
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
    }
}
