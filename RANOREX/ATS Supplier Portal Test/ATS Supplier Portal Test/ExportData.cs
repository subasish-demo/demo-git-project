﻿///////////////////////////////////////////////////////////////////////////////
//
// This file was automatically generated by RANOREX.
// DO NOT MODIFY THIS FILE! It is regenerated by the designer.
// All your modifications will be lost!
// http://www.ranorex.com
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using Ranorex.Core.Repository;

namespace ATS_Supplier_Portal_Test
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The ExportData recording.
    /// </summary>
    [TestModule("a47193c0-d290-4e99-8177-4753caa565a0", ModuleType.Recording, 1)]
    public partial class ExportData : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ATS_Supplier_Portal_TestRepository repository.
        /// </summary>
        public static ATS_Supplier_Portal_TestRepository repo = ATS_Supplier_Portal_TestRepository.Instance;

        static ExportData instance = new ExportData();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ExportData()
        {
            varRFQNumber = "";
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ExportData Instance
        {
            get { return instance; }
        }

#region Variables

        /// <summary>
        /// Gets or sets the value of variable varRFQNumber.
        /// </summary>
        [TestVariable("9f9af140-8652-47b5-bf51-6c8da983666b")]
        public string varRFQNumber
        {
            get { return repo.varRFQNumber; }
            set { repo.varRFQNumber = value; }
        }

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", global::Ranorex.Core.Constants.CodeGenVersion)]
        public static void Start()
        {
            TestModuleRunner.Run(Instance);
        }

        /// <summary>
        /// Performs the playback of actions in this recording.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", global::Ranorex.Core.Constants.CodeGenVersion)]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            varRFQNumber = SPCollection.ReadRequisitionFile("\\\\ca01a9001\\pgmis\\Deployment_DEV\\Ranorex\\Supplier_Portal\\RFQNumber\\StaticRFQNumber.txt");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'LoginATSSupplierPortal.Search' at Center.", repo.LoginATSSupplierPortal.SearchInfo, new RecordItemIndex(1));
            repo.LoginATSSupplierPortal.Search.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence from variable '$varRFQNumber' with focus on 'LoginATSSupplierPortal.Search'.", repo.LoginATSSupplierPortal.SearchInfo, new RecordItemIndex(2));
            repo.LoginATSSupplierPortal.Search.PressKeys(varRFQNumber);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'LoginATSSupplierPortal.SelectRFQNumber' at Center.", repo.LoginATSSupplierPortal.SelectRFQNumberInfo, new RecordItemIndex(3));
            repo.LoginATSSupplierPortal.SelectRFQNumber.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 10s to exist. Associated repository item: 'LoginATSSupplierPortal.BtnSetPromise'", repo.LoginATSSupplierPortal.BtnSetPromiseInfo, new ActionTimeout(10000), new RecordItemIndex(4));
            repo.LoginATSSupplierPortal.BtnSetPromiseInfo.WaitForExists(10000);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'LoginATSSupplierPortal.BtnExport' at Center.", repo.LoginATSSupplierPortal.BtnExportInfo, new RecordItemIndex(5));
            repo.LoginATSSupplierPortal.BtnExport.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'VendorQuoteATSSupplierPortalInt.Options' at Center.", repo.VendorQuoteATSSupplierPortalInt.OptionsInfo, new RecordItemIndex(6));
            repo.VendorQuoteATSSupplierPortalInt.Options.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Iexplore.SaveAs' at Center.", repo.Iexplore.SaveAsInfo, new RecordItemIndex(7));
            repo.Iexplore.SaveAs.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Set value", "Setting attribute Text to '' on item 'SaveAs.FileName'.", repo.SaveAs.FileNameInfo, new RecordItemIndex(8));
            repo.SaveAs.FileName.Element.SetAttributeValue("Text", "");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Set value", "Setting attribute Text to '\\\\ca01a9001\\pgmis\\Deployment_DEV\\Ranorex\\Supplier_Portal\\Web\\QuoteExport\\12491.xlsx' on item 'SaveAs.FileName'.", repo.SaveAs.FileNameInfo, new RecordItemIndex(9));
            repo.SaveAs.FileName.Element.SetAttributeValue("Text", "\\\\ca01a9001\\pgmis\\Deployment_DEV\\Ranorex\\Supplier_Portal\\Web\\QuoteExport\\12491.xlsx");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SaveAs.ButtonSave' at Center.", repo.SaveAs.ButtonSaveInfo, new RecordItemIndex(10));
            repo.SaveAs.ButtonSave.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ConfirmSaveAs.ButtonYes' at Center.", repo.ConfirmSaveAs.ButtonYesInfo, new RecordItemIndex(11));
            repo.ConfirmSaveAs.ButtonYes.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
