namespace Api.Constants;

public static class AuthConstants
{
    public static class Roles
    {
        public const string Admin = "admin";
        public const string Manager = "manager";
    }
    public static class Policies
    {
        public const string CanManagePermissions = "CanManagePermissions";
        public const string CanManageUsers = "CanManageUsers";
        public const string CanManageCoffee = "CanManageCoffee";
    }
}