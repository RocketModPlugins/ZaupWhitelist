# ZaupWhitelist
Whitelist Mysql Mod for Unturned requiring Rocket

Per request, I created a way for the whitelist to be used in a mysql database. It will override permit and unpermit commands, and makes them available to be used in the console. You can also add to the whitelist directly into the mysql, and the whitelist in game will be updated when the server restarts. Zip on the repo comes with a default config file and translation file.

This will not include into the mysql table anything already in the whitelist. If you want them in the mysql, you need to add them into the mysql or call the command again.