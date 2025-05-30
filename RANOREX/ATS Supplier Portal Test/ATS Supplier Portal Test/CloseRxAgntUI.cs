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
    ///The CloseRxAgntUI recording.
    /// </summary>
    [TestModule("a5ce6a58-7cd3-4942-aa98-18a8f5ca020d", ModuleType.Recording, 1)]
    public partial class CloseRxAgntUI : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ATS_Supplier_Portal_TestRepository repository.
        /// </summary>
        public static ATS_Supplier_Portal_TestRepository repo = ATS_Supplier_Portal_TestRepository.Instance;

        static CloseRxAgntUI instance = new CloseRxAgntUI();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CloseRxAgntUI()
        {
            varFound = "False";
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static CloseRxAgntUI Instance
        {
            get { return instance; }
        }

#region Variables

        string _varFound;

        /// <summary>
        /// Gets or sets the value of variable varFound.
        /// </summary>
        [TestVariable("c5873a17-e106-4b57-b5d8-5864c3a32068")]
        public string varFound
        {
            get { return _varFound; }
            set { _varFound = value; }
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

            Report.Screenshot(ReportLevel.Info, "User", "", null, false, new RecordItemIndex(0));
            
            SPCollection.GetSS();
            Delay.Milliseconds(0);
            
            varFound = ValueConverter.ToString(SPCollection.RunOCRCogServ("Agent"));
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 1s.", new RecordItemIndex(3));
            Delay.Duration(1000, false);
            
            SPCollection.CloseWithAltF4(ValueConverter.ArgumentFromString<bool>("val", varFound));
            Delay.Milliseconds(0);
            
            Report.Screenshot(ReportLevel.Info, "User", "", null, false, new RecordItemIndex(5));
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
