namespace HumanCapitalManagement.Common
{
    public class ModelValidationConstants
    {
        public static class Project
        {
            public const int TitleMaxLength = 40;
            public const int TitleMinLength = 10;

            public const int DescriptionMaxLength = 100;
            public const int DescriptionMinLength = 10;
        }

        public static class Town
        {
            public const int NameMaxLength = 30;
            public const int NameMinLength = 4;
        }

        public static class Employee
        {
            public const int EmailMaxLength = 60;
            public const int EmailMinLength = 10;

            public const int UserNameMaxLength = 20;
            public const int UserNameMinLength = 4;

            public const int FirstNameMaxLength = 20;
            public const int FirstNameMinLength = 2;

            public const int MiddleNameMaxLength = 20;
            public const int MiddleNameMinLength = 2;

            public const int LastNameMaxLength = 20;
            public const int LastNameMinLength = 2;

            public const int PasswordMaxLength = 20;
            public const int PasswordMinLength = 6;
        }

        public static class Role
        {
            public const int NameMaxLength = 20;
            public const int NameMinLength = 2;
        }
    }
}