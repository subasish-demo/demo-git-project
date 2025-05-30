﻿///////////////////////////////////////////////////////////////////////////////
//
// This file was automatically generated by RANOREX.
// Your custom recording code should go in this file.
// The designer will only add methods to this file, so your custom code won't be overwritten.
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
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace ATS_Supplier_Portal_Test
{
    public partial class LoginJDEPROD
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        public void ShowCarosel()
        {
        	Delay.Seconds(2.0);
        	if(repo.JDEPath.CaroBar.Title == "Show Carousel")
        	{
        		Report.Info("Carousel not shown.  Expanding...");
        		repo.JDEPath.CaroBar.Click();
        	}
        	else
        	{
        		Report.Info("Carousel already expanded");
        	}
        	Delay.Seconds(2.0);
        }

        public void HideCarosel()
        {
        	Delay.Seconds(2.0);
            if(repo.JDEPath.CaroBar.Title == "Hide Carousel")
        	{
        		Report.Info("Hiding Carousel...");
        		repo.JDEPath.CaroBar.Click();
        	}
        	else
        	{
        		Report.Info("Carousel already hidden.");
        	}
        	Delay.Seconds(2.0);
        }

    }
}
