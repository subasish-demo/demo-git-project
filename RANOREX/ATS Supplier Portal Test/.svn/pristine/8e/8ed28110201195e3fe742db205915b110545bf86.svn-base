using ATS.CodeLibrary.gcwebservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.ActiveDirectory
{
    public class ActiveDirectoryHelper : IDisposable
    {
        public ATSGlobalCatalogQuery ws { get; set; }
        
        /// <summary>
        /// Instantiate ATSGlobalCatalogQuery with default credentials and pre-authentication
        /// </summary>
        public ActiveDirectoryHelper()
        {
            ws = new ATSGlobalCatalogQuery
            {
                PreAuthenticate = true,
                UseDefaultCredentials = true
            };
        }

        /// <summary>
        /// Check if user is in group
        /// </summary>
        /// <param name="groupName">Is user in group with this name</param>
        /// <param name="emailAddress">optional, leave as empty string to check current user, otherwise enter email address and username to check different user</param>
        /// <param name="userName">optional, leave as empty string to check current user, otherwise enter email address and username to check different user</param>
        /// <returns>True if user is in group</returns>
        public bool IsUserGroupMember(string groupName, string emailAddress = "", string userName = "")
        {
            return ws.IsUserGroupMember(emailAddress, groupName, userName) == "true";
        }

        /// <summary>
        /// Awaitable, check if user is in group
        /// </summary>
        /// <param name="groupName">Is user in group with this name</param>
        /// <param name="emailAddress">optional, leave as empty string to check current user, otherwise enter email address to check different user</param>
        /// <param name="userName">optional, leave as empty string to check current user, otherwise enter account name to check different user</param>
        /// <returns>True if user is in group</returns>
        public async Task<bool> IsUserGroupMemberAsync(string groupName, string emailAddress = "", string userName = "")
        {
            return await Task.Run(() => { return IsUserGroupMember(groupName, emailAddress, userName); });
        }

        /// <summary>
        /// Get active directory property for current user
        /// </summary>
        /// <param name="propertyName">Name of property to retrieve</param>
        /// <returns>Value of property</returns>
        public string GetPropertyByAccountIDCurrentContext(string propertyName)
        {
            return ws.GetPropertyByAccountIDCurrentContext(propertyName);
        }

        /// <summary>
        /// Awaitable, get active directory property for current user
        /// </summary>
        /// <param name="propertyName">Name of property to retrieve</param>
        /// <returns>Value of property</returns>
        public async Task<string> GetPropertyByAccountIDCurrentContextAsync(string propertyName)
        {
            return await Task.Run(() => { return GetPropertyByAccountIDCurrentContext(propertyName); });
        }

        public void Dispose()
        {
            ws?.Dispose();
            ws = null;            
        }
    }
}
