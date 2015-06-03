using System;
using System.Collections.Generic;
using System.Linq;
using I18N.West;
using MySql.Data.MySqlClient;
using Rocket.Unturned.Logging;
using Steamworks;
using System.Text;
using System.Threading.Tasks;

namespace ZaupWhitelist
{
    class WLDatabaseManager
    {
        public WLDatabaseManager()
		{
			new CP1250();
			this.CheckSchema();
		}
        internal void CheckSchema()
        {
            try
            {
                MySqlConnection mySqlConnection = this.createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = "show tables like '" + ZaupWhitelist.Instance.Configuration.DatabaseTableName + "'";
                mySqlConnection.Open();
                if (mySqlCommand.ExecuteScalar() == null)
                {
                    mySqlCommand.CommandText = "CREATE TABLE `" + ZaupWhitelist.Instance.Configuration.DatabaseTableName + "` (`steamId` varchar(32) NOT NULL,`name` varchar(32) NOT NULL,`modId` varchar(32) NOT NULL DEFAULT '11111111111111111',PRIMARY KEY (`steamId`))";
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private MySqlConnection createConnection()
		{
			MySqlConnection result = null;
			try
			{
				if (ZaupWhitelist.Instance.Configuration.DatabasePort == 0)
				{
					ZaupWhitelist.Instance.Configuration.DatabasePort = 3306;
				}
				result = new MySqlConnection(string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", new object[]
				{
					ZaupWhitelist.Instance.Configuration.DatabaseAddress,
					ZaupWhitelist.Instance.Configuration.DatabaseName,
					ZaupWhitelist.Instance.Configuration.DatabaseUsername,
					ZaupWhitelist.Instance.Configuration.DatabasePassword,
					ZaupWhitelist.Instance.Configuration.DatabasePort
				}));
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
			return result;
		}
        public bool IsWhitelisted(CSteamID playerid)
        {
            bool whitelisted = false;
            ulong id = 0;
            try
            {
                MySqlConnection mySqlConnection = this.createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat(new string[]
				{
					"select `steamId` from `",
					ZaupWhitelist.Instance.Configuration.DatabaseTableName,
					"` where `steamId` = '",
					playerid.ToString(),
					"';"
				});
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();
                if (obj != null)
                {
                    ulong.TryParse(obj.ToString(), out id);
                    if (id > 0) whitelisted = true;
                }
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return whitelisted;
        }
        public bool RemWhitelist(CSteamID playerid)
        {
            bool result = false;
            try
            {
                MySqlConnection mySqlConnection = this.createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat(new string[]
				{
					"DELETE FROM `",
					ZaupWhitelist.Instance.Configuration.DatabaseTableName,
					"` where `steamId` = '",
					playerid.ToString(),
					"';"
				});
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();
                if (obj != null)
                {
                    result = true;
                }
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return result;
        }
        public bool AddWhitelist(CSteamID playerid, string name, CSteamID modid)
        {
            bool result = false;
            try
            {
                MySqlConnection mySqlConnection = this.createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat(new string[]
				{
					"INSERT INTO `",
					ZaupWhitelist.Instance.Configuration.DatabaseTableName,
					"` (steamId, name, modId) VALUES ('",
					playerid.ToString(),
					"', '",
                    name,
                    "', '", 
                    modid.ToString(),
                    "') ON DUPLICATE KEY UPDATE steamId=steamId;"
				});
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();
                if (obj != null)
                {
                    result = true;
                }
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return result;
        }
        public List<WhitelistRow> GetWhitelist()
        {
            List<WhitelistRow> whitelist = new List<WhitelistRow>();
            try
            {
                MySqlConnection mySqlConnection = this.createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat(new string[]
				{
					"SELECT * FROM `",
					ZaupWhitelist.Instance.Configuration.DatabaseTableName,
					"`;"
				});
                mySqlConnection.Open();
                MySqlDataReader obj = mySqlCommand.ExecuteReader();
                while(obj.Read())
                {
                    ulong sid = new ulong();
                    ulong mid = new ulong();
                    string name = "";
                    ulong.TryParse(obj["steamId"].ToString(), out sid);
                    name = obj["name"].ToString();
                    ulong.TryParse(obj["modId"].ToString(), out mid);
                    WhitelistRow row = new WhitelistRow((CSteamID)sid, name, (CSteamID)mid);
                    whitelist.Add(row);
                }
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return whitelist;
        }
    }
}
