using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vendor_Portal.App_Code.BLL;

namespace Vendor_Portal.BDM
{
    public partial class BDM : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 9978 || int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 9977 ||
                int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 9976 || int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 8967
                 || int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 10332
                  || int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 10333
                   || int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 10334)
            {
                costingMaster.Style.Add("display", "none");
                clientmaster.Style.Add("display", "");
                //payingEntity.Style.Add("display", "none");
                mergebilling.Style.Add("display", "none");
                // billingheader.Style.Add("display", "none");
                reports.Style.Add("display", "none");
                Li1.Style.Add("display", "none");
                Li2.Style.Add("display", "none");
            }
            else
            {
                costingMaster.Style.Add("display", "");
                clientmaster.Style.Add("display", "none");
                //payingEntity.Style.Add("display", "");
                mergebilling.Style.Add("display", "none");
                reports.Style.Add("display", "");
                allprojectbilling.Style.Add("display", "none");
                //billingheader.Style.Add("display", "");
                automanual.Style.Add("display", "none");
                cosperrecord.Style.Add("display", "none");
                Li1.Style.Add("display", "");
                Li2.Style.Add("display", "");
            }
            if (int.Parse(HttpContext.Current.User.Identity.Name.ToString()) == 9961)
            {
                allprojectbilling.Style.Add("display", "none");
                clientmaster.Style.Add("display", "none");
                automanual.Style.Add("display", "none");
                cosperrecord.Style.Add("display", "none");
                //billingheader.Style.Add("display", "none");
                mergebilling.Style.Add("display", "none");
            }

        }
    }
}