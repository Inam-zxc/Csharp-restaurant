namespace Restaurant.Api.Constants
{
    public abstract class Constants
    {
        public abstract class Roles
        {
            public const string User = "user";
            public const string Admin = "admin";
        }


    }

    public abstract class CustomClaimsType
    {
        public const string Username = "Username";
        public const string Roles = "Roles";
        public const string UserId = "Id";

    }
}