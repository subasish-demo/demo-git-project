/*
 * Created by Ranorex
 * User: anpatel
 * Date: 10/15/2019
 * Time: 1:09 PM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using WinForms = System.Windows.Forms;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.Util;
using ATS.CodeLibrary;
using ATS.CodeLibrary.DataUtilities.Cryptography;
using ATS.CodeLibrary.DataUtilities.SQL;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace ATS_Supplier_Portal_Test
{
	/// <summary>
	/// Creates a Ranorex user code collection. A collection is used to publish user code methods to the user code library.
	/// </summary>
	/// 
	
	[UserCodeCollection]
	public class SPCollection
	{
		// You can use the "Insert New User Code Method" functionality from the context menu,
		// to add a new method with the attribute [UserCodeMethod].
//		public static ATS_Supplier_Portal_TestRepository repo = ATS_Supplier_Portal_TestRepository.Instance;
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		/// 
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		/// 
		public static SqlCredentials credsJDEPROD = new SqlCredentials("CA15A2400", "JDE_PRODUCTION", "integration", CryptographyHelper.DecryptString("XrqqBbXPhNAVo7l1T2YU1Q=="));
		//public static SqlCredentials credsSPPROD = new SqlCredentials("CA01A2305", "SupplierPortal", "SupplierPortal", CryptographyHelper.DecryptString("o6uhcbGQM75DOpMW0rz3cQ=="));
		public static SqlCredentials credsSPPROD = new SqlCredentials("Server=tcp:atsautomationsupplierportal.database.windows.net,1433;Initial Catalog=atssupplierportalp;Persist Security Info=False;User ID=hqadmin;Password=" + CryptographyHelper.DecryptString(@"/f3DqIPmhYWWsLTd4yTUIQ==") + ";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
		//public static SqlCredentials credsSPDev = credsSPDev;
		public static SqlCredentials credsSPDev = new SqlCredentials("Server=tcp:atsautomationsupplierportal.database.windows.net,1433;Initial Catalog=atsautomationsupplierportal;Persist Security Info=False;User ID=hqadmin;Password=" + CryptographyHelper.DecryptString(@"/f3DqIPmhYWWsLTd4yTUIQ==") + ";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
		//public static SqlCredentials credsSPPROD = credsSPDev;
		
		static string subscriptionKey = "09fc9f2d062244afa8dbdfb12b617ff4";
        static string endpoint = "https://computer-vision-ocr-it.cognitiveservices.azure.com/";
        private const string READ_TEXT_LOCAL_IMAGE = @"\\ca01a9001\pgmis\Deployment_DEV\Ranorex\Supplier_Portal\OCR\capture.png";
    	
    	/// <summary>
    	/// This is a placeholder text. Please describe the purpose of the
    	/// user code method here. The method is published to the user code library
    	/// within a user code collection.
    	/// </summary>
    	[UserCodeMethod]
    	public static void GetSS()
    	{
    		
    		string filename = READ_TEXT_LOCAL_IMAGE;
    		Report.Info("Taking Screenshot");
    		Bitmap bmp = Imaging.CaptureDesktopImage(null);
    		CompressedImage img = new CompressedImage(bmp);
    		img.Store(filename);
    		Report.Info("Success, screenshot image saved to: " + filename);
    		
    	}
    	
    	
    	/// <summary>
    	/// This is a placeholder text. Please describe the purpose of the
    	/// user code method here. The method is published to the user code library
    	/// within a user code collection.
    	/// </summary>
    	[UserCodeMethod]
    	public static bool RunOCRCogServ(string sWord)
    	{
    		
    		ComputerVisionClient client = ATS_Supplier_Portal_Test.CogServ.Authenticate(endpoint, subscriptionKey);
			return CogServ.ReadFileLocal(client, READ_TEXT_LOCAL_IMAGE, sWord).Result;
			
    	}
		[UserCodeMethod]
		public static string DecryptPassword(string encryptedPassword)
		{
			return CryptographyHelper.DecryptString(encryptedPassword);
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void CloseWithAltF4(bool val)
		{	
			if (val)
        	{
        		Ranorex.Report.Info("Attempting to close window with Alt-F4");
        		Keyboard.Press("{LMenu down}{F4}{LMenu up}");
        	}    
		}
		
		[UserCodeMethod]
		public static void ActiveMQRequest(string destinationName, string jsonPath, string environment, string companyNumber)
		{
			
			Uri connecturi = new Uri("activemq:tcp://ca01a2709.atsna.atsauto.net:61616");  //DEV/Test server//.109 for DEV
			if(environment == "PROD")
			{
				connecturi = new Uri("activemq:tcp://ca01a2710.atsna.atsauto.net:61616"); //PROD Server//.110 or .111
				Report.Info("Connecting to Active MQ PROD Server: " + connecturi.ToString());
			}
			else
			{
				Report.Info("Connecting to Active MQ DEV Server: " + connecturi.ToString());
			}
			IConnectionFactory factory = new NMSConnectionFactory(connecturi);
			string json = File.ReadAllText(jsonPath);
			if (json == null)
				return;

			using (IConnection connection = factory.CreateConnection())
				using (ISession session = connection.CreateSession())
			{
				IDestination destination = SessionUtil.GetDestination(session, destinationName);

				// Create producer
				using (IMessageProducer producer = session.CreateProducer(destination))
				{
					// Start the connection so that messages will be processed.
					connection.Start();
					//producer.Persistent = true;

					// Send a message
					IMapMessage request = session.CreateMapMessage();
					request.Body.SetString("source", json);

					JObject dynObj = JObject.Parse(json);
					if (dynObj == null)
						return;

					string headerID = "NoHeaderID";
					if (dynObj != null)
					{
						headerID = dynObj["header_id"].ToString();
					}

					request.NMSCorrelationID = headerID;
					request.Properties["NMSXGroupID"] = "ERPInboundRequisition";
					request.Properties["myHeader"] = "Integration";
					request.Properties["CorrelationID"] = headerID;
					request.Properties["ATSCompanyNumber"] = companyNumber;
					

					producer.Send(request);
					Report.Log(ReportLevel.Info, "Active MQ Request Sent");
				}
			}
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static bool CheckForJDELedgerTableErrorActiveMQ(string order, string lineNumber)
		{
			var uri = "tcp://CA01a2709.atsna.atsauto.net:61616";
            var queue = "ATSActiveMQUpdateF43199Error";

 

            try
            {
                IConnectionFactory factory = new NMSConnectionFactory(uri);

 

                using (IConnection connection = factory.CreateConnection())
                using (ISession session = connection.CreateSession())
                {
                    IQueue destination = session.GetQueue(queue);
                    using (IQueueBrowser browser = session.CreateBrowser(destination))
                    {
                        connection.Start();
                       // var order = "youorder";
                       // var line = 1;
                       	var lineNumberConverted = Convert.ToDecimal(lineNumber) * 1000;
        				var converted = Convert.ToInt32(lineNumberConverted);
        				var line = converted.ToString();
                        foreach (ActiveMQTextMessage msg in browser)
                        {
                            if (msg.Text.Contains("\"PDDOCO\":" + order) && msg.Text.Contains("\"PDLNID\":" + line))
                            {
                            	Report.Info("Found order number: " + order + " and line number: " + line + " in Active MQ Error Queue!");
                            	return true;
                            }
                        }
                        Report.Warn("Did Not Find order number: " + order + " and line number: " + line + " in Active MQ Error Queue!");
                        return false;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("## Browser Thread Aborted");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                throw;
            }
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void EditJSONFile(string jsonPath, string textToReplace, string ReplaceWith, string newPath)
		{
		//	string newJSONPath = @"\\ca01a9001\pgmis\Deployment_DEV\Ranorex\Supplier_Portal\JSON\jsonSP.txt";
		//	string reqNumber = ReadReqFile(@"\\ca01a9001\pgmis\Deployment_DEV\Ranorex\Supplier_Portal\RequisitionNumber\req.txt");
			string text = File.ReadAllText(jsonPath);
			text = text.Replace(textToReplace, ReplaceWith);
			File.WriteAllText(newPath, text);
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string ReadReqFile(string reqFilePath)
		{
			int x = 0;
			string line = File.ReadAllText(reqFilePath);
			x = Convert.ToInt32(line);
			x++;
			File.WriteAllText(reqFilePath, x.ToString("D7"));
			Report.Log(ReportLevel.Info, "Req number: " + x.ToString("D7"));
			return x.ToString("D7");
			
			
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string ReadRequisitionFile(string requisitionFilePath)
		{
			return File.ReadAllText(requisitionFilePath);
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string GetUsername()
		{
			return Environment.UserName.Replace("atsna\\", "").ToLower();
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ProcessKill(string processname)		
        {
        //    Process[] processes = Process.GetProcessesByName(processname);
       	 Process[] processes = Process.GetProcesses();
         if (processes.Length == 0)
            {
                Report.Warn(string.Format("Process '{0}' not found.", processname));
            }
       	 foreach (var proc in processes) 
        	{
        		if (proc.ProcessName.Contains(processname))
        		{
        			try
                	{
                 	   proc.Kill();
                 	   Report.Info("Process killed: " + proc.ProcessName);
                	}
                	catch (Exception ex)
                	{
                    	Report.Warn(ex.Message);
                	}
        		}
        		
        	}
           
        }
			
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void FilterValidate(string columnName, string textToCompare, bool compareComboBox)
		{
			int rowCount = 0;
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			textToCompare = textToCompare.ToUpper();
			int colIndex = GetColumnIndexofHeader(columnName);
			if (colIndex == -1)
			{
				Report.Failure("Unable to Find column name: " + columnName);
			}
			var UnprocessedTableContainer = repo.MainWindow.NavigationPanel.UnprocessedTableContainer;
			var rTable = UnprocessedTableContainer.FindChildren<Ranorex.Row>();
			foreach (var rRow in rTable) 
			{
				var cellList = rRow.FindDescendants<Ranorex.Cell>();
				
				foreach (var cell in cellList) 
				{
					if (cell.ColumnIndex == colIndex)
					{
						if (compareComboBox)
						{
							if (rTable.Count >0)
							{
									var cBox = cell.FindDescendant<Ranorex.Text>();
							//		Report.Log(ReportLevel.Info, cBox.TextValue);
							//		Report.Log(ReportLevel.Info, textToCompare);
									if (cBox.TextValue.ToUpper() == textToCompare)
									{
										Ranorex.Validate.IsTrue(cBox.TextValue.ToUpper() == textToCompare, "The validation result @ValidateRESULT@");
										Report.Screenshot(cell, true);
									}
							}
						}
						
						else
						{					
							
						 if(cell.Text.Contains(textToCompare))
							{
								Ranorex.Validate.IsTrue(cell.Text.Contains(textToCompare), "The validation result @ValidateRESULT@");
								Report.Screenshot(cell, true);
							}
						}
						
					}
					
				}
			}
			if (rTable != null) 
			{
				rowCount = rTable.Count;				
			}
			
			
			Report.Log(ReportLevel.Info, "Row count of UnprocessedDetailsGrid Table is: " + rowCount);
			
		
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void FilterValidateDateTimeText(string columnName, string textToCompare)
		{
			int rowCount = 0;
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			textToCompare = textToCompare.ToUpper();
			int colIndex = GetColumnIndexofHeader(columnName);
			if (colIndex == -1)
			{
				Report.Failure("Unable to Find column name: " + columnName);
			}
			var UnprocessedTableContainer = repo.MainWindow.NavigationPanel.UnprocessedTableContainer;
			var rTable = UnprocessedTableContainer.FindChildren<Ranorex.Row>();
			foreach (var rRow in rTable) 
			{
				var cellList = rRow.FindDescendants<Ranorex.Cell>();
				
				foreach (var cell in cellList) 
				{
					if (cell.ColumnIndex == colIndex)
					{
						
						Ranorex.Text txt = cell.FindDescendant<Ranorex.Text>();
					//	bool isTxt = cell.TryFindSingle(cell.GetPath() + @"/datetime/text[@automationid='PART_TextBox']", out txt);
						
							Report.Info(cell.GetPath() + @"/datetime/text[@automationid='PART_TextBox']");
						   	Ranorex.Validate.IsTrue(txt.SelectionText.Contains(textToCompare), "The validation result @ValidateRESULT@");
							Report.Screenshot(txt, true);
					
					}
					
				}
			}
			if (rTable != null) 
			{
				rowCount = rTable.Count;				
			}
			
			
			Report.Log(ReportLevel.Info, "Row count of UnprocessedDetailsGrid Table is: " + rowCount);
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static int GetColumnIndexofHeader(string colName)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			var cellList = repo.MainWindow.NavigationPanel.TableHeaderContainer.FindChildren<Ranorex.Cell>();
			foreach (var cell in cellList) 
			{
			//	Report.Log(ReportLevel.Info, "Cell Text: " + cell.Text);
				if (cell.Text == colName)
				{
					return cell.ColumnIndex;
				}
				
			}
			return -1;
			
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void FindFileExtentionFromTableRxPath(string colName, string rxPathofTable, string fileExtension, string rowIndex)
		{
			//Ranorex.Table table = "/form[@title='MainWindow']/list[@automationid='navigationPanel']//table[@automationid='datagrid']";
			int colIndex = -1;
			Report.Info("Getting index of column: " + colName + "...");
			colIndex = GetColumnIndexofTable(rxPathofTable, colName, false);
			if (colIndex == -1)
			{
				Report.Failure("Unable to Find column name: " + colName);
			}
		
			Ranorex.Table rTable = @rxPathofTable;
			var rRows = rTable.FindDescendants<Ranorex.Row>();
			Report.Info("Searching for Row with RowIndex: " + rowIndex + " in table...");
			var rRow = rRows.FirstOrDefault(x => x.Index.ToString() == rowIndex);			
			if(rRow != null)
			{
				
				Report.Info("Found Row Index: " + rRow.Index.ToString() + "!");
				var cellList = rRow.FindDescendants<Ranorex.Cell>();
				Report.Info("Searching for cell in Table with Row Index: " + rowIndex + " and Column Index: " + colIndex);
				foreach (var cell in cellList) 
				{
					if ((cell.ColumnIndex == colIndex) && (cell.RowIndex.ToString() == rowIndex))
					{
						Report.Info("Cell Found! Looking for file button with File Extension: " + fileExtension);
						var rButton = cell.FindDescendants<Ranorex.Button>();
						foreach (var button in rButton) 
						{
							if (button.Text.ToUpper().Contains(fileExtension))
							{
								button.MoveTo();
								Delay.Seconds(2.0);
								Report.Screenshot(button,true);
								button.Click();
								CloseApplicationWithFileExtension(fileExtension);
								return;
							}
						}
					}
				}
				
		//	Report.Log(ReportLevel.Warn, fileExtension + " file not found");
			}
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void CloseApplicationWithFileExtension(string FileExtention)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			Report.Info("Started Closing Application Module for file with file extension: " + FileExtention);
			if(FileExtention == ".PDF")
			{
				Report.Log(ReportLevel.Info, "Wait", "Waiting 1m to exist. Associated repository item: 'AdobeReader'", repo.AdobeReader.SelfInfo, new ActionTimeout(60000), new RecordItemIndex(12));
	            repo.AdobeReader.SelfInfo.WaitForExists(60000);
	            
	            Report.Log(ReportLevel.Info, "Validation", "Validating Exists on item 'AdobeReader'.", repo.AdobeReader.SelfInfo, new RecordItemIndex(13));
	            Validate.Exists(repo.AdobeReader.SelfInfo);
	            Delay.Milliseconds(0);
	            
	            Report.Log(ReportLevel.Info, "Delay", "Waiting for 2s.", new RecordItemIndex(14));
	            Delay.Duration(2000, false);
	            
	            Report.Screenshot(ReportLevel.Info, "User", "", repo.AdobeReader.Self, false, new RecordItemIndex(15));
	            
	            Report.Log(ReportLevel.Info, "Delay", "Waiting for 2s.", new RecordItemIndex(16));
	            Delay.Duration(2000, false);
	            
	            Report.Log(ReportLevel.Info, "Application", "Closing application containing item 'AdobeReader'.", repo.AdobeReader.SelfInfo, new RecordItemIndex(17));
	            Host.Current.CloseApplication(repo.AdobeReader.Self, new Duration(0));
	            Delay.Milliseconds(0);
			}
			else if(FileExtention == ".SLDPRT")
			{
				Report.Log(ReportLevel.Info, "Wait", "Waiting 1m to exist. Associated repository item: 'SOLIDWORKS.statusBarText'", repo.SOLIDWORKS.statusBarTextInfo, new ActionTimeout(60000), new RecordItemIndex(5));
	            repo.SOLIDWORKS.statusBarTextInfo.WaitForExists(60000);
	            
	            Report.Log(ReportLevel.Info, "Validation", "Validating Exists on item 'SOLIDWORKS.statusBarText'.", repo.SOLIDWORKS.statusBarTextInfo, new RecordItemIndex(6));
	            Validate.Exists(repo.SOLIDWORKS.statusBarTextInfo);
	            Delay.Milliseconds(0);
	            
	            Report.Log(ReportLevel.Info, "Delay", "Waiting for 2s.", new RecordItemIndex(7));
	            Delay.Duration(2000, false);
	            
	            Report.Screenshot(ReportLevel.Info, "User", "", repo.SOLIDWORKS.Self, false, new RecordItemIndex(8));
	            
	            Report.Log(ReportLevel.Info, "Delay", "Waiting for 10s.", new RecordItemIndex(9));
	            Delay.Duration(10000, false);
	            
	            Report.Log(ReportLevel.Info, "Application", "Closing application containing item 'SOLIDWORKS'.", repo.SOLIDWORKS.SelfInfo, new RecordItemIndex(10));
	            Host.Current.CloseApplication(repo.SOLIDWORKS.Self, new Duration(0));
	            Delay.Milliseconds(0);
			}
			else if(FileExtention == ".DWG")
			{
				Report.Log(ReportLevel.Info, "Wait", "Waiting 1m to exist. Associated repository item: 'AutoCAD2018'", repo.AutoCAD2018.SelfInfo, new ActionTimeout(60000), new RecordItemIndex(19));
	            repo.AutoCAD2018.SelfInfo.WaitForExists(60000);
	            
	            Report.Log(ReportLevel.Info, "Validation", "Validating Exists on item 'AutoCAD2018'.", repo.AutoCAD2018.SelfInfo, new RecordItemIndex(20));
	            Validate.Exists(repo.AutoCAD2018.SelfInfo);
	            Delay.Milliseconds(0);
	            
	            Report.Log(ReportLevel.Info, "Delay", "Waiting for 15s.", new RecordItemIndex(21));
	            Delay.Duration(15000, false);
	            
	            Report.Screenshot(ReportLevel.Info, "User", "", repo.AutoCAD2018.Self, false, new RecordItemIndex(22));
	            
	            Report.Log(ReportLevel.Info, "Delay", "Waiting for 2s.", new RecordItemIndex(23));
	            Delay.Duration(2000, false);
	            
	            Report.Log(ReportLevel.Info, "Application", "Closing application containing item 'AutoCAD2018'.", repo.AutoCAD2018.SelfInfo, new RecordItemIndex(24));
	            Host.Current.CloseApplication(repo.AutoCAD2018.Self, new Duration(0));
	            Delay.Milliseconds(0);
			}
			else
			{
				Report.Info("No files open with file extensions .PDF, .SLDPRT, or .DWG");
			}
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string GetDateTimePlusXDays(int days)
		{
			return System.DateTime.Today.AddDays(days).ToString("MM/dd/yyyy");
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static int GetRowCountofTable()
		{
			int rowCount = -1;
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			var UnprocessedTableContainer = repo.MainWindow.NavigationPanel.UnprocessedTableContainer;
			var rTable = UnprocessedTableContainer.FindChildren<Ranorex.Row>();
			if (rTable != null) 
			{
				rowCount = rTable.Count;
				return rowCount;				
			}
			return rowCount;
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SelectOddRows(int rCount)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			var UnprocessedTableContainer = repo.MainWindow.NavigationPanel.UnprocessedTableContainer;
			var rRowList = UnprocessedTableContainer.FindChildren<Ranorex.Row>();
			

				for (int i = 0; i<rCount - 1; i = i + 2)
				{
					var cellList = rRowList[i].FindDescendants<Ranorex.Cell>();
					if (i == 0)
					{
						cellList[2].Click();
					}
					else
					{
						Keyboard.Down(System.Windows.Forms.Keys.LControlKey, Keyboard.DefaultScanCode, true);
						cellList[2].Click();
						Keyboard.Up(System.Windows.Forms.Keys.LControlKey, Keyboard.DefaultScanCode, true);
					}
					
				}
			
			
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SelectHalfofCheckboxes()
		{
			var rowCount = GetRowCountofTable();
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			var UnprocessedTableContainer = repo.MainWindow.NavigationPanel.UnprocessedTableContainer;
			var rRowList = UnprocessedTableContainer.FindChildren<Ranorex.Row>();
			int numRows = 0;
			if (rowCount%2>0)
			{
				 numRows = (rowCount + 1)/2;
			}
			
			else if (rowCount%2 == 0)
			{
				 numRows = (rowCount/2);
			}
			
			for (int i = 0; i < (numRows + 1); i++)
			{
				var cBox = rRowList[i].FindSingle("list/?/?/checkbox").As<Ranorex.CheckBox>();
				cBox.Check();
			}
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ValidateRequiredDateOddRows(int rCount, int colIndex, string Date)
		{
		//	var convertedDate = System.DateTime.ParseExact(Date, "mm/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("mm/dd/yyyy");
			var convertedDate = Date;
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			var UnprocessedTableContainer = repo.MainWindow.NavigationPanel.UnprocessedTableContainer;
			var rRowList = UnprocessedTableContainer.FindChildren<Ranorex.Row>();
			

				for (int i = 0; i<rCount - 1; i = i + 2)
				{
					var cellList = rRowList[i].FindDescendants<Ranorex.Cell>();
					var Text = cellList[colIndex].FindDescendant<Ranorex.Text>();
					Report.Info(Text.TextValue.ToString() + " Converted Date: " + convertedDate + " Original Date: " + Date);
					Ranorex.Validate.IsTrue(Text.TextValue == convertedDate,"Checking if date matches the bulk date, will fail if not True.",true);
					Report.Screenshot(Text);
				}
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ValidateAssignees()
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			var UnprocessedTableContainer = repo.MainWindow.NavigationPanel.UnprocessedTableContainer;
			var rRowList = UnprocessedTableContainer.FindChildren<Ranorex.Row>();
			repo.MainWindow.NavigationPanel.AssignedToDropDown.Click();
			var listItemsAssignee = repo.ATSSupplierPortalManagment.Self.FindDescendants<Ranorex.ListItem>();
			repo.MainWindow.NavigationPanel.AssignedToDropDown.Click();
			foreach (var listItem in listItemsAssignee) 
			{
				repo.MainWindow.NavigationPanel.AssignedToDropDown.Click();
				listItem.Click();
				Report.Screenshot(repo.MainWindow.NavigationPanel.AssignedToDropDown, true);
				if (listItem.Text.ToString() == "Any" || listItem.Text.ToString() == "")
				{
					Report.Log(ReportLevel.Info, "Filter is Any or Empty, not validating");
					Report.Screenshot(repo.MainWindow.NavigationPanel.tabPageList);
				}
				else
				{
					FilterValidate("Assignee", listItem.Text.ToString(), true);
				}
				
			}
			
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ValidateMultiSelectOddRows(string columnName, string textToCompare)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			textToCompare = textToCompare.ToUpper();
			int colIndex = GetColumnIndexofHeader(columnName);
			if (colIndex == -1)
			{
				Report.Failure("Unable to Find column name: " + columnName);
			}
			var UnprocessedTableContainer = repo.MainWindow.NavigationPanel.UnprocessedTableContainer;
			var rTable = UnprocessedTableContainer.FindChildren<Ranorex.Row>();
			for (int i = 0; i < rTable.Count - 1; i = i + 2)
			{
				var cellList = rTable[i].FindDescendants<Ranorex.Cell>();
				
				foreach (var cell in cellList) 
				{
					if (cell.ColumnIndex == colIndex)
					{
							if (rTable.Count > 0)
							{
									var cBox = cell.FindDescendant<Ranorex.Text>();
							//		Report.Log(ReportLevel.Info, cBox.TextValue);
							//		Report.Log(ReportLevel.Info, textToCompare);
									if (cBox.TextValue.ToUpper() == textToCompare)
									{
										Ranorex.Validate.IsTrue(cBox.TextValue.ToUpper() == textToCompare, "The validation result @ValidateRESULT@");
										Report.Screenshot(cell, true);
									}
							}
						
						
						
					}
					
				}
			}
			
			
			
			
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void FindFileExtension(string columnName, string fileExtension)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			int colIndex = GetColumnIndexofHeader(columnName);
			if (colIndex == -1)
			{
				Report.Failure("Unable to Find column name: " + columnName);
			}
			var UnprocessedTableContainer = repo.MainWindow.NavigationPanel.UnprocessedTableContainer;
			var rTable = UnprocessedTableContainer.FindChildren<Ranorex.Row>();
			foreach (var rRow in rTable) 
			{
				var cellList = rRow.FindDescendants<Ranorex.Cell>();
				
				foreach (var cell in cellList) 
				{
					if (cell.ColumnIndex == colIndex)
					{
						var rButton = cell.FindDescendants<Ranorex.Button>();
						foreach (var button in rButton) 
						{
							if (button.Text.ToUpper().Contains(fileExtension))
							{
								button.MoveTo();
								Delay.Seconds(2.0);
								Report.Screenshot(button,true);
								button.Click();
								return;
							}
						}
					}
				}
			Report.Log(ReportLevel.Warn, fileExtension + " file not found");
			}
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ValidateSorting(string columnName)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			int colIndex = GetColumnIndexofHeader(columnName);
			if (colIndex == -1)
			{
				Report.Failure("Unable to Find column name: " + columnName);
			}
			var UnprocessedTableContainer = repo.MainWindow.NavigationPanel.UnprocessedTableContainer;
			var rTable = UnprocessedTableContainer.FindDescendants<Ranorex.Row>();
			string previousValue = "";
			foreach (var rRow in rTable) 
			{
				var cellList = rRow.FindDescendants<Ranorex.Cell>();
				
				foreach (var cell in cellList) 
				{
					if (cell.ColumnIndex == colIndex)
					{
						if (previousValue == "")
						{
							previousValue = cell.Text;
						}
						
						else
						{
							var result = String.Compare(previousValue, cell.Text);
							Report.Info("Previous Value: " + previousValue);
							Report.Info("Current Cell Value: " + cell.Text);
							Ranorex.Validate.IsTrue(result<=0, "Checking status of sort compare: @ValidateRESULT@", true);
							previousValue = cell.Text;
						}
					}
				}
			
			}
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ValidateCellContainsText(string textToCompare, string columnName)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			var rTable = repo.MainWindow.NavigationPanel.VendorsTable;
			var rCells = rTable.FindDescendants<Ranorex.Cell>();
			
			foreach (var rCell in rCells) 
			{
				if (rCell.Text != null)
				{
					if(rCell.Text.ToUpper() == textToCompare.ToUpper())
					{
						Ranorex.Validate.IsTrue((rCell.Text.ToUpper() == textToCompare.ToUpper()), "The validation @ValidateRESULT@.  The cell contains the text: " + textToCompare.ToUpper());
						Report.Screenshot(rCell,true);
						return;
					}
				}
				
			}
			
			Ranorex.Validate.Fail("Did not find a cell with matching text: " + textToCompare.ToUpper());
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void WriteReqNumber(string ReqNum, string pathtoReqFile)
		{
			//string pathtoReqFile = @"\\ca01a9001\pgmis\Deployment_DEV\Ranorex\Supplier_Portal\RFQNumber\RFQNumber.txt";
			File.WriteAllText(pathtoReqFile, ReqNum);
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		/// 
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SQLUPDATEGenericDev(string SQLStatement, string comment)
		{
			Report.Info(comment);
			SqlHelper.ExecuteNonQuery(credsSPDev, SQLStatement);
			Report.Info("Success!");
		}
		[UserCodeMethod]
		public static string SQLQuery(string manfname)
		{
			var SQL = "SELECT [VendorID] FROM [dbo].[tblVendor] (NOLOCK) WHERE Name = @manufacturer";
			var dic = new Dictionary<string, object>{{"@manufacturer", manfname}};
			//var creds = new SqlCredentials("10.128.33.7", "SupplierPortal", "SupplierPortal", CryptographyHelper.DecryptString("o6uhcbGQM75DOpMW0rz3cQ=="));
			var results = SqlHelper.ExecuteQuery(credsSPDev, SQL, dic).Tables[0];
			var id = results.Rows[0][0].ToString();
			Report.Info("id = " + id);
			return id;
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SQLQueryColumnInfo(string vendorid, string tableName)
		{
		//	var SQL = "DELETE FROM tblVendorErpLink WHERE VendorID = @vendorid";
			var SQL = "SELECT * FROM " + tableName + " (NOLOCK) WHERE VendorID = @vendorid";
			var dic = new Dictionary<string, object>{{"@vendorid", vendorid}};
			//var creds = new SqlCredentials("10.128.33.7", "SupplierPortal", "SupplierPortal", CryptographyHelper.DecryptString("o6uhcbGQM75DOpMW0rz3cQ=="));
		//	SqlHelper.ExecuteNonQuery(creds, SQL, dic);
			var results = SqlHelper.ExecuteQuery(credsSPDev, SQL, dic).Tables;
			foreach (System.Data.DataTable table in results) 
			{
				Report.Info("DataTable Name: " + table.TableName.ToString());
				for (int i = 0; i<table.Rows.Count; i++)
				{
					for(int j=0; j<table.Columns.Count; j++)
					{
						Report.Info("DataColumn Name: " + table.Columns[j].ColumnName);
						Report.Info("Result for row " + i.ToString() + " column " + j.ToString() + " is: " + table.Rows[i][j].ToString());
					}
				}
			}
		
			
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SQLDeleteFromSpecificTable(string vendorid, string tableName)
		{
		//	var SQL = "DELETE FROM [dbo].[tblVendor] WHERE VendorID = @vendorid";
		//	Use tableName = for deleting both [dbo].[tblVendorUser] and [dbo].[tblVendor]
			var SQL = "DELETE FROM " + tableName + " WHERE VendorID = @vendorid";
			var dic = new Dictionary<string, object>{{"@vendorid", vendorid}};
			//var creds = credsSPDev;
			SqlHelper.ExecuteQuery(credsSPDev, SQL, dic);
			Report.Info("Rows with vendorid: " + vendorid + " deleted from " + tableName);
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SQLDeleteShippingMethod(string method)
		{
			var SQL = "DELETE FROM [dbo].[tblShipping] WHERE Method = @method";
			var dic = new Dictionary<string, object>{{"@method", method}};
		//	var creds = credsSPDev;
			SqlHelper.ExecuteNonQuery(credsSPDev, SQL, dic);
			Report.Info("Rows with Shipping Method: " + method + " deleted from Supplier Portal");	
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static int SQLCheckSPForReqNumber(string reqnum, string environmentServerName)
		{
			var SQL = "SELECT COUNT(*) FROM [dbo].[tblDetail] WHERE UserReference = @reqnum";
			var dic = new Dictionary<string, object>{{"@reqnum", reqnum}};
			System.Data.DataTable results;
		//	var creds = new SqlCredentials(environmentServerName, "SupplierPortal", "SupplierPortal", CryptographyHelper.DecryptString("o6uhcbGQM75DOpMW0rz3cQ=="));
			
			if (environmentServerName == "CA01A2305")
			{
				 results = SqlHelper.ExecuteQuery(credsSPPROD, SQL, dic).Tables[0];
			}
			else
			{
				 results = SqlHelper.ExecuteQuery(credsSPDev, SQL, dic).Tables[0];
			}
		
			int count = Convert.ToInt32(results.Rows[0][0].ToString());
			Report.Info("Searching for User Reference number: " + reqnum + " in Supplier Portal...Found " + count.ToString() + " instances");
			return count;
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		/// 
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string SQLJDEPRODReturnOrderNumber(string rfqnumber)
		{
			var SQLQuery = "SELECT DISTINCT PDDOCO FROM PRODDTA.F4311  (NOLOCK) WHERE PDURAB = @rfqnumber";
			var dic = new Dictionary<string, object>{{"@rfqnumber", rfqnumber}};
			var results = SqlHelper.ExecuteQuery(credsJDEPROD, SQLQuery, dic).Tables[0];
			if (results.Rows.Count == 0)
			{
				Ranorex.Validate.Fail("ERROR: No ordernumber found in JDE PROD for RFQ Number: " + rfqnumber);
				return null;
			}
			else
			{
				return results.Rows[0][0].ToString();
			}
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SQLCleanupPRODJDE(string SQL, string logMessage)
		{
			Report.Info("Executing SQL in JDE PROD to perform the following: " + logMessage);
			SqlHelper.ExecuteNonQuery(credsJDEPROD, SQL);
			Report.Info("Success!");
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SQLCleanupSPPROD(string SQL, string logMessage)
		{
			Report.Info("Executing SQL in SupplierPortal PROD to perform the following: " + logMessage);
			SqlHelper.ExecuteNonQuery(credsSPPROD, SQL);
			Report.Info("Success!");
		}
		
		
		[UserCodeMethod]
		public static string SQLQueryJDEReturnOrderNumber(string rfqnumber)
		{
			var SQLQuery = "SELECT DISTINCT PDDOCO FROM TESTDTA.F4311  (NOLOCK) WHERE PDURAB = @rfqnumber";
			var dic = new Dictionary<string, object>{{"@rfqnumber", rfqnumber}};
			var creds = new SqlCredentials("CA15A2450", "JDE_DEVELOPMENT", "integration", CryptographyHelper.DecryptString("XrqqBbXPhNAVo7l1T2YU1Q=="));
			var results = SqlHelper.ExecuteQuery(creds, SQLQuery, dic).Tables[0];
			return results.Rows[0][0].ToString();
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static System.Data.DataTable SQLQueryJDEF4311(string buyerNumber, string detailNo, string userRefNo, string Qty)
		{
			var SQLQuery = "SELECT PDDOCO, PDLNID FROM TESTDTA.F4311  (NOLOCK) WHERE PDANBY = @buyerNo AND PDVR01 LIKE @detailNo AND PDURRF LIKE @userRefNo AND PDUOPN = @Qty";
			var dic = new Dictionary<string, object>{{"@buyerNo", buyerNumber}, {"@detailNo", detailNo + "%"}, {"@userRefNo", userRefNo + "%"}, {"@Qty", Qty}};
			var creds = new SqlCredentials("CA15A2450", "JDE_DEVELOPMENT", "integration", CryptographyHelper.DecryptString("XrqqBbXPhNAVo7l1T2YU1Q=="));
			var results = SqlHelper.ExecuteQuery(creds, SQLQuery, dic).Tables[0];
			return results;
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static System.Data.DataTable SQLQueryJDEDevLedgerTable(string orderno, string linenumber, string username)
		{
			var SQLQuery = "SELECT OLUSER, dbo.fn_ConvertJDE_Date(OLUPMJ) AS OLUPMJ, OLANBY, OLNXTR, OLLTTR FROM TESTDTA.F43199  (NOLOCK) WHERE OLDOCO = @orderno AND OLLNID = @linenumber AND OLUSER LIKE @username";
			var dic = new Dictionary<string, object> { { "@orderno", orderno }, { "@linenumber", linenumber }, { "@username", username + "%" }};
            var creds = new SqlCredentials("CA15A2450", "JDE_DEVELOPMENT", "integration", CryptographyHelper.DecryptString("XrqqBbXPhNAVo7l1T2YU1Q=="));
            var results = SqlHelper.ExecuteQuery(creds, SQLQuery, dic).Tables[0];
            return results;
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string SQLQueryJDELastStatus(string userRefNo, string supplierSO, string qty, bool isPROD)
		{
			var qtyConverted = Convert.ToDecimal(qty) * 1000;
        	var converted = Convert.ToInt32(qtyConverted);
			var SQLQuery = "SELECT PDLTTR FROM TESTDTA.F4311 (NOLOCK) WHERE PDDCTO = 'OQ' and PDURRF = @userRefNo and PDVR01 = @supplierSO and PDUOPN = @qty";
			var dic = new Dictionary<string, object> { { "@userRefNo", userRefNo }, { "@supplierSO", supplierSO }, { "@qty", converted.ToString()}};
            var creds = new SqlCredentials("CA15A2450", "JDE_DEVELOPMENT", "integration", CryptographyHelper.DecryptString("XrqqBbXPhNAVo7l1T2YU1Q=="));
            if (isPROD)
            {
            	Report.Info("Querying JDE PROD with QUERY: SELECT PDLTTR FROM PRODDTA.F4311 (NOLOCK) WHERE PDDCTO = 'OQ' and PDURRF = " + userRefNo + " and PDVR01 = " + supplierSO + " and PDUOPN = " + converted.ToString());
            	var SQLQueryPROD = "SELECT PDLTTR FROM PRODDTA.F4311 (NOLOCK) WHERE PDDCTO = 'OQ' and PDURRF = @userRefNo and PDVR01 = @supplierSO and PDUOPN = @qty";
            	var results = SqlHelper.ExecuteQuery(credsJDEPROD, SQLQueryPROD, dic).Tables[0];
            	return results.Rows[0][0].ToString();
            }
            else
            {
            	Report.Info("Querying JDE DEV with QUERY: SELECT PDLTTR FROM TESTDTA.F4311 (NOLOCK) WHERE PDDCTO = 'OQ' and PDURRF = " + userRefNo + " and PDVR01 = " + supplierSO + " and PDUOPN = " + converted.ToString());
            	var results = SqlHelper.ExecuteQuery(creds, SQLQuery, dic).Tables[0];
            	return results.Rows[0][0].ToString();
            }
           
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string SQLQueryDetailNumber(string rfqnumber, string DetailNo)
		{
			var SQLQuery = "SELECT ReferenceOrderLine, UserReference FROM tblDetail d (NOLOCK) INNER JOIN tblRFQ r (NOLOCK) ON r.RFQID = d.RFQID WHERE r.RFQNum = @rfqnumber AND d.DetailNo = @DetailNo";
			var dic = new Dictionary<string, object>{{"@rfqnumber", rfqnumber}, {"@DetailNo", DetailNo}};
		//	var creds = credsSPDev;
			var results = SqlHelper.ExecuteQuery(credsSPDev, SQLQuery, dic).Tables[0];
			for (int i = 0; i < results.Rows.Count; i++) 
			{
				for (int j = 0; j < results.Columns.Count; j++) 
				{
					Report.Info(results.Rows[i][j].ToString());
				}
			}
			
			return results.Rows[0][0].ToString();
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SQLUpdateSPDEV(string useremail, string supplierPortalChecked, string engContactChecked)
		{
			var SQLQUERY = "UPDATE [dbo].[tblEmployee] SET [ToolRoomContact] = @supplierPortalChecked, [EngContact] = @engContactChecked WHERE email = @useremail";
			var dic = new Dictionary<string, object>{{"@useremail", useremail}, {"@supplierPortalChecked", supplierPortalChecked}, {"@engContactChecked", engContactChecked}};
			var creds = credsSPDev;
			SqlHelper.ExecuteNonQuery(creds, SQLQUERY, dic);
			Report.Info("SQL statement executed");
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SQLSetSPDevDefaultSite(string useremail, string defaultSite)
		{
			var SQLQUERY = "UPDATE [dbo].[tblEmployee] SET [DefaultSite] = @defaultSite WHERE email = @useremail";
			var dic = new Dictionary<string, object>{{"@useremail", useremail}, {"@defaultSite", defaultSite}};
			var creds = credsSPDev;
			SqlHelper.ExecuteNonQuery(creds, SQLQUERY, dic);
			Report.Info("SQL statement executed to change default site to: " + defaultSite);
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void DeleteAnchorDanlyVendorLink()
		{
			var SQLQUERY = "DELETE [tblVendorErpLink] WHERE vendorid = @vendorid and Currency != @currency and SiteID IN (select code from [tblATWSites] where CompanyName = @location)";
			var dic = new Dictionary<string, object>{{"@vendorid", "ANC0005"}, {"@currency", "USD"}, {"@location", "Cambridge LS"}};
			var creds = credsSPDev;
			SqlHelper.ExecuteNonQuery(creds, SQLQUERY, dic);
			Report.Info("Deleting NON USD Anchor Danly vendor link by Executing SQL Query: DELETE [tblVendorErpLink] WHERE vendorid = 'ANC0005' and Currency != 'USD' and SiteID IN (select code from [tblATWSites] where CompanyName = 'Cambridge LS')");
			
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ChangeCurrencyPrefs()
		{
			var SQLQUERY = "UPDATE [dbo].[tblEmployee] Set UserPreferences = '{\"ReceiveEmail\":true,\"ShowLabels\":true,\"Currency\":\"USD\"}' WHERE email = @email";
			var dic = new Dictionary<string, object>{{"@email", "ranorexrt@atsautomation.com"}};
			var creds = credsSPDev;
			SqlHelper.ExecuteNonQuery(creds, SQLQUERY, dic);
			Report.Info("Updating User Currency Preferences to USD by Executing SQL Query: UPDATE [dbo].[tblEmployee] Set UserPreferences = '{\"ReceiveEmail\":true,\"ShowLabels\":true,\"Currency\":\"USD\"}' WHERE email = 'ranorexrt@atsautomatoin.com ");
		}
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SQLSetSPDevDefaultShippingMethod(string companyname)
		{
			var SQLQUERY = "UPDATE [dbo].[tblATWSites] SET [Preferences] = @prefs WHERE CompanyName = @companyName";
			var dic = new Dictionary<string, object>{{"@prefs", "{\"SendWinLossEmail\":false,\"DefaultShippingCode\":\"GND\"}"}, {"@companyName", companyname}};
			var creds = credsSPDev;
			SqlHelper.ExecuteNonQuery(creds, SQLQUERY, dic);
			Report.Info("SQL statement executed to change default shipping method to GND");
		}
		
		public static Random ran = new Random();
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string GenerateRandomTrackingNumber(int length)
		{
			const string chars = "A0123456789Z";
            return "T" + new string(Enumerable.Repeat(chars, length)
            .Select(s => s[ran.Next(s.Length)]).ToArray());	
		}
		
		private static Random random = new Random();
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string RandomString(int length)
		{
		//	Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789電買車紅無東馬風愛時鳥島語頭魚園長紙書見";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SetPriceinQuote(string NumEntries)
		{
			WebDocument webDoc = "/dom[@domain='ca10a2061.atsna.atsauto.net']";
			var webTable = webDoc.FindSingle(".//table[#'detailsTable']");
		//	var inputTag = webTable.FindSingle(".//input[@name='[0].DetailCost' and @visible='True']").As<InputTag>();
		//	Report.Info(inputTag.Id);
			
			for (int i = 0; i < Convert.ToInt32(NumEntries); i++) 
			{
		//		string rxPath = ".//input[@visible='True' and @name='[" + i.ToString() + "].DetailCost']";
				string rxPath = ".//input[@name='[" + i.ToString() + "].DetailCost' and @type='text']";
				var iTag = webTable.FindSingle(rxPath).As<InputTag>();
				iTag.MoveTo();
				iTag.Click();
		//		iTag.InnerText = GenerateRandomDecimal();
				Keyboard.Press(GenerateRandomDecimal() + "00");
		//		iTag.TagValue = GenerateRandomDecimal();
				string notesRxPath = "./tbody/tr[" + (i+1).ToString() + "]/td/textarea";
				var NotesTextArea = webTable.FindSingle(notesRxPath).As<TextAreaTag>();
				if (i==0 || i==2 || i==4)
				{
					NotesTextArea.Click();
					NotesTextArea.InnerText = "Here is Note number " + i.ToString();
				}
			}
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void GetUnitPrices(string NumEntries)
		{
			string pricePath = @"\\ca01a9001\pgmis\Deployment_DEV\Ranorex\Supplier_Portal\Web\prices.txt";
			WebDocument webDoc = "/dom[@domain='ca10a2061.atsna.atsauto.net']";
		//	var webTable = webDoc.FindSingle(".//table[@id='detailsTable']");
			var sList = new List<string>();
//			for (int i = 0; i < Convert.ToInt32(NumEntries); i++)
//			{
//				string rxPath = ".//input[@name='[" + i.ToString() + "].DetailCost']";
//			//	var iTag = webTable.FindSingle(rxPath).As<InputTag>();
//				var iTag = webTable.Find(rxPath).As<InputTag>();
//				foreach (var inputTag in iTag) 
//				{
//					Report.Info(inputTag.Value);
//				}
//				//sList.Add(iTag.Value);
//			}
//			
			string rxPath = ".//table[@id='detailsTable']//div[@safeclass='input-group']/input[@type='text' and @id='txtEachPrice']";
			IList<Ranorex.InputTag> iElements = webDoc.Find<Ranorex.InputTag>(rxPath);
			
//			foreach (var element in iElements) 
//			{
//				iTags.Add(element.As<InputTag>());
//			}
			foreach (var iTag in iElements)
			{
				Report.Info(iTag.Value);
				sList.Add(iTag.Value);
				
			}
			
			
			File.WriteAllLines(pricePath, sList);
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ValidateJDERecords(string NumEntries)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			WebDocument webDoc = "/dom[@domain='deve1' or @domain='qae1']";
			for (int i = 0; i < Convert.ToInt32(NumEntries); i++)
			{
				var webTableTag = webDoc.FindSingle(".//iframe[#'e1menuAppIframe']//div[#'FormAboveGrid']/span[2]/table/tbody/tr[2]/?/?/table//td/table/tbody/tr[2]//table/?/?/tr/td[2]/table/tbody/tr[" + (i+1).ToString() + "]/?/?/table/").As<Ranorex.TableTag>();
				var BuyerNumber = webTableTag.FindSingle("./tbody/tr/td[7]/div").As<Ranorex.DivTag>();
				var LastStatus = webTableTag.FindSingle("./tbody/tr/td[8]/div").As<Ranorex.DivTag>();
				var NextStatus = webTableTag.FindSingle("./tbody/tr/td[9]/div").As<Ranorex.DivTag>();
				var SupplierNumber = webTableTag.FindSingle("./?/?/tr/td[17]/div").As<Ranorex.DivTag>();
				var SupplierName = webTableTag.FindSingle("./?/?/tr/td[18]/div").As<Ranorex.DivTag>();
				if (GetUsername() == "anpatel")
				{
					Ranorex.Validate.IsTrue(BuyerNumber.InnerText == "18118", "Validating Buyer Number for buyer 'anpatel', the result @ValidateRESULT@");
				}
				if (GetUsername() == "ranorexrt")
				{
					Ranorex.Validate.IsTrue(BuyerNumber.InnerText == "192257", "Validating Buyer Number for buyer 'ranorexrt', the result @ValidateRESULT@");
				}
				Ranorex.Validate.IsTrue(LastStatus.InnerText == "230", "Validating Last Status = 230, the result @ValidateRESULT@");
				Ranorex.Validate.IsTrue(NextStatus.InnerText == "280", "Validating Next Status = 280, the result @ValidateRESULT@");
				if (SupplierName.InnerText == "ATS ADVISORS")
				{
					Ranorex.Validate.IsTrue(SupplierNumber.InnerText == "182790", "Validating Supplier Number for ATS ADVISORS, the result @ValidateRESULT@");
				}
				if (SupplierName.InnerText == "ANCHOR DANLY")
				{
					Ranorex.Validate.IsTrue(SupplierNumber.InnerText == "1184173", "Validating Supplier Number for ANCHOR DANLY, the result @ValidateRESULT@");
				}
				
				
				
				
			}
			
			
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ConfirmPOJDE(string NumEntries)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			WebDocument webDoc = "/dom[@path~'/jde/']";
			for (int i = 0; i < Convert.ToInt32(NumEntries); i++)
			{
				var webTableTag = webDoc.FindSingle(".//iframe[#'e1menuAppIframe']//div[#'FormAboveGrid']/span[2]/table/tbody/tr[2]/?/?/table//td/table/tbody/tr[2]//table/?/?/tr/td[2]/table/tbody/tr[" + (i+1).ToString() + "]/?/?/table/").As<Ranorex.TableTag>();
				var ConfirmCode = webTableTag.FindSingle("./?/?/tr/td[31]/div").As<Ranorex.DivTag>();
				Ranorex.Validate.IsTrue(ConfirmCode.InnerText == "C", "Validating Report Code = 'C', the result @ValidateRESULT@");
			}
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ValidateFilterStatusRFQs(string NumRecords, string rxPathofElement, string restofPath, string validateText, bool checkBox, bool isChecked)
		{
			
			ATS_Supplier_Portal_TestRepository repo = ATS_Supplier_Portal_TestRepository.Instance;
			Delay.Seconds(5.0);
			string varNumRecords;
			WebDocument webDoc = "/dom[@domain='ca10a2061.atsna.atsauto.net' or @domain~'supplierportal.*']";
			if(rxPathofElement.Contains("#'posTable'"))
			   {
			   		varNumRecords = repo.LoginATSSupplierPortal.PosTableInfo.Element.GetAttributeValueText("InnerText", new Regex("(?<=\\w+\\s\\d+\\sto\\s)\\d+"));
			   		NumRecords = varNumRecords;
					Report.Info("Number of records is: " + NumRecords);
			   }
			else if(rxPathofElement.Contains("#'tblRfqs'"))
			        {
			        	varNumRecords = repo.LoginATSSupplierPortal.TblRfqsInfo.Element.GetAttributeValueText("InnerText", new Regex("(?<=\\w+\\s\\d+\\sto\\s)\\d+"));
			        	NumRecords = varNumRecords;
						Report.Info("Number of records is: " + NumRecords);
			        }
			
			if (Convert.ToInt32(NumRecords) == 0)
			{
				Report.Info("There are No entries showing for this Filter status");
				return;
			}
			for (int i = 0; i < Convert.ToInt32(NumRecords); i++)
			{
				var RxWebElement = webDoc.FindSingle(rxPathofElement + "/tr[" + (i+1).ToString() + "]" + restofPath);
				
				if (checkBox)
				{
							
					var cBox = RxWebElement.As<InputTag>();
					bool Status = Convert.ToBoolean(cBox.Checked);
					if (isChecked)
					{
						Ranorex.Validate.IsTrue(Status, "Validating if the checkbox is checked, the validation @ValidateRESULT@!");
					}
					else
					{
						Ranorex.Validate.IsFalse(Status, "Validating if the checkbox is unchecked, the validation @ValidateRESULT@!");
					}
				}
				
				else
				{
					var TDTag = RxWebElement.As<TdTag>();
					Ranorex.Validate.IsTrue(validateText.Contains(TDTag.InnerText), "Validating if status: " + TDTag.InnerText.ToString() + " contains " + validateText + ". The validation @ValidateRESULT@!");
				}
			}
			
			
		}
		private static Random rdecimal = new Random();
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string GenerateRandomDecimal()
		{
		//	Random rdecimal = new Random();
			return Math.Round(((rdecimal.NextDouble() * 10.0)+ 2.0),2).ToString();
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static int GetColumnIndexofTable(string rxPathofTable, string colName, bool returnRowIndex)
		{
		//	var repo = ATS_Supplier_Portal_TestRepository.Instance;
			
			var rTable = Ranorex.Table.FromPath(rxPathofTable);
//			var rList = rTable.FindSingle<Ranorex.List>(".//list[@automationid='PART_ColumnHeadersPresenter']");
//			//var myList = ListCollection.FirstOrDefault(x => x.GetAttributeValue<Ranorex.List>("AutomationId") == "PART_ColumnHeadersPresenter");
//			var rCell = rList.FindSingle(".//cell[@text~'" + colName + "']").As<Ranorex.Cell>();
			if (returnRowIndex)
			{
				Report.Info("Searching for the Row Index of Row Name: " + colName + " in the table...");
				var rRow = rTable.FindSingle(".//cell[@text~'" + colName + "']").As<Ranorex.Cell>();
				return rRow.RowIndex;
			}
			else
			{
				Report.Info("Searching for the Column Index of Column Name: " + colName + " in the table...");
				var rList = rTable.FindSingle<Ranorex.List>(".//list[@automationid='PART_ColumnHeadersPresenter']");
				var rCell = rList.FindSingle(".//cell[@text~'" + colName + "']").As<Ranorex.Cell>();
				return rCell.ColumnIndex;
			}
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ValidatePricesinDetailsTab()
		{
			string tableRXpath = "/form[@title='MainWindow']/list[@automationid='navigationPanel']//table[@automationid='ProcessedDetailsGrid']";
			int colIndex = GetColumnIndexofTable(tableRXpath, "ANCHOR DANLY", false);
			var rTable = Ranorex.Table.FromPath(tableRXpath);
			var rRowList = rTable.FindDescendants<Ranorex.Row>();
			var pListinColumn = new List<string>();
			var priceList = File.ReadAllLines(@"\\ca01a9001\pgmis\Deployment_DEV\Ranorex\Supplier_Portal\Web\prices.txt");
			foreach (var row in rRowList) 
			{
				var rCell = row.FindSingle("./list/cell[@columnindex='" + (colIndex - 7).ToString() + "']").As<Ranorex.Cell>();
				pListinColumn.Add(rCell.Text);
				rCell.EnsureVisible();
				Report.Screenshot(rCell, true);
			}
			
			var convertedPriceList = pListinColumn.ToArray();
			if (priceList.Length == convertedPriceList.Length)
			{
				for (int i=0; i<priceList.Length; i++)
				{
					Ranorex.Validate.IsTrue(priceList[i]==convertedPriceList[i], "Checking if the price from Web: " + priceList[i] + " matches the price in SP Details: " + convertedPriceList[i] + ".  The validation @ValidateRESULT@");
				}
			}
			else
			{
				Ranorex.Validate.Fail("The prices do not match!");
			}
			
		}
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string ReturnCellValue(string rxPathofTable, int rowIndex, int colIndex, bool returnCBoxstate, bool clickObject)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			var rTable = Ranorex.Table.FromPath(rxPathofTable);
			var rRows = rTable.FindDescendants<Ranorex.Row>();
			if(returnCBoxstate)
			{
				var cellList = rRows[rowIndex].FindDescendants<Ranorex.Cell>();
				var cBox = cellList[colIndex].FindChild<Ranorex.CheckBox>();
				Ranorex.Validate.IsTrue(cBox.Checked, "Checking if the checkbox is checked, the validation @ValidateRESULT@");
				Report.Screenshot(cBox, true);
				return "true";
			}
			
			else
			{
				var cellList = rRows[rowIndex].FindDescendants<Ranorex.Cell>();
				Report.Screenshot(cellList[colIndex], true);
				if (clickObject)
				{
					cellList[colIndex].Click();
				}
				return cellList[colIndex].Text.ToString();
			}
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void FilterValidateTable(string columnName, string textToCompare, bool compareComboBox)
		{
			int rowCount = 0;
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			textToCompare = textToCompare.ToUpper();
			int colIndex = GetColumnIndexofTable(repo.MainWindow.NavigationPanel.ProcessDetailsGrid.GetPath().ToString(), columnName, false);
			
			if (colIndex.ToString() == null)
			{
				Report.Failure("Unable to Find column name: " + columnName);
			}
			var ProcessedTable = repo.MainWindow.NavigationPanel.ProcessDetailsGrid;
			var rTable = ProcessedTable.FindDescendants<Ranorex.Row>();
			foreach (var rRow in rTable) 
			{
				var cellList = rRow.FindDescendants<Ranorex.Cell>();
				
				foreach (var cell in cellList) 
				{
					if (cell.ColumnIndex == colIndex)
					{
						if (compareComboBox)
						{
							if (rTable.Count >0)
							{
									var cBox = cell.FindDescendant<Ranorex.Text>();
							//		Report.Log(ReportLevel.Info, cBox.TextValue);
							//		Report.Log(ReportLevel.Info, textToCompare);
									if (cBox.TextValue.ToUpper() == textToCompare)
									{
										Ranorex.Validate.IsTrue(cBox.TextValue.ToUpper() == textToCompare, "The validation result @ValidateRESULT@");
										cBox.EnsureVisible();
										Report.Screenshot(cell, true);
									}
							}
						}
						
						else
						{
							if (cell.Text.Contains(textToCompare))
							{
								Ranorex.Validate.IsTrue(cell.Text.Contains(textToCompare), "The validation result @ValidateRESULT@");
								cell.EnsureVisible();
								Report.Screenshot(cell, true);
							}
						}
						
					}
					
				}
			}
			if (rTable != null) 
			{
				rowCount = rTable.Count;				
			}
			
			
			Report.Log(ReportLevel.Info, "Row count of ProcessedDetailsGrid Table is: " + rowCount);
			
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void SortDetailColumns(string columnName)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			int colIndex = GetColumnIndexofTable(repo.MainWindow.NavigationPanel.ProcessDetailsGrid.GetPath().ToString(), columnName, false);
			if (colIndex.ToString() == null)
			{
				Report.Failure("Unable to Find column name: " + columnName);
			}
			var ProcessedTableContainer = repo.MainWindow.NavigationPanel.ProcessDetailsGrid;
			var rTable = ProcessedTableContainer.FindDescendants<Ranorex.Row>();
			string previousValue = "";
			foreach (var rRow in rTable) 
			{
				var cellList = rRow.FindDescendants<Ranorex.Cell>();
				
				foreach (var cell in cellList) 
				{
					if (cell.ColumnIndex == colIndex)
					{
						Ranorex.ComboBox cBox = null;
						if (cell.TryFindSingle<Ranorex.ComboBox>("./combobox", out cBox))
						    {
								if (previousValue == "")
								{
									previousValue = cBox.Text;
									Report.Screenshot(cBox, true);
								}
								
								else
								{
									var result = String.Compare(previousValue, cBox.Text);
									Report.Info("Previous Value: " + previousValue);
									Report.Info("Current Cell Value: " + cBox.Text);
									Report.Screenshot(cBox, true);
									Ranorex.Validate.IsTrue(result<=0, "Checking status of sort compare: @ValidateRESULT@", true);
									previousValue = cBox.Text;
								}
								
						    }
						
						
						
						
						else
						{
						
							if (previousValue == "")
							{
								previousValue = cell.Text;
							}
							
							else
							{
								var result = String.Compare(previousValue, cell.Text);
								Report.Info("Previous Value: " + previousValue);
								Report.Info("Current Cell Value: " + cell.Text);
								Ranorex.Validate.IsTrue(result<=0, "Checking status of sort compare: @ValidateRESULT@", true);
								previousValue = cell.Text;
							}
						}
					}
				}
			
			}
		}
		
		
		 
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string ConvertDate(string datetoConvert)
		{
			return System.DateTime.ParseExact(datetoConvert, "mm/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("mm/dd/yy");
		}
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static string ConvertDateYYYY(string datetoConvert)
		{
			return System.DateTime.ParseExact(datetoConvert, "mm/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("mm/dd/yyyy");
		}
		
		[UserCodeMethod]
		public static string GetCountofWinner(string columnName, string supplierName)
		{
			var repo = ATS_Supplier_Portal_TestRepository.Instance;
			int colIndex = GetColumnIndexofTable(repo.MainWindow.NavigationPanel.ProcessDetailsGrid.GetPath().ToString(), columnName, false);
			if (colIndex.ToString() == null)
			{
				Report.Failure("Unable to Find column name: " + columnName);
			}
			var ProcessedTableContainer = repo.MainWindow.NavigationPanel.ProcessDetailsGrid;
			var rTable = ProcessedTableContainer.FindDescendants<Ranorex.Row>();
			
			int count = 0;
			
			foreach (var rRow in rTable)  
			{
				var cellList = rRow.FindDescendants<Ranorex.Cell>();
				
				foreach (var cell in cellList) 
				{
					if (cell.ColumnIndex == colIndex)
					{
						Ranorex.ComboBox cBox = null;
						if (cell.TryFindSingle<Ranorex.ComboBox>("./combobox", out cBox))
						    {
								if (cBox.Text == supplierName)
								{
									count++;
								}
								
						    }
					}
				}
			}
			
			return count.ToString();
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void UpdateExcel(string workbookPath, string sheetName, string columnName, string startRowNum)
		{
			Microsoft.Office.Interop.Excel.Worksheet ws = null;
            Microsoft.Office.Interop.Excel.Range last = null;
            Microsoft.Office.Interop.Excel.Range dataRange = null;
            Workbook excelWorkbook = null;
            Microsoft.Office.Interop.Excel.Application Excel = null;
            string strToWrite = "";
            
           
            try
            {
                Excel = new Microsoft.Office.Interop.Excel.Application();

                excelWorkbook = Excel.Workbooks.Open(workbookPath,
                        0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "",
                        true, false, 0, true, false, false);


                ws = Excel.Worksheets[sheetName] as Worksheet;
                var sheetRange = ws.UsedRange;
          //      last = ws.get_Range("A" + ws.Rows.Count).get_End(XlDirection.xlUp);
           //     Report.Info(last.Rows.Count.ToString());
          //      dataRange = ws.get_Range(columnName + startRowNum, columnName + ws.Rows.Count);
          //      dataRange = ws.get_Range(ws.Cells[2,8], ws.Cells[43,8]);
                  dataRange = ws.get_Range(columnName + startRowNum, columnName + sheetRange.Rows.Count.ToString());

                object[,] cellValues = dataRange.Value2 as object[,];
                string colName = (ws.Cells[1,columnName as Object] as Range).Value2.ToString();
                Report.Info((ws.Cells[1,columnName as Object] as Range).Value2.ToString());
                

                for (int i = 1; i < cellValues.Length + 1; i++)
                {
                	
                	if(colName == "Promise Date")
                	{                		
                		strToWrite = GetDateTimePlusXDays(5);
                		if(i==1)
                		{
                			Report.Info("Cell Value is " + colName + ", going to next row...");
                			
                		}
                		else
                		{
                			cellValues[i, 1] = strToWrite;
                			File.WriteAllText(@"\\ca01a9001\pgmis\Deployment_DEV\Ranorex\Supplier_Portal\PROD\RFQExport\PromiseDate.txt", strToWrite);
                		}
                	}
                		
                			
                	if(colName == "Unit Price")
                	{
                		
                		if(i==1)
                		{
                			Report.Info("Cell Value is " + colName + ", going to next row...");
                			
                		}
                		else
                		{
                			strToWrite = GenerateRandomDecimal();
                			Report.Info("Random Number Generated is: " + strToWrite);
                			if (strToWrite.Contains("."))
                			{
                				var substr = strToWrite.Substring(strToWrite.IndexOf('.') + 1);
            					if (substr.Length == 1)
            					{
                					strToWrite = strToWrite + "0";
                					Report.Info("Converted strToWrite Value after adding a '0': " + strToWrite + "00");
            					}
                			}
                			else
                			{
                				strToWrite = strToWrite + ".00";
                				Report.Info("Converted strToWrite Value after adding a '.00': " + strToWrite + "00");
                			}
                			cellValues[i, 1] = strToWrite;
                			File.WriteAllText(@"\\ca01a9001\pgmis\Deployment_DEV\Ranorex\Supplier_Portal\PROD\RFQExport\UnitPrice.txt", strToWrite + "00");
                		}
                	}
                		
                			
                	if(colName == "Note")
                	{
                		strToWrite = "This is Note number: " + i.ToString() + " " + RandomString(4);
                		if(i==1)
                		{
                			Report.Info("Cell Value is " + colName + ", going to next row...");
                			
                		}
                		else
                		{
                			cellValues[i, 1] = strToWrite;
                			File.WriteAllText(@"\\ca01a9001\pgmis\Deployment_DEV\Ranorex\Supplier_Portal\PROD\RFQExport\Note.txt", strToWrite);
                		}
                	}
                			
                	
                	
                }

                dataRange.Value2 = cellValues;
                Excel.DisplayAlerts = false;
                excelWorkbook.Close(SaveChanges: true, Filename: workbookPath);//No prompt when overwriting file
                Excel.Quit();
                Report.Log(ReportLevel.Info, "Done writing to file: " + workbookPath);

            }
            catch (Exception ex)
            {
            	Report.Log(ReportLevel.Error, "Exception: " + ex.StackTrace.ToString());
            }

            finally
            {
                if (last != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(last);
                if (dataRange != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(dataRange);
                if (ws != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(ws);

                if (excelWorkbook != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorkbook);

                if (Excel != null)
                {
                    Excel.Quit();
                }
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Excel);
            }
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ValidateExcelandWeb(string workbookPath, string sheetName, string columnName, string startRowNum)
		{
			Microsoft.Office.Interop.Excel.Worksheet ws = null;
            Microsoft.Office.Interop.Excel.Range last = null;
            Microsoft.Office.Interop.Excel.Range dataRange = null;
            Workbook excelWorkbook = null;
            Microsoft.Office.Interop.Excel.Application Excel = null;
            string strToWrite = "";
            WebDocument webDoc = "/dom[@domain='ca10a2061.atsna.atsauto.net']";
			var webTable = webDoc.FindSingle(".//table[#'detailsTable']");
           
            try
            {
                Excel = new Microsoft.Office.Interop.Excel.Application();

                excelWorkbook = Excel.Workbooks.Open(workbookPath,
                        0, true, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "",
                        true, false, 0, true, false, false);


                ws = Excel.Worksheets[sheetName] as Worksheet;
                var sheetRange = ws.UsedRange;
                dataRange = ws.get_Range(columnName + startRowNum, columnName + sheetRange.Rows.Count.ToString());

                object[,] cellValues = dataRange.Value2 as object[,];
                string colName = (ws.Cells[1,columnName as Object] as Range).Value2.ToString();
                Report.Info((ws.Cells[1,columnName as Object] as Range).Value2.ToString());
                

                for (int i = 1; i < cellValues.Length + 1; i++)
                {
                	
                	if(colName == "Promise Date")
                	{                		
                		strToWrite = GetDateTimePlusXDays(5);
                		var DT2 = System.DateTime.FromOADate(Convert.ToDouble(cellValues[i, 1].ToString()));
                		var DT1 = System.DateTime.ParseExact(strToWrite, "MM/dd/yy", null);
                		Ranorex.Validate.IsTrue(System.DateTime.Equals(DT1, DT2), "Checking if Promise Date is equal to: " + strToWrite + ". The Validation @ValidateRESULT@!");
                	}
                		
                			
                	if(colName == "Unit Price")
                	{
                		string rxPath = ".//input[@name='[" + (i-1).ToString() + "].DetailCost']";
						var iTag = webTable.FindSingle(rxPath).As<InputTag>();
						var iTagNum = Convert.ToDecimal(iTag.Value);
						var iTagString = iTagNum.ToString("F2");
						Ranorex.Validate.IsTrue(Convert.ToDecimal(cellValues[i, 1].ToString()).ToString("F2") == iTagString, "Checking if Unit Prices are equal. The Validation @ValidateRESULT@!");
                	}
                		
                			
                	if(colName == "Note")
                	{
                		string notesRxPath = "./tbody/tr[" + (i).ToString() + "]/td/textarea";
						var NotesTextArea = webTable.FindSingle(notesRxPath).As<TextAreaTag>();
						Ranorex.Validate.IsTrue(cellValues[i, 1].ToString() == NotesTextArea.InnerText, "Checking if Notes values are equal. The Validation @ValidateRESULT@!");
                	}
                			
                	
                	
                }

                dataRange.Value2 = cellValues;
                Excel.DisplayAlerts = false;
                excelWorkbook.Close(SaveChanges: false, Filename: workbookPath);//No prompt when overwriting file
                Excel.Quit();
                Report.Log(ReportLevel.Info, "Done Validating file: " + workbookPath);

            }
            catch (Exception ex)
            {
            	Report.Log(ReportLevel.Error, "Exception: " + ex.StackTrace.ToString());
            }

            finally
            {
                if (last != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(last);
                if (dataRange != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(dataRange);
                if (ws != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(ws);

                if (excelWorkbook != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorkbook);

                if (Excel != null)
                {
                    Excel.Quit();
                }
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Excel);
            }
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void ValidateExcelContainsShippingDate(string workbookPath, string sheetName, string columnName, string startRowNum, string stringToFindinCol)
		{
			Microsoft.Office.Interop.Excel.Worksheet ws = null;
            Microsoft.Office.Interop.Excel.Range last = null;
            Microsoft.Office.Interop.Excel.Range dataRange = null;
            Workbook excelWorkbook = null;
            Microsoft.Office.Interop.Excel.Application Excel = null;
            bool flag = false;
            
           
            try
            {
                Excel = new Microsoft.Office.Interop.Excel.Application();

                excelWorkbook = Excel.Workbooks.Open(workbookPath,
                        0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "",
                        true, false, 0, true, false, false);


                ws = Excel.Worksheets[sheetName] as Worksheet;
           //     var sheetRange = ws.UsedRange;
          //      last = ws.get_Range("A" + ws.Rows.Count).get_End(XlDirection.xlUp);
           //     Report.Info(last.Rows.Count.ToString());
          //      dataRange = ws.get_Range(columnName + startRowNum, columnName + ws.Rows.Count);
          //      dataRange = ws.get_Range(ws.Cells[2,8], ws.Cells[43,8]);
        //          dataRange = ws.get_Range(columnName + startRowNum, columnName + sheetRange.Rows.Count.ToString());
        			dataRange = ws.UsedRange;

                object[,] cellValues = dataRange.Value2 as object[,];
                string colName = (ws.Cells[1,columnName as Object] as Range).Value2.ToString();
                Report.Info((ws.Cells[1,columnName as Object] as Range).Value2.ToString());
                

                if(colName == "Requested Ship Date")
                {
                	
                	 for (int i = 1; i < dataRange.Rows.Count + 1; i++)
                	{                		
                		var result =  (ws.Cells[i, columnName as Object] as Range).Value2.ToString();
                		if(result == stringToFindinCol)
                		{
                	 		flag = true;
                		}
                	}
                		           	
                }

   //             dataRange.Value2 = cellValues;
                Excel.DisplayAlerts = false;
                excelWorkbook.Close(SaveChanges: true, Filename: workbookPath);//No prompt when overwriting file
                Excel.Quit();
                Report.Log(ReportLevel.Info, "Done writing to file: " + workbookPath);
                Ranorex.Validate.IsTrue(flag,  "Searching for: " + stringToFindinCol + " in Excel File with Column: " + colName + " .The Validation @ValidateRESULT@!");

            }
            catch (Exception ex)
            {
            	Report.Log(ReportLevel.Error, "Exception: " + ex.StackTrace.ToString());
            }

            finally
            {
                if (last != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(last);
                if (dataRange != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(dataRange);
                if (ws != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(ws);

                if (excelWorkbook != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorkbook);

                if (Excel != null)
                {
                    Excel.Quit();
                }
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Excel);
            }
		}
	}
	
	
}
