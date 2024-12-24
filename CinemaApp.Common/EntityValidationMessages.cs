namespace CinemaApp.Common
{    
    public static class EntityValidationMessages
    {
        public static class Event
        {            
            public const string TitleRequired = "Event title is required.";
            public const string TitleMinLengthMessage = "Event title must be at least {0} characters long.";
            public const string TitleMaxLengthMessage = "Event title can be at most {0} characters long.";

            public const string DescriptionRequired = "Event description is required.";
            public const string DescriptionMinLengthMessage = "Event description must be at least {0} characters long.";
            public const string DescriptionMaxLengthMessage = "Event description can be at most {0} characters long.";

            public const string DateRequired = "Event date is required.";

            public const string LocationRequired = "Event location is required.";
            public const string LocationMinLengthMessage = "Event location must be at least {0} characters long.";
            public const string LocationMaxLengthMessage = "Event location can be at most {0} characters long.";

            public const string DistanceRequired = "Event distance is required.";

            public const string GroupIdRequired = "Group ID is required.";
        }

        public static class Group
        {            
            public const string NameRequired = "Group name is required.";
            public const string NameMinLengthMessage = "Group name must be at least {0} characters long.";
            public const string NameMaxLengthMessage = "Group name can be at most {0} characters long.";

            public const string DescriptionRequired = "Group description is required.";
            public const string DescriptionMinLengthMessage = "Group description must be at least {0} characters long.";
            public const string DescriptionMaxLengthMessage = "Group description can be at most {0} characters long.";

            public const string LocationRequired = "Group location is required.";
            public const string LocationMinLengthMessage = "Group location must be at least {0} characters long.";
            public const string LocationMaxLengthMessage = "Group location can be at most {0} characters long.";

            public const string AdminIdRequired = "Admin ID is required.";
        }

        public static class Notification
        {
            public const string MessageRequired = "Notification message is required.";
            public const string MessageMinLengthMessage = "Notification message must be at least {0} characters long.";
            public const string MessageMaxLengthMessage = "Notification message can be at most {0} characters long.";

            public const string UserIdRequired = "User ID is required.";
            public const string EventIdRequired = "Event ID is required.";
        }

        public static class ApplicationUser
        {
            public const string BioMinLengthMessage = "The Bio must be at least {0} characters long.";
            public const string BioMaxLengthMessage = "The Bio can be at most {0} characters long.";
        }
    }
}
