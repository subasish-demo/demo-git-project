using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.IO.File;
using ATS.CodeLibrary.Impsersonation;
using System.IO;
using ATS.CodeLibrary.DataUtilities.SQL;

namespace ATS.CodeLibrary.SecureFile
{
    public class SecureFileHelper
    {
        private readonly string neutralLocationPath;               

        /// <summary>
        /// Set credentials for re-use without including them in the methods.
        /// </summary>
        /// <param name="userName">The name of the user to act as.</param>
        /// <param name="domainName">The domain name of the user to act as.</param>
        /// <param name="password">The password of the user to act as.</param>
        /// <param name="neutralLocationPath">
        /// The temporary location that the current user and the impersonated user both have access to.
        /// Required for CopyToSecure and CopyFromSecure
        /// </param>
        public SecureFileHelper(string userName, string domainName, string password, string neutralLocationPath)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(domainName) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("The supplied userName, domainName, or password is not valid");

            ImpersonationHelper.SetCredentials(userName, domainName, password);
            this.neutralLocationPath = neutralLocationPath;
        }

        /// <summary>
        /// Set credentials for re-use without including them in the methods.
        /// </summary>
        /// <param name="user">Credentials for user including username, domain, and password</param>
        /// <param name="neutralLocationPath">
        /// The temporary location that the current user and the impersonated user both have access to.
        /// Required for CopyToSecure and CopyFromSecure
        /// </param>
        public SecureFileHelper(Credentials user, string neutralLocationPath)
        {
            if (string.IsNullOrWhiteSpace(user.userName) || string.IsNullOrWhiteSpace(user.domainName) || string.IsNullOrWhiteSpace(user.password))
                throw new ArgumentNullException("The supplied userName, domainName, or password is not valid");

            ImpersonationHelper.SetCredentials(user.userName, user.domainName, user.password);

            this.neutralLocationPath = neutralLocationPath;
        }

        /// <summary>
        /// Copy a local file to a location to which the current user does not have access from a location that the impersonated user does not have access. Overwrites existing files.
        /// </summary>
        /// <param name="source">Full path of file to copy</param>
        /// <param name="destination">Full path of file location for file</param>
        public void CopyToSecure(string source, string destination)
        {
            if (string.IsNullOrWhiteSpace(neutralLocationPath))
                throw new ArgumentNullException("The supplied neutralLocationPath is not valid");


            string tempPath = Path.Combine(neutralLocationPath, Path.GetFileName(destination));
            Copy(source, tempPath, true);
            using (new ImpersonationHelper())
            {
                if (Exists(destination))
                {
                    SetAttributes(destination, FileAttributes.Normal);
                    Delete(destination);
                }

                Move(tempPath, destination);
            }
        }

        /// <summary>
        /// Copy a file from a location that the current user does not have access to a location that the impersonated user does not have access. Overwrites existing files.
        /// </summary>
        /// <param name="source">Full path of file to copy</param>
        /// <param name="destination">Full path of file location for file</param>
        public void CopyFromSecure(string source, string destination)
        {
            if (string.IsNullOrWhiteSpace(neutralLocationPath))
                throw new ArgumentNullException("The supplied neutralLocationPath is not valid");

            string tempPath = Path.Combine(neutralLocationPath, Path.GetFileName(destination));
            using (new ImpersonationHelper())
            {
                try
                {
                    Copy(source, tempPath, true);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            if (Exists(destination))
            {
                SetAttributes(destination, FileAttributes.Normal);
                Delete(destination);
            }

            Move(tempPath, destination);            
        }

        /// <summary>
        /// Copy a file between locations the current user does not have access
        /// </summary>
        /// <param name="source">Full path of file to copy</param>
        /// <param name="destination">Full path of file location for file</param>
        public void SecureCopy(string source, string destination)
        {
            using (new ImpersonationHelper())
            {
                Copy(source, destination);
            }
        }

        /// <summary>
        /// Delete a file in a location the current user does not have access
        /// </summary>
        /// <param name="path">Full path to file that will be deleted</param>
        public void SecureDelete(string path)
        {
            using (new ImpersonationHelper())
            {
                SetAttributes(path, FileAttributes.Normal);
                Delete(path);
            }
        }

        /// <summary>
        /// Set attributes on a file in a location the current user does not have access 
        /// </summary>
        /// <param name="path">Full path to file that </param>
        /// <param name="attributes"></param>
        public void SecureSetAttributes(string path, FileAttributes attributes)
        {
            using (new ImpersonationHelper())
            {
                SetAttributes(path, attributes);
            }
        }
    }
}