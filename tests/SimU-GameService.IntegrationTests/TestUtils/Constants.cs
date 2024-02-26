namespace SimU_GameService.IntegrationTests.TestUtils;

#pragma warning disable CA2211 // Non-constant fields should not be visible

public static class Constants
{
    public static class ConnectionStrings
    {
        public const string ProdDBConnectionString = "Server=simudb.c7dymeo5dq31.us-east-2.rds.amazonaws.com;Database=simudb-prod;Port=5432;User=alan;Password=simyoudev;";
        public const string DevDBConnectionString = "Server=localhost;Database=simudb-dev;Port=5432;User Id=postgres;Password=postgres;";
    }

    public static class AgentService
    {
        public const string BaseUri = "https://sim-you-lll-service.victoriousrock-3d13ba09.eastus2.azurecontainerapps.io/api";
    }

    public static class Authentication
    {
        public const string TokenUri = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyCd_MWhRziW2mPKEcHR-7aJ5JcrJPyN1lU";
        public const string ValidIssuer = "https://securetoken.google.com/simu-27c33";
        public const string Audience = "simu-27c33";
    }

    public static class User
    {
        public const string TestUsername = "TestUsername";
        public static string TestEmail = $"{TestUsername}{DateTime.Now.Ticks}@SimU.com";
        public const string TestPassword = "TestPassword";
    }

    public static class Routes
    {
        public const string BaseUri = "http://localhost:5000";

        public static class UnityHub
        {
            public const string Route = "/unity";
        }

        public static class Authentication
        {
            public const string BaseUri = "/authentication";
            public const string RegisterUser = $"{BaseUri}/register";
            public const string LoginUser = $"{BaseUri}/login";
            public const string LogoutUserPrefix = $"{BaseUri}/logout";
        }

        public static class Users
        {
            public const string BaseUri = "/users";
            public const string GetUserPrefix = $"{BaseUri}";
        }
    }
}

#pragma warning restore CA2211 // Non-constant fields should not be visible