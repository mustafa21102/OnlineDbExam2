using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Trial6   // <-- match your project name here
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Check if user is logged in
            if (Session["UserId"] != null)
            {
                // User is logged in - Update UI
                if (Session["Username"] != null)
                {
                    lblUsername.Text = "Hello, " + Session["Username"].ToString();
                }

                // Show Student Link only for students
                if (Session["Role"]?.ToString() == "Student")
                {
                    lnkStudent.Visible = true;
                }
            }
            else
            {
                // 2. User is NOT logged in.
                // WE MUST CHECK: Are we already on the login page?

                string currentUrl = Request.Url.AbsolutePath.ToLower();

                // If the URL does NOT contain "login.aspx", then kick them out.
                // If it DOES contain "login.aspx", do nothing (let the page load).
                if (!currentUrl.Contains("login.aspx"))
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}
