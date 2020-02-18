namespace AuditService.Infrastructure
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Audits
        {
            public const string GetAll = Base + "/audits";
            public const string Get = Base + "/audits/{id}";
            public const string Create = Base + "/audits";
        }
        
        
    }

}