/*
 * Created by Ranorex
 * User: anpatel
 * Date: 9/7/2021
 * Time: 10:31 PM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ATS_Supplier_Portal_Test
{
	/// <summary>
	/// Description of CogServ.
	/// </summary>
	public class CogServ
	{
	//	static string subscriptionKey = "09fc9f2d062244afa8dbdfb12b617ff4";
//        static string endpoint = "https://computer-vision-ocr-it.cognitiveservices.azure.com/";
//        private const string READ_TEXT_LOCAL_IMAGE = @"C:\Users\anpatel\Pictures\Ranorex\test.png";
		public CogServ()
		{
			// ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
			// ReadFileLocal(client, READ_TEXT_LOCAL_IMAGE).Wait();
		}
		
		public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }
		
		public static async Task<bool> ReadFileLocal(ComputerVisionClient client, string localFile, string searchWord)
        {
            Ranorex.Report.Info("----------------------------------------------------------");
            Ranorex.Report.Info("READ FILE FROM LOCAL");
            Ranorex.Report.Info("Searching for word: " + searchWord);
			bool flag = false;
            // Read text from URL
            var textHeaders = await client.ReadInStreamAsync(File.OpenRead(localFile));
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            // <snippet_extract_response>
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Ranorex.Report.Info("Reading text from local file");
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            // </snippet_extract_response>

            // <snippet_extract_display>
            // Display the found text.
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            
            foreach (ReadResult page in textUrlFileResults)
            {
                
                foreach (Line line in page.Lines)
                {
                   

         //           Ranorex.Report.Info(line.Text);
                    if(line.Text.Contains(searchWord))
                    {
                        foreach (var word in line.Words)
                        {
                        //	Ranorex.Report.Info(word.Text);
                        	
                        	if (word.Text == searchWord && word.Confidence>0.9)
                            {
                        		Ranorex.Report.Info("\n\nFound: " + searchWord + " with confidence of: " + ((word.Confidence)*100).ToString() + "%, getting Bounding Box Coordinates\n\n");
                                var bbox = word.BoundingBox;
                                var coordarray = bbox.ToArray();
                                var midpointtoclick = midpoint(coordarray);
                                Ranorex.Report.Info("Clicking on the coordinates...");
                                Ranorex.Mouse.MoveTo(midpointtoclick);
                                Ranorex.Delay.Seconds(1.0);
                                Ranorex.Mouse.Click(midpointtoclick);
                                Ranorex.Report.Info("Success!");
                                flag = true;
                              /*  foreach (var coord in bbox)
                                {
                                	Ranorex.Report.Info(coord.ToString());
                                } */

                            }

                        }
                    }

                }
            }
           
            if(!flag)
            {
            	Ranorex.Report.Info(searchWord + " not found!");
            }
            Ranorex.Report.Info("Done");
            return flag;
        }
		 
		 public static Point midpoint(double?[] coords)
        {
            Point mid = new Point();
            double? x1, x2, x3, x4, y1, y2, y3, y4, x, y;
            int xmid, ymid;
            x1 = coords[0];
            y1 = coords[1];
            x2 = coords[2];
            y2 = coords[3];
            x3 = coords[4];
            y3 = coords[5];
            x4 = coords[6];
            y4 = coords[7];

            x = x1 + ((x2 - x1) / 2);
            y = y2 + ((y4 - y2) / 2);

            xmid = Convert.ToInt32(x);
            ymid = Convert.ToInt32(y);

            Ranorex.Report.Info("Coordinates for midpoint of bounding box are = (" + xmid.ToString() + "," + ymid.ToString() + ")");
            mid.X = xmid;
            mid.Y = ymid;
            return mid;
        }
	}
}
