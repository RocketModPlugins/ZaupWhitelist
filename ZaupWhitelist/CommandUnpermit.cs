using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using Steamworks;


namespace ZaupWhitelist
{
    public class CommandUnpermit : IRocketCommand
    {
        public bool RunFromConsole
        {
            get
            {
                return true;
            }
        }
        public string Name
        {
            get
            {
                return "unpermit";
            }
        }
        public string Help
        {
            get
            {
                return "Removes a person to the whitelist.";
            }
        }
        public string Syntax
        {
            get
            {
                return "<steamid>";
            }
        }
        public void Execute(RocketPlayer playerid, string[] info)
        {
            bool console = (playerid == null) ? true : false;
            string message = "";
            if (info.Length == 0)
            {
                message = ZaupWhitelist.Instance.Translate("command_generic_invalid_parameter", new object[0]);
                this.sendMessage(message, console, playerid);
                return;
            }
            ulong pcsteamid;
            if (!ulong.TryParse(info[0], out pcsteamid))
            {
                message = ZaupWhitelist.Instance.Translate("command_generic_invalid_steamid", new object[] {
                    info
                });
                this.sendMessage(message, console, playerid);
                return;
            }
            if (!ZaupWhitelist.Instance.Database.IsWhitelisted((CSteamID)pcsteamid))
            {
                message = ZaupWhitelist.Instance.Translate("no_player_found_unpermit", pcsteamid.ToString());
                this.sendMessage(message, console, playerid);
                return;
            }
            else
            {
                ZaupWhitelist.Instance.Database.RemWhitelist((CSteamID)pcsteamid);
                if (ZaupWhitelist.Instance.Configuration.AddtoGameWhitelist)
                    SteamWhitelist.unwhitelist(playerid.CSteamID);
                message = ZaupWhitelist.Instance.Translate("default_unpermit_message", new object[] {
                pcsteamid.ToString()
                });
            }
            this.sendMessage(message, console, playerid);
            return;
        }

        private void sendMessage(string message, bool console, RocketPlayer caller = null)
        {
            if (console)
            {
                Logger.Log(message);
            }
            else
            {
                RocketChat.Say(caller, message);
            }
        } 
    }
}
