﻿using AssetProject.Models;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace AssetProject.Reports
{
    public partial class rptCustomer : DevExpress.XtraReports.UI.XtraReport
    {
        public Tenant TenantObj { get; set; }
        public rptCustomer(Tenant tenant)
        {
            InitializeComponent();
            TenantObj = tenant;
        }
        public void LoadTalent()
        {
            if (TenantObj != null)
            {
                txt_Address.Text = TenantObj.Address;
                website.Text = TenantObj.Website;
                phone.Text = TenantObj.Phone;
                email.Text = TenantObj.Email;
                pictureBox1.ImageUrl = "https://localhost:44311/" + TenantObj.Logo;
            }
        }

        private void rptCustomer_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            LoadTalent();
        }

        private void pictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //pictureBox1.ImageUrl = "https://localhost:44311/" + TenantObj.Logo;

        }
    }
}