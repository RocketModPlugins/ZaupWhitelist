using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;

namespace ZaupWhitelist
{
    class WhitelistRow
    {
        public CSteamID steamId;
        public string name;
        public CSteamID modId;

        public WhitelistRow(CSteamID steamId, string name, CSteamID modId)
        {
            this.steamId = steamId;
            this.name = name;
            this.modId = modId;
        }
    }
}
