using ATS.CodeLibrary.DataUtilities.SQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.Configuration
{
    public class ConfigurationHelper
    {
        SqlCredentials creds;
        public IDictionary<string, string> settings;
        public const string invalidPathMessage = "Application is being run from a different location than where it was intended.\r\nOperation Cancelled.";

        /// <summary>
        /// Gets all settings from app.config and verifies that the settings required to read from database are configured
        /// </summary>
        public ConfigurationHelper()
        {
            settings = GetAllLocalSettingsDecrypted();
            creds = GetCredentials(settings);
        }

        private static SqlCredentials GetCredentials(IDictionary<string, string> settings)
        {
            var keys = new[] { "atsConfigurationsServer", "atsConfigurationsDatabase", "atsConfigurationsUsername", "atsConfigurationsPassword" };
            var sb = new StringBuilder();
            foreach (var key in keys)
            {
                if (settings == null || !settings.ContainsKey(key))
                {
                    sb.Append(key + ", ");
                }
            }

            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                throw new Exception("app.config is missing the following keys: " + sb.ToString());
            }
            
            return new SqlCredentials(settings["atsConfigurationsServer"], settings["atsConfigurationsDatabase"], settings["atsConfigurationsUsername"], settings["atsConfigurationsPassword"]);
        }

        #region non static members

        /// <summary>
        /// Get settings from ATSConfigurations table for the specified applicationID
        /// </summary>
        /// <param name="applicationID">Application id as it appears in the ATSConfigurations.tblConfigurationProperties</param>
        /// <returns>Dictionary keyed on property_name with values representing property_value</returns>
        public IDictionary<string, string> GetSettingsFromDatabaseDecrypted(string applicationID)
        {
            string sqlStatement = "SELECT [property_name],[property_value] FROM [tblConfigurationProperties] with (NOLOCK) where application_id = @appID";
            var ds = SqlHelper.ExecuteQuery(creds, sqlStatement, new Dictionary<string, object>() { { "@appID", applicationID } });
            return ds.Tables[0].AsEnumerable().ToDictionary(x => x["property_name"].ToString(), x => DataUtilities.Cryptography.CryptographyHelper.DecryptString(x["property_value"].ToString()));
        }

        public bool IsValidExecutionPath(string applicationID)
        {
            var appSettings = GetSettingsFromDatabaseDecrypted(applicationID);
            return appSettings["application_path"].Equals(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// Get local settings and database settings for current application. Local settings must contain key called applicationID and database settings must have application_id equal to the corresponding value. If duplicate keys exist then local settings will be returned
        /// </summary>
        /// <returns>All settings for current application decrypted</returns>
        public IDictionary<string, string> AddSettingsDecrypted()
        {
            var databaseSettings = GetSettingsFromDatabaseDecrypted(settings["applicationID"], creds);
            var result = settings.Merge(databaseSettings);
            return result;
        }

        #endregion


        #region static members

        /// <summary>
        /// Read configuration from app.config
        /// </summary>
        /// <param name="name">Name of app setting to return</param>
        /// <returns>Value of app setting</returns>
        public static string GetLocalConfiguration(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// Read multiple configurations from app.config
        /// </summary>
        /// <param name="names">Names of app setting to return</param>
        /// <returns>Values of app setting</returns>
        public static IDictionary<string, string> GetAllLocalSettingsDecrypted()
        {
            var values = new Dictionary<string, string>();
            var names = ConfigurationManager.AppSettings.AllKeys;
            if (names == null)
                return null;

            foreach (var name in names)
            {
                values.Add(name, DataUtilities.Cryptography.CryptographyHelper.DecryptString(ConfigurationManager.AppSettings[name]));
            }
            return values;
        }

        /// <summary>
        /// Get local settings and database settings for current application. Local settings must contain key called applicationID and database settings must have application_id equal to the corresponding value. If duplicate keys exist then local settings will be returned
        /// </summary>
        /// <param name="names">Names of app setting to return</param>
        /// <returns>Values of app setting</returns>
        public static IDictionary<string, string> GetAllSettingsDecrypted(string applicationID)
        {
            var settings = GetAllLocalSettingsDecrypted();
            var creds = GetCredentials(settings);
            var databaseValues = GetSettingsFromDatabaseDecrypted(applicationID, creds);           

            return settings.Merge(databaseValues);
        }

        public static bool IsValidExecutionPath(IDictionary<string, string> settings)
        {
            return settings["application_path"].Equals(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsValidExecutionPath(IReadOnlyDictionary<string, string> settings)
        {
            return settings["application_path"].Equals(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get settings from ATSConfigurations table for the specified applicationID
        /// </summary>
        /// <param name="applicationID">Application id as it appears in the ATSConfigurations.tblConfigurationProperties</param>
        /// <param name="creds">SqlCredentials for connecting to the Configuration Database</param>
        /// <returns>Dictionary keyed on property_name with decrypted values representing property_value</returns>
        public static IDictionary<string, string> GetSettingsFromDatabaseDecrypted(string applicationID, SqlCredentials creds)
        {
            string sqlStatement = "SELECT [property_name],[property_value] FROM [tblConfigurationProperties] with (NOLOCK) where application_id = @appID";
            var ds = SqlHelper.ExecuteQuery(creds, sqlStatement, new Dictionary<string, object>() { { "@appID", applicationID } });
            return ds.Tables[0].AsEnumerable().ToDictionary(x => x["property_name"].ToString(), x => DataUtilities.Cryptography.CryptographyHelper.DecryptString(x["property_value"].ToString()));
        }

        #endregion


        
    }
    static class localExtensions
    {
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> dictA, IDictionary<TKey, TValue> dictB)
    where TValue : class
        {
            return dictA.Keys.Union(dictB.Keys).ToDictionary(k => k, k => dictA.ContainsKey(k) ? dictA[k] : dictB[k]);
        }
    }
}
