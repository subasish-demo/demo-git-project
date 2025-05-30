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
    ///The CreateRFQforSelectWinner recording.
    /// </summary>
    [TestModule("92e2eba0-48a2-4f28-9ee4-c83e5e7a7b48", ModuleType.Recording, 1)]
    public partial class CreateRFQforSelectWinner : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ATS_Supplier_Portal_TestRepository repository.
        /// </summary>
        public static ATS_Supplier_Portal_TestRepository repo = ATS_Supplier_Portal_TestRepository.Instance;

        static CreateRFQforSelectWinner instance = new CreateRFQforSelectWinner();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CreateRFQforSelectWinner()
        {
            varRFQNumber = "";
            varDate = "";
            varcolumnIndex = "";
            varATSReqDate = "";
            varShippingMethodText = "";
            varDateIncorrect = "";
            varUserReferenceNumberRow0 = "";
            varDetailRow0 = "";
            varQtyRow0 = "";
            varUserReferenceNumber = "";
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static CreateRFQforSelectWinner Instance
        {
            get { return instance; }
        }

#region Variables

        string _varDate;

        /// <summary>
        /// Gets or sets the value of variable varDate.
        /// </summary>
        [TestVariable("1973c579-6082-4ba5-a453-cffacf50c82e")]
        public string varDate
        {
            get { return _varDate; }
            set { _varDate = value; }
        }

        string _varATSReqDate;

        /// <summary>
        /// Gets or sets the value of variable varATSReqDate.
        /// </summary>
        [TestVariable("18ea7126-f4a8-4738-ab54-f0946acf1022")]
        public string varATSReqDate
        {
            get { return _varATSReqDate; }
            set { _varATSReqDate = value; }
        }

        string _varShippingMethodText;

        /// <summary>
        /// Gets or sets the value of variable varShippingMethodText.
        /// </summary>
        [TestVariable("0ff5dacf-412b-43aa-ba32-697fd1d52f0f")]
        public string varShippingMethodText
        {
            get { return _varShippingMethodText; }
            set { _varShippingMethodText = value; }
        }

        string _varDateIncorrect;

        /// <summary>
        /// Gets or sets the value of variable varDateIncorrect.
        /// </summary>
        [TestVariable("99bdfce8-eb01-4aef-9146-7f1f4da5772c")]
        public string varDateIncorrect
        {
            get { return _varDateIncorrect; }
            set { _varDateIncorrect = value; }
        }

        string _varUserReferenceNumberRow0;

        /// <summary>
        /// Gets or sets the value of variable varUserReferenceNumberRow0.
        /// </summary>
        [TestVariable("11e49a47-0541-4112-bb18-9bf5b73a3ff6")]
        public string varUserReferenceNumberRow0
        {
            get { return _varUserReferenceNumberRow0; }
            set { _varUserReferenceNumberRow0 = value; }
        }

        string _varDetailRow0;

        /// <summary>
        /// Gets or sets the value of variable varDetailRow0.
        /// </summary>
        [TestVariable("6db7e785-9b54-4d40-915a-275ef75a5812")]
        public string varDetailRow0
        {
            get { return _varDetailRow0; }
            set { _varDetailRow0 = value; }
        }

        string _varQtyRow0;

        /// <summary>
        /// Gets or sets the value of variable varQtyRow0.
        /// </summary>
        [TestVariable("6b2f4262-ceb4-4d33-a6a5-8d0ba5043e66")]
        public string varQtyRow0
        {
            get { return _varQtyRow0; }
            set { _varQtyRow0 = value; }
        }

        string _varUserReferenceNumber;

        /// <summary>
        /// Gets or sets the value of variable varUserReferenceNumber.
        /// </summary>
        [TestVariable("ff45f269-0b08-45eb-8c70-0744ddc27438")]
        public string varUserReferenceNumber
        {
            get { return _varUserReferenceNumber; }
            set { _varUserReferenceNumber = value; }
        }

        /// <summary>
        /// Gets or sets the value of variable varcolumnIndex.
        /// </summary>
        [TestVariable("b9499348-d3cf-41bf-b00e-71da8635fc4a")]
        public string varcolumnIndex
        {
            get { return repo.varcolumnIndex; }
            set { repo.varcolumnIndex = value; }
        }

        /// <summary>
        /// Gets or sets the value of variable varRFQNumber.
        /// </summary>
        [TestVariable("46910cc8-dee0-4625-a668-e1bae3917a74")]
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

            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.CreateRFQ' at Center.", repo.MainWindow.NavigationPanel.CreateRFQInfo, new RecordItemIndex(0));
            repo.MainWindow.NavigationPanel.CreateRFQ.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 1m to not exist. Associated repository item: 'MainWindow.BusyOverlay'", repo.MainWindow.BusyOverlayInfo, new ActionTimeout(60000), new RecordItemIndex(1));
            repo.MainWindow.BusyOverlayInfo.WaitForNotExists(60000);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.VendorRequirements' at Center.", repo.MainWindow.NavigationPanel.VendorRequirementsInfo, new RecordItemIndex(2));
            repo.MainWindow.NavigationPanel.VendorRequirements.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 1m to not exist. Associated repository item: 'MainWindow.BusyOverlay'", repo.MainWindow.BusyOverlayInfo, new ActionTimeout(60000), new RecordItemIndex(3));
            repo.MainWindow.BusyOverlayInfo.WaitForNotExists(60000);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 5s.", new RecordItemIndex(4));
            Delay.Duration(5000, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'MainWindow.NavigationPanel.SupplierPortalContactDropDown' at Center.", repo.MainWindow.NavigationPanel.SupplierPortalContactDropDownInfo, new RecordItemIndex(5));
            repo.MainWindow.NavigationPanel.SupplierPortalContactDropDown.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.SupplierPortalContactDropDown' at Center.", repo.MainWindow.NavigationPanel.SupplierPortalContactDropDownInfo, new RecordItemIndex(6));
            repo.MainWindow.NavigationPanel.SupplierPortalContactDropDown.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ATSSupplierPortalManagment.PatelAnkit' at Center.", repo.ATSSupplierPortalManagment.PatelAnkitInfo, new RecordItemIndex(7));
            repo.ATSSupplierPortalManagment.PatelAnkit.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.SupplierPortalContactBackupDropDown' at Center.", repo.MainWindow.NavigationPanel.SupplierPortalContactBackupDropDownInfo, new RecordItemIndex(8));
            repo.MainWindow.NavigationPanel.SupplierPortalContactBackupDropDown.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ATSSupplierPortalManagment.PatelAnkit' at Center.", repo.ATSSupplierPortalManagment.PatelAnkitInfo, new RecordItemIndex(9));
            repo.ATSSupplierPortalManagment.PatelAnkit.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.EngineeringContactDropDown' at Center.", repo.MainWindow.NavigationPanel.EngineeringContactDropDownInfo, new RecordItemIndex(10));
            repo.MainWindow.NavigationPanel.EngineeringContactDropDown.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ATSSupplierPortalManagment.PatelAnkit' at Center.", repo.ATSSupplierPortalManagment.PatelAnkitInfo, new RecordItemIndex(11));
            repo.ATSSupplierPortalManagment.PatelAnkit.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.EngineeringContactBackupDropDown' at Center.", repo.MainWindow.NavigationPanel.EngineeringContactBackupDropDownInfo, new RecordItemIndex(12));
            repo.MainWindow.NavigationPanel.EngineeringContactBackupDropDown.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ATSSupplierPortalManagment.PatelAnkit' at Center.", repo.ATSSupplierPortalManagment.PatelAnkitInfo, new RecordItemIndex(13));
            repo.ATSSupplierPortalManagment.PatelAnkit.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.ShippingMethodComboBox' at Center.", repo.MainWindow.NavigationPanel.ShippingMethodComboBoxInfo, new RecordItemIndex(14));
            repo.MainWindow.NavigationPanel.ShippingMethodComboBox.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ATSSupplierPortalManagment.OCNOcean' at Center.", repo.ATSSupplierPortalManagment.OCNOceanInfo, new RecordItemIndex(15));
            repo.ATSSupplierPortalManagment.OCNOcean.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Get Value", "Getting attribute 'Text' from item 'MainWindow.NavigationPanel.ShippingMethodComboBoxText' and assigning its value to variable 'varShippingMethodText'.", repo.MainWindow.NavigationPanel.ShippingMethodComboBoxTextInfo, new RecordItemIndex(16));
            varShippingMethodText = repo.MainWindow.NavigationPanel.ShippingMethodComboBoxText.Element.GetAttributeValueText("Text");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "User", varShippingMethodText, new RecordItemIndex(17));
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.VendorSelection' at Center.", repo.MainWindow.NavigationPanel.VendorSelectionInfo, new RecordItemIndex(18));
            repo.MainWindow.NavigationPanel.VendorSelection.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 30s to not exist. Associated repository item: 'MainWindow.BusyOverlay'", repo.MainWindow.BusyOverlayInfo, new ActionTimeout(30000), new RecordItemIndex(19));
            repo.MainWindow.BusyOverlayInfo.WaitForNotExists(30000);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.ANCHORDANLY' at Center.", repo.MainWindow.NavigationPanel.ANCHORDANLYInfo, new RecordItemIndex(20));
            repo.MainWindow.NavigationPanel.ANCHORDANLY.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.ATSAUTOMATION1' at Center.", repo.MainWindow.NavigationPanel.ATSAUTOMATION1Info, new RecordItemIndex(21));
            repo.MainWindow.NavigationPanel.ATSAUTOMATION1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating Exists on item 'MainWindow.NavigationPanel.ANCHORDANLY1'.", repo.MainWindow.NavigationPanel.ANCHORDANLY1Info, new RecordItemIndex(22));
            Validate.Exists(repo.MainWindow.NavigationPanel.ANCHORDANLY1Info);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating Exists on item 'MainWindow.NavigationPanel.ATSAUTOMATION11'.", repo.MainWindow.NavigationPanel.ATSAUTOMATION11Info, new RecordItemIndex(23));
            Validate.Exists(repo.MainWindow.NavigationPanel.ATSAUTOMATION11Info);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.DetailsInBatch' at Center.", repo.MainWindow.NavigationPanel.DetailsInBatchInfo, new RecordItemIndex(24));
            repo.MainWindow.NavigationPanel.DetailsInBatch.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 30s to not exist. Associated repository item: 'MainWindow.BusyOverlay'", repo.MainWindow.BusyOverlayInfo, new ActionTimeout(30000), new RecordItemIndex(25));
            repo.MainWindow.BusyOverlayInfo.WaitForNotExists(30000);
            
            varUserReferenceNumber = SPCollection.ReadRequisitionFile("\\\\ca01a9001\\pgmis\\Deployment_DEV\\Ranorex\\Supplier_Portal\\RequisitionNumber\\req.txt");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Set value", "Setting attribute Text to '$varUserReferenceNumber' on item 'MainWindow.NavigationPanel.UserReferenceFilter'.", repo.MainWindow.NavigationPanel.UserReferenceFilterInfo, new RecordItemIndex(27));
            repo.MainWindow.NavigationPanel.UserReferenceFilter.Element.SetAttributeValue("Text", varUserReferenceNumber);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Get Value", "Getting attribute 'Text' from item 'MainWindow.NavigationPanel.ATSRequiredDateRow0' and assigning its value to variable 'varATSReqDate'.", repo.MainWindow.NavigationPanel.ATSRequiredDateRow0Info, new RecordItemIndex(28));
            varATSReqDate = repo.MainWindow.NavigationPanel.ATSRequiredDateRow0.Element.GetAttributeValueText("Text");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Get Value", "Getting attribute 'Text' from item 'MainWindow.NavigationPanel.CellUserReference0' and assigning its value to variable 'varUserReferenceNumberRow0'.", repo.MainWindow.NavigationPanel.CellUserReference0Info, new RecordItemIndex(29));
            varUserReferenceNumberRow0 = repo.MainWindow.NavigationPanel.CellUserReference0.Element.GetAttributeValueText("Text");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "User", varUserReferenceNumberRow0, new RecordItemIndex(30));
            
            Report.Log(ReportLevel.Info, "Get Value", "Getting attribute 'Text' from item 'MainWindow.NavigationPanel.DetailRow0' and assigning its value to variable 'varDetailRow0'.", repo.MainWindow.NavigationPanel.DetailRow0Info, new RecordItemIndex(31));
            varDetailRow0 = repo.MainWindow.NavigationPanel.DetailRow0.Element.GetAttributeValueText("Text");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "User", varDetailRow0, new RecordItemIndex(32));
            
            Report.Log(ReportLevel.Info, "Get Value", "Getting attribute 'Text' from item 'MainWindow.NavigationPanel.QuantityRow0' and assigning its value to variable 'varQtyRow0'.", repo.MainWindow.NavigationPanel.QuantityRow0Info, new RecordItemIndex(33));
            varQtyRow0 = repo.MainWindow.NavigationPanel.QuantityRow0.Element.GetAttributeValueText("Text");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "User", varQtyRow0, new RecordItemIndex(34));
            
            varDateIncorrect = SPCollection.GetDateTimePlusXDays(ValueConverter.ArgumentFromString<int>("days", "30"));
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "User", varDateIncorrect, new RecordItemIndex(36));
            
            varcolumnIndex = ValueConverter.ToString(SPCollection.GetColumnIndexofHeader("Required Date"));
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "User", varcolumnIndex, new RecordItemIndex(38));
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.ReqDateFirstRowTextBox' at Center.", repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBoxInfo, new RecordItemIndex(39));
            repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBox.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{LControlKey down}{Akey}{LControlKey up}{Delete}'.", new RecordItemIndex(40));
            Keyboard.Press("{LControlKey down}{Akey}{LControlKey up}{Delete}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.Row0Cell6' at Center.", repo.MainWindow.NavigationPanel.Row0Cell6Info, new RecordItemIndex(41));
            repo.MainWindow.NavigationPanel.Row0Cell6.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.ReqDateFirstRowTextBox' at Center.", repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBoxInfo, new RecordItemIndex(42));
            repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBox.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence from variable '$varDateIncorrect'.", new RecordItemIndex(43));
            Keyboard.Press(varDateIncorrect, 500);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.Row0Cell6' at Center.", repo.MainWindow.NavigationPanel.Row0Cell6Info, new RecordItemIndex(44));
            repo.MainWindow.NavigationPanel.Row0Cell6.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text=$varDateIncorrect) on item 'MainWindow.NavigationPanel.ReqDateFirstRowTextBox'.", repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBoxInfo, new RecordItemIndex(45));
            Validate.AttributeEqual(repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBoxInfo, "Text", varDateIncorrect);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.CheckBoxItem1' at Center.", repo.MainWindow.NavigationPanel.CheckBoxItem1Info, new RecordItemIndex(46));
            repo.MainWindow.NavigationPanel.CheckBoxItem1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.AssigneeDropDownRow0' at Center.", repo.MainWindow.NavigationPanel.AssigneeDropDownRow0Info, new RecordItemIndex(47));
            repo.MainWindow.NavigationPanel.AssigneeDropDownRow0.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ATSSupplierPortalManagment.AssigneeList.ranorexrt' at Center.", repo.ATSSupplierPortalManagment.AssigneeList.ranorexrtInfo, new RecordItemIndex(48));
            repo.ATSSupplierPortalManagment.AssigneeList.ranorexrt.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.CreateRFQButton' at Center.", repo.MainWindow.NavigationPanel.CreateRFQButtonInfo, new RecordItemIndex(49));
            repo.MainWindow.NavigationPanel.CreateRFQButton.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 1m to exist. Associated repository item: 'NotificationsWindow.ErrorShippingDate'", repo.NotificationsWindow.ErrorShippingDateInfo, new ActionTimeout(60000), new RecordItemIndex(50));
            repo.NotificationsWindow.ErrorShippingDateInfo.WaitForExists(60000);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating Exists on item 'NotificationsWindow.ErrorShippingDate'.", repo.NotificationsWindow.ErrorShippingDateInfo, new RecordItemIndex(51));
            Validate.Exists(repo.NotificationsWindow.ErrorShippingDateInfo);
            Delay.Milliseconds(0);
            
            Report.Screenshot(ReportLevel.Info, "User", "", repo.MainWindow.Self, false, new RecordItemIndex(52));
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'NotificationsWindow.CloseButton' at Center.", repo.NotificationsWindow.CloseButtonInfo, new RecordItemIndex(53));
            repo.NotificationsWindow.CloseButton.Click();
            Delay.Milliseconds(200);
            
            varDate = SPCollection.GetDateTimePlusXDays(ValueConverter.ArgumentFromString<int>("days", "365"));
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "User", varDate, new RecordItemIndex(55));
            
            varcolumnIndex = ValueConverter.ToString(SPCollection.GetColumnIndexofHeader("Required Date"));
            Delay.Milliseconds(0);
            
            //Report.Log(ReportLevel.Info, "Set value", "Setting attribute Text to '' on item 'MainWindow.NavigationPanel.ReqDateFirstRowTextBox'.", repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBoxInfo, new RecordItemIndex(57));
            //repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBox.Element.SetAttributeValue("Text", "");
            //Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.ReqDateFirstRowTextBox' at Center.", repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBoxInfo, new RecordItemIndex(58));
            repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBox.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{LControlKey down}{Akey}{LControlKey up}{Delete}'.", new RecordItemIndex(59));
            Keyboard.Press("{LControlKey down}{Akey}{LControlKey up}{Delete}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.Row0Cell6' at Center.", repo.MainWindow.NavigationPanel.Row0Cell6Info, new RecordItemIndex(60));
            repo.MainWindow.NavigationPanel.Row0Cell6.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.ReqDateFirstRowTextBox' at Center.", repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBoxInfo, new RecordItemIndex(61));
            repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBox.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence from variable '$varDate'.", new RecordItemIndex(62));
            Keyboard.Press(varDate);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.Row0Cell6' at Center.", repo.MainWindow.NavigationPanel.Row0Cell6Info, new RecordItemIndex(63));
            repo.MainWindow.NavigationPanel.Row0Cell6.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text=$varDate) on item 'MainWindow.NavigationPanel.ReqDateFirstRowTextBox'.", repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBoxInfo, new RecordItemIndex(64));
            Validate.AttributeEqual(repo.MainWindow.NavigationPanel.ReqDateFirstRowTextBoxInfo, "Text", varDate);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 1s.", new RecordItemIndex(65));
            Delay.Duration(1000, false);
            
            Report.Log(ReportLevel.Info, "Get Value", "Getting attribute 'Text' from item 'MainWindow.NavigationPanel.ATSRequestDateFirstRowTextBox' and assigning its value to variable 'varATSReqDate'.", repo.MainWindow.NavigationPanel.ATSRequestDateFirstRowTextBoxInfo, new RecordItemIndex(66));
            varATSReqDate = repo.MainWindow.NavigationPanel.ATSRequestDateFirstRowTextBox.Element.GetAttributeValueText("Text");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainWindow.NavigationPanel.CreateRFQButton' at Center.", repo.MainWindow.NavigationPanel.CreateRFQButtonInfo, new RecordItemIndex(67));
            repo.MainWindow.NavigationPanel.CreateRFQButton.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 1m to exist. Associated repository item: 'NotificationsWindow.SuccessMessage'", repo.NotificationsWindow.SuccessMessageInfo, new ActionTimeout(60000), new RecordItemIndex(68));
            repo.NotificationsWindow.SuccessMessageInfo.WaitForExists(60000);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating Exists on item 'NotificationsWindow.SuccessMessage'.", repo.NotificationsWindow.SuccessMessageInfo, new RecordItemIndex(69));
            Validate.Exists(repo.NotificationsWindow.SuccessMessageInfo);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Get Value", "Getting attribute 'Text' from item 'NotificationsWindow.SuccessMessage' and assigning the part of its value captured by '\\d+' to variable 'varRFQNumber'.", repo.NotificationsWindow.SuccessMessageInfo, new RecordItemIndex(70));
            varRFQNumber = repo.NotificationsWindow.SuccessMessage.Element.GetAttributeValueText("Text", new Regex("\\d+"));
            Delay.Milliseconds(0);
            
            Report.Screenshot(ReportLevel.Info, "User", "", repo.NotificationsWindow.SuccessMessage, false, new RecordItemIndex(71));
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 1s.", new RecordItemIndex(72));
            Delay.Duration(1000, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'NotificationsWindow.CloseButton' at Center.", repo.NotificationsWindow.CloseButtonInfo, new RecordItemIndex(73));
            repo.NotificationsWindow.CloseButton.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 3s.", new RecordItemIndex(74));
            Delay.Duration(3000, false);
            
            GetOrderNumber("192257", varDetailRow0, varUserReferenceNumberRow0, varQtyRow0);
            Delay.Milliseconds(0);
            
            //GetOrderNumber("192257", "100002108", "90000806", "1");
            //Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
