using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using AssetProject.Models;

namespace AssetProject.Reports
{
	public partial class rptOutcomingStoreHistory : DevExpress.XtraReports.UI.XtraReport
	{
        public Tenant TenantObj { get; set; }

        public rptOutcomingStoreHistory(Tenant tenant)
        {
            InitializeComponent();
            TenantObj = tenant;
        }
        public void LoadTalent()
        {
            if (TenantObj != null)
            {
                txt_Address.Text = TenantObj.Address;
                Text_CN.Text = TenantObj.CompanyName;
                website.Text = TenantObj.Website;
                phone.Text = TenantObj.Phone;
                email.Text = TenantObj.Email;
                pictureBox1.ImageUrl = "https://localhost:44311/" + TenantObj.Logo;

            }
        }

        private void rptOutcomingStoreHistory_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            LoadTalent();
        }
    }
}
