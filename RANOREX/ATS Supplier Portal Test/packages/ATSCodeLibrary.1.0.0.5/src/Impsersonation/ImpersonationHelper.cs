using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Principal;
using System.Runtime.InteropServices;
using System.ComponentModel;
using ATS.CodeLibrary.DataUtilities.SQL;

namespace ATS.CodeLibrary.Impsersonation
{
    /// <summary>
    /// Provides a method for impersonating a domain user by providing credentials as string.
    /// </summary>
    public class ImpersonationHelper : IDisposable
    {
        private static string userName { get; set; }
        private static string domainName { get; set; }
        private static string password { get; set; }

        /// <summary>
        /// Set credentials for re-use without including them in the constructor.
        /// </summary>
        /// <param name="userName">The name of the user to act as.</param>
        /// <param name="domainName">The domain name of the user to act as.</param>
        /// <param name="password">The password of the user to act as.</param>
        public static void SetCredentials(string userName, string domainName, string password)
        {
            ImpersonationHelper.userName = userName;
            ImpersonationHelper.domainName = domainName;
            ImpersonationHelper.password = password;
        }
        
        /// <summary>
        /// Constructor. Starts the impersonation with the given credentials.
        /// Please note that the account that instantiates the ImpersonationHelper class
        /// needs to have the 'Act as part of operating system' privilege set.
        /// </summary>
        /// <param name="userName">The name of the user to act as.</param>
        /// <param name="domainName">The domain name of the user to act as.</param>
        /// <param name="password">The password of the user to act as.</param>
        public ImpersonationHelper(string userName, string domainName, string password)
        {
            ImpersonateValidUser(userName, domainName, password);
        }

        /// <summary>
        /// Call SetCredentials before using this constructor, error is thrown if username, domain, or password is null.
        /// Please note that the account that instantiates the ImpersonationHelper class
        /// needs to have the 'Act as part of operating system' privilege set.
        /// </summary>
        /// <param name="user">User with username, domain, and password set.</param>
        public ImpersonationHelper()
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(domainName) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("The supplied userName, domainName, or password is not valid, call SetCredentials before using this constructor");

            ImpersonateValidUser(userName, domainName, password);
        }

        public ImpersonationHelper(SqlCredentials credentials)
        {
            ImpersonateValidUser(credentials.userName, credentials.domainName, credentials.password);
        }

        public void Dispose()
        {
            UndoImpersonation();
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int LogonUser(
            string lpszUserName,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int DuplicateToken(
            IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(
            IntPtr handle);

        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_PROVIDER_DEFAULT = 0;

        /// <summary>
        /// Does the actual impersonation.
        /// </summary>
        /// <param name="userName">The name of the user to act as.</param>
        /// <param name="domainName">The domain name of the user to act as.</param>
        /// <param name="password">The password of the user to act as.</param>
        private void ImpersonateValidUser(
            string userName,
            string domain,
            string password)
        {
            WindowsIdentity tempWindowsIdentity = null;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            try
            {
                if (RevertToSelf())
                {
                    if (LogonUser(
                        userName,
                        domain,
                        password,
                        LOGON32_LOGON_INTERACTIVE,
                        LOGON32_PROVIDER_DEFAULT,
                        ref token) != 0)
                    {
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                            impersonationContext = tempWindowsIdentity.Impersonate();
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                if (token != IntPtr.Zero)
                {
                    CloseHandle(token);
                }
                if (tokenDuplicate != IntPtr.Zero)
                {
                    CloseHandle(tokenDuplicate);
                }
            }
        }

        /// <summary>
        /// Reverts the impersonation.
        /// </summary>
        private void UndoImpersonation()
        {
            impersonationContext?.Undo();            
        }

        private WindowsImpersonationContext impersonationContext = null;
    }
}
