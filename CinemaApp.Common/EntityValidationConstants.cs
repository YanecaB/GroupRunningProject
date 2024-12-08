namespace CinemaApp.Common
{
    public static class EntityValidationConstants
    {
        public static class Group
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

            public const int LocationMinLength = 3;
            public const int LocationMaxLength = 85;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 500;

            public const string ReleaseDateFormat = "MM/yyyy";
        }

        public static class Event
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 50;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 500;

            public const int LocationMinLength = 3;
            public const int LocationMaxLength = 85;

            public const string DateFormat = "dd/MM/yyyy";
        }

        public static class Notification
        {
            public const int MessageMinLength = 10;
            public const int MessageMaxLength = 500;

            public const string DateFormat = "dd/MM/yyyy HH:mm";


            public const string Message = "Reminder: The event {0} is happening tomorrow!";
        }
    }
}
