namespace ATM.Core.Services.Authentication
{
    public static class AuthenticationSettings
    {
        /// <summary>
        /// The default value used for authentication scheme.
        /// </summary>
        public static string AuthenticationScheme { get; set; } = "Authentication";

        /// <summary>
        /// The default value used for external authentication scheme.
        /// </summary>
        public static string ExternalAuthenticationScheme { get; set; } = "ExternalAuthentication";

        /// <summary>
        /// The issuer that should be used for any claims that are created.
        /// </summary>
        public static string ClaimsIssuer { get; set; } = "ATM";

        /// <summary>
        /// The default value for the login path.
        /// </summary>
        public static string LoginPath { get; set; } = "/account/login";

        /// <summary>
        /// The default value used for the logout path.
        /// </summary>
        public static string LogoutPath { get; set; } = "/account/logoff";

        /// <summary>
        /// The default value for the access denied path.
        /// </summary>
        public static string AccessDeniedPath { get; set; } = "/error/denied";

        /// <summary>
        /// The default value of the return URL parameter.
        /// </summary>
        public static string ReturnUrlParameter { get; set; } = "";

        /// <summary>
        /// Gets a key to store external authentication errors to session.
        /// </summary>
        public static string ExternalAuthenticationErrorsSessionKey { get; set; } = "ATM.externalauth.errors";

        /// <summary>
        /// Gets the cookie name prefix
        /// </summary>
        public static string CookiePrefix { get; set; } = ".ATM";

        /// <summary>
        /// Gets a cookie name of the authentication
        /// </summary>
        public static string CookieAuthentication { get; set; } = ".Authentication";

        /// <summary>
        /// Gets a cookie name of the external authentication
        /// </summary>
        public static string CookieExternalAuthentication { get; set; } = ".ExternalAuthentication";
    }
}