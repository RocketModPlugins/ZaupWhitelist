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
    public class CommandPermit : IRocketCommand
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
                return "permit";
            }
        }
        public string Help
        {
            get
            {
                return "Adds a person to the whitelist.";
            }
        }
        public string Syntax
        {
            get
            {
                return "<steamid> <name>";
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
        public void Execute(IRocketPlayer caller, string[] command)
        {
            bool console = (caller is ConsolePlayer);
            UnturnedPlayer playerid = (UnturnedPlayer)caller;
            string message = "";
            if (command.Length != 2)
            {
                message = ZaupWhitelist.Instance.Translate("command_generic_invalid_parameter", new object[0]);
                this.sendMessage(message, console, playerid);
                return;
            }
            ulong pcsteamid;
            if (!ulong.TryParse(command[0], out pcsteamid))
            {
                message = ZaupWhitelist.Instance.Translate("command_generic_invalid_steamid", new object[] {
                    command[0]
                });
                this.sendMessage(message, console, playerid);
                return;
            }
            CSteamID mod = (playerid == null) ? new CSteamID(11111111111111111) : playerid.Player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID;
            if (ZaupWhitelist.Instance.Configuration.Instance.AddtoGameWhitelist)
                SteamWhitelist.whitelist((CSteamID)pcsteamid, command[1], mod); // We are using the game whitelist to add to game whitelist.
            ZaupWhitelist.Instance.Database.AddWhitelist((CSteamID)pcsteamid, command[1], mod);
            message = ZaupWhitelist.Instance.Translate("default_permit_message", new object[] {
                pcsteamid.ToString(),
                command[1]
            });
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
