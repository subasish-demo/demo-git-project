using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.DataUtilities.SQL
{
    public class SqlCredentials
    {
        public string userName {  get { return user.userName; } }
        public string domainName { get { return user.domainName; } }
        public string password { get { return user.password; } }

        public Credentials user;
        private string server;
        private string database;
        public bool useImpersonation { get; private set; }

        /// <summary>
        /// Connect to database by impersonating user with access
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domainName"></param>
        /// <param name="password"></param>
        /// <param name="server"></param>
        /// <param name="database"></param>
        public SqlCredentials(string server, string database, string userName, string password, string domainName)
        {
            user = new Credentials(userName, password, domainName);
            this.server = server;
            this.database = database;
            useImpersonation = true;
        }

        /// <summary>
        /// Connect to database using SQL user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="server"></param>
        /// <param name="database"></param>
        public SqlCredentials(string server, string database, string userName, string password )
        {
            user = new Credentials(userName, password);
            this.server = server;
            this.database = database;
            useImpersonation = false;
        }

        public string GetConnectionString()
        {
            if (useImpersonation)
            {
                return $"Server={server};Database={database};Trusted_Connection=True;";
            }
            else
            {
                return $"Server={server};Database={database}; User id={userName}; Password={password};";
            }
        }
    }
}
