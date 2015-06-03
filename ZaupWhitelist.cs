using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Plugins;
using SDG;
using Steamworks;

namespace ZaupWhitelist
{
    class ZaupWhitelist : RocketPlugin<ZaupWhitelistConfiguration>
    {
        public static ZaupWhitelist Instance;
        public WLDatabaseManager Database;
        public override Dictionary<string, string> DefaultTranslations
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {
                        "command_generic_invalid_parameter",
                        "Invalid format."
                    },
                    {
                        "command_generic_invalid_steamid",
                        "{0} is an invalid SteamID format."
                    },
                    {
                        "default_permit_message",
                        "You have added {0} {1} to the whitelist."
                    },
                    {
                        "default_unpermit_message",
                        "You have removed {0} from the whitelist."
                    },
                    {
                        "no_player_found_unpermit",
                        "No player found that has id {0}."
                    },
                    {
                        "update_whitelist_mysql_message",
                        "Whitelist up to date from Mysql Database."
                    }
                };
            }
        }

        protected override void Load()
        {
            ZaupWhitelist.Instance = this;
            this.Database = new WLDatabaseManager();
            this.UpdateWhitelist();
        }
        protected void UpdateWhitelist()
        {
            if (!ZaupWhitelist.Instance.Configuration.AddtoGameWhitelist) return; // Do nothing as we are actively using the whitelist in game
            List<WhitelistRow> whitelist = ZaupWhitelist.Instance.Database.GetWhitelist();
            foreach (WhitelistRow row in whitelist)
            {
                SteamWhitelist.whitelist(row.steamId, row.name, row.modId);
            }
            Logger.Log(ZaupWhitelist.Instance.Translate("update_whitelist_mysql_message", new object[0]));
        }
    }
}
