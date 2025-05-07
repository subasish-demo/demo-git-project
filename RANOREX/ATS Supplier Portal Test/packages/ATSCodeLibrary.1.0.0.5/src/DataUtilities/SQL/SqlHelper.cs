using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.DataUtilities.SQL
{
    public class SqlHelper : IDisposable
    {
        public SqlConnection sqlConnection { get; private set; }

        /// <summary>
        /// Open connection to database using SqlCredentials and keep open until disposed
        /// </summary>
        /// <param name="credentials">SqlCredentials with valid username and password for SQL authentication or username, domain, and password for impersonation</param>
        public SqlHelper(SqlCredentials credentials)
        {
            OpenConnection(credentials);
        }

        /// <summary>
        /// Open connection to database and keep open until disposed
        /// </summary>
        /// <param name="server">Name of server</param>
        /// <param name="database">Database that will be read from</param>
        /// <param name="userName">SQL Username</param>
        /// <param name="password">SQL Password</param>
        public SqlHelper(string server, string database, string userName, string password)
        {
            var creds = new SqlCredentials(server, database, userName, password );
            OpenConnection(creds);
        }

        /// <summary>
        /// Open connection to database and keep open until disposed
        /// </summary>
        /// <param name="server">Name of server</param>
        /// <param name="database">Database that will be read from</param>
        /// <param name="userName">Active directory username</param>
        /// <param name="password">Active directory password</param>
        /// <param name="domain">Active directory domain</param>
        public SqlHelper(string server, string database, string userName, string password, string domain)
        {
            var creds = new SqlCredentials(userName, domain, password, server, database);
            OpenConnection(creds);
        }

        /// <summary>
        /// Execute a SQL statement and return the corresponding dataset
        /// </summary>
        /// <param name="creds">Credentials to connect with impersonation or SQL authentication</param>
        /// <param name="sqlStatement">SQL Statement that will be executed</param>
        /// <param name="parameters">Parameters that will be added to the command</param>
        /// <returns>Dataset corresponding to sqlStatement</returns>
        public static DataSet ExecuteQuery(SqlCredentials creds, string sqlStatement, Dictionary<string, object> parameters = null)
        {
            using (var sqlHelper = new SqlHelper(creds))
            {
                return ExecuteQuery(sqlHelper, sqlStatement, parameters);
            }
        }

        /// <summary>
        /// Execute a SQL statement and return the corresponding dataset
        /// </summary>
        /// <param name="sqlHelper">SqlHelper with open connection to the correct database</param>
        /// <param name="sqlStatement">SQL Statement that will be executed</param>
        /// <param name="parameters">Parameters that will be added to the command</param>
        /// <returns>Dataset corresponding to sqlStatement</returns>
        public static DataSet ExecuteQuery(SqlHelper sqlHelper, string sqlStatement, Dictionary<string, object> parameters = null)
        {
            SqlCommand command = CreateCommand(sqlHelper, sqlStatement, parameters);

            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataSet dt = new DataSet();
            command.CommandTimeout = 0;

            sda.Fill(dt);

            return dt;
        }

        /// <summary>
        /// Execute a SQL statement and return the corresponding dataset
        /// </summary>
        /// <param name="creds">Credentials to connect with impersonation or SQL authentication</param>
        /// <param name="sqlStatement">SQL Statement that will be executed</param>
        /// <param name="parameters">Parameters that will be added to the command</param>
        public static void ExecuteNonQuery(SqlCredentials creds, string sqlStatement, Dictionary<string, object> parameters = null)
        {
            using (var sqlHelper = new SqlHelper(creds))
            {
                ExecuteNonQuery(sqlHelper, sqlStatement, parameters);
            }
        }

        /// <summary>
        /// Execute a SQL statement and return the corresponding dataset
        /// </summary>
        /// <param name="sqlHelper">SqlHelper with open connection to the correct database</param>
        /// <param name="sqlStatement">SQL Statement that will be executed</param>
        /// <param name="parameters">Parameters that will be added to the command</param>
        public static void ExecuteNonQuery(SqlHelper sqlHelper, string sqlStatement, Dictionary<string, object> parameters = null)
        {
            SqlCommand command = CreateCommand(sqlHelper, sqlStatement, parameters);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute a SQL statement and return the corresponding scalar value
        /// </summary>
        /// <param name="creds">Credentials to connect with impersonation or SQL authentication</param>
        /// <param name="sqlStatement">SQL Statement that will be executed</param>
        /// <param name="parameters">Parameters that will be added to the command</param>
        /// <returns>Scalar result of sqlStatement</returns>
        public static object ExecuteScalar(SqlCredentials creds, string sqlStatement, Dictionary<string, object> parameters = null)
        {
            using (var sqlHelper = new SqlHelper(creds))
            {
                return ExecuteScalar(sqlHelper, sqlStatement, parameters);
            }
        }

        /// <summary>
        /// Execute a SQL statement and return the corresponding scalar value
        /// </summary>
        /// <param name="sqlHelper">SqlHelper with open connection to the correct database</param>
        /// <param name="sqlStatement">SQL Statement that will be executed</param>
        /// <param name="parameters">Parameters that will be added to the command</param>
        /// <returns>Scalar result of sqlStatement</returns>
        public static object ExecuteScalar(SqlHelper sqlHelper, string sqlStatement, Dictionary<string, object> parameters = null)
        {
            SqlCommand command = CreateCommand(sqlHelper, sqlStatement, parameters);
            return command.ExecuteScalar();
        }

        private static SqlCommand CreateCommand(SqlHelper sqlHelper, string sqlStatement, Dictionary<string, object> parameters)
        {
            var command = new SqlCommand(sqlStatement, sqlHelper.sqlConnection);
            command.CommandType = CommandType.Text;
            if (parameters != null)
            {
                foreach (var pair in parameters)
                {
                    command.Parameters.AddWithValue(pair.Key, pair.Value);
                }
            }

            return command;
        }


        /// <summary>
        /// Open connection to database using SqlCredentials and keep open until CloseConnection is called
        /// </summary>
        /// <param name="credentials">SqlCredentials with valid username and password for SQL authentication or username, domain, and password for impersonation</param>
        private SqlConnection OpenConnection(SqlCredentials credentials)
        {
            sqlConnection = new SqlConnection(credentials.GetConnectionString());
            if (sqlConnection.State != ConnectionState.Open)
            {
                if (credentials.useImpersonation)
                {

                    using (new Impsersonation.ImpersonationHelper(credentials))
                    {
                        sqlConnection.Open();
                    }
                }
                else
                {
                    sqlConnection.Open();
                }
            }

            return sqlConnection;
        }

        /// <summary>
        /// Open connection to database and keep open until close connection is called
        /// </summary>
        /// <param name="server">Name of server</param>
        /// <param name="database">Database that will be read from</param>
        /// <param name="userName">SQL Username</param>
        /// <param name="password">SQL Password</param>
        private SqlConnection OpenConnection(string server, string database, string userName, string password)
        {
            var creds = new SqlCredentials(userName, password, server, database);
            return OpenConnection(creds);
        }

        /// <summary>
        /// Open connection to database and keep open until close connection is called
        /// </summary>
        /// <param name="server">Name of server</param>
        /// <param name="database">Database that will be read from</param>
        /// <param name="userName">Active directory username</param>
        /// <param name="password">Active directory password</param>
        /// <param name="domain">Active directory domain</param>
        private SqlConnection OpenConnection(string server, string database, string userName, string password, string domain)
        {
            var creds = new SqlCredentials(userName, domain, password, server, database);
            return OpenConnection(creds);
        }        

        public void Dispose()
        {
            CloseConnection();
        }

        private void CloseConnection()
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }


        public static int ExecuteScalar(SqlCredentials creds, string sqlStatement, Dictionary<string, object> parameters, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            if (transaction == null)
            {
                return int.Parse(SqlHelper.ExecuteScalar(creds, sqlStatement, parameters).ToString());
            }
            else
            {
                SetConnection(creds, connection, out bool closeConnection, out SqlConnection conn);

                try
                {
                    SqlCommand command = CreateCommand(sqlStatement, parameters, transaction, conn);
                    return int.Parse(command.ExecuteScalar().ToString());
                }
                finally
                {
                    FinalizeConnection(closeConnection, conn);
                }
            }
        }

        public static DataTable ExecuteQuery(SqlCredentials creds, string sqlStatement, Dictionary<string, object> parameters, SqlConnection connection, SqlTransaction transaction)
        {
            if (transaction == null)
            {
                return SqlHelper.ExecuteQuery(creds, sqlStatement).Tables[0];
            }
            else
            {
                SetConnection(creds, connection, out bool closeConnection, out SqlConnection conn);

                try
                {
                    SqlCommand command = CreateCommand(sqlStatement, parameters, transaction, conn);
                    var reader = command.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
                finally
                {
                    FinalizeConnection(closeConnection, conn);
                }
            }
        }

        private static SqlCommand CreateCommand(string sqlStatement, Dictionary<string, object> parameters, SqlTransaction transaction, SqlConnection conn)
        {
            var command = new SqlCommand(sqlStatement, conn, transaction);
            command.CommandType = CommandType.Text;
            foreach (var pair in parameters)
            {
                command.Parameters.AddWithValue(pair.Key, pair.Value);
            }

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            return command;
        }

        public static void ExecuteNonQuery(SqlCredentials creds, string sqlStatement, Dictionary<string, object> parameters, SqlConnection connection, SqlTransaction transaction)
        {
            if (transaction == null)
            {
                SqlHelper.ExecuteNonQuery(creds, sqlStatement, parameters);
            }
            else
            {
                bool closeConnection;
                SqlConnection conn;
                SetConnection(creds, connection, out closeConnection, out conn);

                try
                {

                    SqlCommand command = CreateCommand(sqlStatement, parameters, transaction, conn);
                    command.ExecuteNonQuery();
                }
                finally
                {
                    FinalizeConnection(closeConnection, conn);
                }
            }
        }

        private static void FinalizeConnection(bool closeConnection, SqlConnection conn)
        {
            if (closeConnection)
            {
                try
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
                finally
                {
                    conn.Dispose();
                }
            }
        }

        private static void SetConnection(SqlCredentials creds, SqlConnection connection, out bool closeConnection, out SqlConnection conn)
        {
            closeConnection = false;
            conn = connection;
            if (conn == null)
            {
                closeConnection = true;
                conn = new SqlConnection { ConnectionString = creds.GetConnectionString() };
            }
        }

    }
}
