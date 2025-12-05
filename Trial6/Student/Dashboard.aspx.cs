using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Trial6.Student
{
    public partial class Dashboard : System.Web.UI.Page
    {
        OnlineExamDBEntities db = new OnlineExamDBEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Student")
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadExams();
            }
        }

        private void LoadExams()
        {
            // Fetch Active exams
            // NOTE: In V3 schema, IsActive is a bit (bool).
            var exams = db.Exams.Where(x => x.IsActive == true).ToList();

            if (exams.Count > 0)
            {
                rptExams.DataSource = exams;
                rptExams.DataBind();
            }
            else
            {
                lblNoExams.Visible = true;
            }
        }
    }
}