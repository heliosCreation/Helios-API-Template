namespace Template.API.Contract
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version + "/";

        public static class Category
        {
            public const string Controller = nameof(Category);
            public const string EndpointBase = Base + Controller + "/";

            public const string GetAll = EndpointBase + "all";

            public const string GetById = EndpointBase + "{id}/Event/{includeHistory}";

            public const string Create = EndpointBase;

            public const string Update = EndpointBase + "{id}";

            public const string Delete = EndpointBase + "{id}";
        }

        public static class Event
        {
            public const string Controller = nameof(Event);
            public const string EndpointBase = Base + Controller + "/";

            public const string GetAll = EndpointBase + "all";

            public const string GetById = EndpointBase + "{id}";

            public const string ExportToCsv = EndpointBase + "CsvExport";

            public const string Create = EndpointBase;

            public const string Update = EndpointBase + "{id}";

            public const string Delete = EndpointBase + "{id}";
        }

        public static class Account
        {
            public const string Controller = nameof(Account);
            public const string EndpointBase = Base + Controller + "/";

            public const string Authenticate = EndpointBase + "authenticate";

            public const string Register = EndpointBase + "register";

            public const string ConfirmEmail = EndpointBase + "confirmEmail";
        }
    }
}
