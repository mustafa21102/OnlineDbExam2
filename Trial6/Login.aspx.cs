using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Trial6
{
    public partial class Login : System.Web.UI.Page
    {
        // Connecting to the Entity Framework Context
        // NOTE: Ensure 'OnlineExamDBEntities' matches the name you gave in the Wizard!
        OnlineExamDBEntities db = new OnlineExamDBEntities();

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string u = txtUsername.Text;
            string p = txtPassword.Text;

            // Simple LINQ query to find the user
            var user = db.Users.FirstOrDefault(x => x.Username == u && x.Password == p);

            if (user != null)
            {
                // Store data in Session (Old School way)
                Session["UserId"] = user.UserId;
                Session["Username"] = user.Username;
                Session["Role"] = user.Role;

                if (user.Role == "Student")
                {
                    Response.Redirect("~/Student/Dashboard.aspx");
                }
                else
                {
                    // For now, Instructors just reload or go to a placeholder
                    Response.Redirect("~/Login.aspx");
                }
            }
            else
            {
                lblError.Text = "Invalid credentials";
            }
        }
    }
}