/*
 * Created by Ranorex
 * User: anpatel
 * Date: 6/19/2018
 * Time: 1:09 PM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;
using ATS.CodeLibrary.Email;
using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Reporting;
using Ranorex.Core.Testing;
using Ranorex.AutomationHelpers.Modules;
using System.IO;

namespace UpchainMechDeployTest
{
    /// <summary>
    /// Description of ATSEmail.
    /// </summary>
    [TestModule("2709F2A7-7243-4D3C-AAE5-05097F0A4ED6", ModuleType.UserCode, 1)]
    public class ATSEmail : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// 
        
   /*     string _durationofTestCase = "0";
        [TestVariable("5f22aa5d-e6bc-4935-8b9c-d8738ce41d6d")]
        public string durationofTestCase
        {
        	get { return _durationofTestCase; }
        	set { _durationofTestCase = value; }
        }*/
        

        
        public ATSEmail()
        {
        	
        }

        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        /// 
        
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            string reportPath = CreatePdfReport();
            Report.Log(ReportLevel.Info, "Report Path: " + reportPath);
        	SendMail(reportPath);
        	
        }
        
        private string CreatePdfReport()
        {
                ReportToPDFModule pdfModule = new ReportToPDFModule();
                pdfModule.PdfName = "";               
               // pdfModule.PdfName = TestReport.ReportEnvironment.ReportName + ".pdf";
              // pdfModule.Xml = "";
              pdfModule.Xml = @"\\ca01a9001\pgmis\Deployment\Ranorex\style.xml";
                pdfModule.Details = "all";
                return pdfModule.CreatePDF();
   
        }
        
        public void SendMail(string reportPath)
        {
			
        	string sResult = GetTestSuiteStatus();
        	string subject, addressTo, addressFrom, body;
            addressFrom = "ranorexrt@atsautomation.com";
            if (sResult.Contains("Success"))
           	{
           		sResult = "Passed";
           	}

            try 
            {
      
            
            		subject = "ATS Supplier Portal Test config: " + TestSuite.Current.SelectedRunConfig.Name.ToString() + " on " + Environment.MachineName + " " + sResult;
            	

            	
            		addressTo = "anpatel@atsautomation.com; mphelan@atsautomation.com; mmahida@atsautomation.com; blay@atsautomation.com";
            	//	addressTo = "anpatel@atsautomation.com";

            	
            	if (System.IO.File.Exists(reportPath))
           		{
            
           		 	body = subject + ".  See Attached PDF for more info.";

           		 	MailHelper.SendMessage(body, addressTo, subject, "relay2.atsna.atsauto.net", addressFrom, "Supplier Portal Test", new [] { reportPath });
           		 	MovePDF(reportPath);
           	//	 	WriteResults(sResult, durationofTestCase);
            	}
            
            	else
            	{
            		Report.Info("Could not find file: " + reportPath);
            	} 
            }
            catch (Exception ex)
            	{
            		Report.Failure("Mail Error: " + ex.ToString());
            	}
        }
        
        public void MovePDF(string rpath)
        {
        	string PDFfolderpath = @"\\ca01a9001\PGMIS\Deployment_DEV\Ranorex\Supplier_Portal\PDF";
        	if (!(Directory.Exists(PDFfolderpath)))
        		{
        			Directory.CreateDirectory(PDFfolderpath);
        		}
        	if (Directory.Exists(PDFfolderpath))
        	    {
        	    	File.Copy(rpath, PDFfolderpath + "\\" + GetDivision(System.Environment.MachineName) + ".pdf", true);
        	    }
        	    
        	else
        	{
        		Report.Log(ReportLevel.Info, "Failed to find or create PDF directory");
        	}
        	
        }
        
        public static string GetDivision(string machineName)
        {
        	switch (machineName) 
        	{
        		case "US12A2028":
        			return "PAGreenville";
        		case "US03A2028":
        			return "ATSWixom";
        		case "US04A2028":
        			return "ATSOhio";
        		case "US07A2028":
        			return "sortimat";
        		case "CA01A5749":
        			return "Cambridge";
        		default:
        			return machineName;
        	}
        }
        
        public void WriteResults(string TestResultStatus, string TotalTimeElapsed)
        {
        	string testResultsDirectory = @"\\ca01a9001\PGMIS\Deployment_DEV\Ranorex\Supplier_Portal\TestResults\";
        	if (!(Directory.Exists(testResultsDirectory)))
        		{
        			Directory.CreateDirectory(testResultsDirectory);
        		}
        	string text = GetDivision(System.Environment.MachineName) + "\t\t" + TestResultStatus + "\t\t" + TotalTimeElapsed + " seconds" + "\t\t" + System.DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
        	File.WriteAllText(testResultsDirectory + GetDivision(System.Environment.MachineName) + ".txt", text);
        }

        private static string GetTestSuiteStatus()
        {
            string status = "";

            var rootChildren = ActivityStack.Instance.RootActivity.Children;

            if (rootChildren.Count > 1)
            {
                Console.WriteLine("Multiple TestSuiteActivites, status taken from first entry");
            }

            var testSuiteAct = rootChildren[0] as TestSuiteActivity;

            if (testSuiteAct != null)
            {
              //  status = testSuiteAct.Status.ToString();
                if (testSuiteAct.TotalFailedTestCaseCount>0)
                {
                	status = "Failed";
                }
                else
                {
                	status = "Success";
                }
            }

            return status;
        }
    }
}
