namespace Webstore.API.Domain;

public static class DomainConst
{
    public static class Cultures
    {
        public const string English = "en";
    }

    public static class FileStorage
    {
        public const long MaxImageBytes = 15L * 1024L * 1024L;

        public static class FolderKeys
        {
            public const string Vendors = "vendors";
            public const string Media = "media";
            public const string EmailOutbox = "email_outbox";
            public const string VendorMedia = "vendor-media";
            public const string VendorTeam = "vendor-team";
            public const string UserAvatars = "user-avatars";
            public const string EventPhotos = "event-photos";
        }
    }

    public static class IdentityRoles
    {
        public const string SysAdmin = "SystemAdministrator";
        public const string Admin = "Administrator";
        public const string Vendor = "Vendor";
        public const string Customer = "Customer";
    }
}
