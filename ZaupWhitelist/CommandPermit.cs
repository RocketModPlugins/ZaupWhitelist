using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.RocketAPI;
using Rocket.Logging;
using SDG;
using Steamworks;


namespace ZaupWhitelist
{
    public class CommandPermit : IRocketCommand
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
        public void Execute(RocketPlayer playerid, string info)
        {
            bool console = (playerid == null) ? true : false;
            string message = "";
            string[] command = Parser.getComponentsFromSerial(info, '/');
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
            SteamWhitelist.whitelist((CSteamID)pcsteamid, command[1], mod);
            ZaupWhitelist.Instance.Database.AddWhitelist((CSteamID)pcsteamid, command[1], mod);
            message = ZaupWhitelist.Instance.Translate("default_permit_message", new object[] {
                pcsteamid.ToString(),
                command[1]
            });
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
                RocketChatManager.Say(caller, message);
            }
        } 
    }
}
