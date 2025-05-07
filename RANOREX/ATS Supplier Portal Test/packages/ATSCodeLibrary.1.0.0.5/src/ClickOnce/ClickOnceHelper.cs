using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.ClickOnce
{
    /// <summary>
    /// Provides methods to help make working with ClickOnce applications easier
    /// </summary>
    public class ClickOnceHelper
    {
        /// <summary>
        /// Get args from the appropriate source depending on whether the application is deployed or not
        /// </summary>
        /// <returns>Parameters that were passed into application, does not include execution path</returns>
        public static string[] GetArgs(string delimiter = null)
        {       
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                if (delimiter == null)
                {
                    return AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
                }
                else
                {
                    if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null)
                    {
                        return AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0].Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return Environment.GetCommandLineArgs().Skip(1).ToArray();
            }
        }

        /// <summary>
        /// Install update if available
        /// </summary>
        /// <param name="allowOffline">If false and update is unavailable then return false</param>
        /// <param name="errorMessage">descriptive error message or null if there were no errors</param>
        /// <param name="restartRequired">If true, then call Application.Restart()</param>
        /// <returns>True if safe to continue</returns>
        public static void InstallUpdateIfAvailable(bool allowOffline, out string errorMessage, out bool restartRequired)
        {
            UpdateCheckInfo info = null;
            errorMessage = null;
            restartRequired = false;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    if (!allowOffline)
                        throw new Exception("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                }
                catch (InvalidDeploymentException ide)
                {
                    throw new Exception("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);                    
                }
                catch (InvalidOperationException ioe)
                {
                    throw new Exception("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);                    
                }

                if (info.UpdateAvailable)
                {
                    try
                    {
                        ad.Update();
                        restartRequired = true;
                    }
                    catch (DeploymentDownloadException dde)
                    {
                        throw new Exception("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                    }
                }
            }
        }
    }
}
