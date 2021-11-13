namespace StaticAssets
{
    public static class WebAPI_Headers
    {
        /// <summary>
        /// Used in Authentication. The Backer Token is needed to register a new user whose role is
        /// either Admin or Vendor. The Backer Token is used to verify if the backer for a new user
        /// is itself an admin role.
        /// </summary>
        public const string backerToken = "Backer-Bearer-Token";

        public const string bearerToken = "Bearer-Token";
    }
}