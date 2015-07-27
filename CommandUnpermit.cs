using System;
using System.Collections.Generic;
using System.Linq;

using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;


namespace ZaupWhitelist
{
    public class CommandUnpermit : IRocketCommand
    {
        public bool AllowFromConsole
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
        public List<string> Aliases
        {
            get { return new List<string>(); }
        }
        public List<string> Permissions
        {
            get { return new List<string>() { }; }
        }
        public void Execute(IRocketPlayer caller, string[] info)
        {
            bool console = (caller is ConsolePlayer);
            UnturnedPlayer playerid = (UnturnedPlayer)caller;
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
                if (ZaupWhitelist.Instance.Configuration.Instance.AddtoGameWhitelist)
                    SteamWhitelist.unwhitelist(playerid.CSteamID);
                message = ZaupWhitelist.Instance.Translate("default_unpermit_message", new object[] {
                pcsteamid.ToString()
                });
            }
            this.sendMessage(message, console, playerid);
            return;
        }

        private void sendMessage(string message, bool console, UnturnedPlayer caller = null)
        {
            if (console)
            {
                Logger.Log(message);
            }
            else
            {
                UnturnedChat.Say(caller, message);
            }
        } 
    }
}
