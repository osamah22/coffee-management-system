using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Api.Constants;

public static class ApiEndpoints
{
    public static class Coffee
    {
        public static class V1
        {
            private const string Base = "/api/v1/coffees";
            public const string GetAll = $"{Base}";
            public const string Get = $"{Base}/{{idOrSlug}}";
            public const string Create = $"{Base}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
    }
}