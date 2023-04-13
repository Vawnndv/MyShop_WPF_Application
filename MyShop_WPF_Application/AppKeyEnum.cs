using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application
{
    class AppKeyEnum
    {
        public static string Username = "Username";
        public static string Password = "Password";

        public static string getValue(string key)
        {
            return ConfigurationManager.AppSettings[key]!;
        }

        public static void saveToConfig(string password, string username)
        {
            var config = ConfigurationManager.OpenExeConfiguration(
                        ConfigurationUserLevel.None);

            // save username to config
            config.AppSettings.Settings["Username"].Value = username;

            // encrypt password
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropy = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropy);
            }

            var cypherText = ProtectedData.Protect(
                passwordInBytes,
                entropy,
                DataProtectionScope.CurrentUser
            );

            // save encrypted password to config
            string passwordIn64 = Convert.ToBase64String(cypherText);
            string entropyIn64 = Convert.ToBase64String(entropy);
            config.AppSettings.Settings["Password"].Value = passwordIn64;
            config.AppSettings.Settings["Entropy"].Value = entropyIn64;

            config.Save(ConfigurationSaveMode.Full);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }


    }
}
