﻿namespace CinemaApp.Common
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

            public const string MessageForEvents = "Reminder: The event {0} is happening tomorrow!";

            public const string MessageForFriendRequests = "Friend Request: {0} wants you to become friends!";

            public const string MessageForConfirmedRequests = "{0} confirmed your friend request!";
        }

        public static class RankList
        {
            public const int PageSizeConstant = 2;
        }

        public static class ApplicationUser
        {
            public const string EmptyBioMessage = "The Bio is empty! :o";

            public const int UsernameMinLength = 1;
            public const int UsernameMaxLength = 40;

            public const int BioMinLength = 3;
            public const int BioMaxLength = 150;
        }
    }
}
