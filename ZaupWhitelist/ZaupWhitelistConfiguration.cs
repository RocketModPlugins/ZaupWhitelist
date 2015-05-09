using System;
using Rocket.RocketAPI;

namespace ZaupWhitelist
{
    public class ZaupWhitelistConfiguration : IRocketConfiguration
    {
        public string DatabaseAddress;
		public string DatabaseUsername;
		public string DatabasePassword;
		public string DatabaseName;
		public string DatabaseTableName;
		public int DatabasePort;
        public ulong DefaultWhitelisterSteamId;
        public bool AddtoGameWhitelist;
        public IRocketConfiguration DefaultConfiguration
        {
            get
            {
                return new ZaupWhitelistConfiguration
                {
                    DatabaseAddress = "localhost",
		            DatabaseUsername = "unturned",
		            DatabasePassword = "password",
		            DatabaseName = "unturned",
		            DatabaseTableName = "whitelist",
		            DatabasePort = 3306,
                    DefaultWhitelisterSteamId = 11111111111111111,
                    AddtoGameWhitelist = true
                };
            }
        }
    }
}
