using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Trial6;

namespace Trial6.Student
{
    public partial class TakeExam : System.Web.UI.Page
    {
        OnlineExamDBEntities db = new OnlineExamDBEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Student")
                Response.Redirect("~/Login.aspx");

            if (!IsPostBack)
            {
                int examId = 0;
                if (int.TryParse(Request.QueryString["id"], out examId))
                {
                    LoadExam(examId);
                }
                else
                {
                    Response.Redirect("Dashboard.aspx");
                }
            }
        }

        private void LoadExam(int examId)
        {
            // 1. Get Exam Details
            var exam = db.Exams.FirstOrDefault(x => x.ExamId == examId);
            if (exam == null) return;

            lblExamTitle.Text = exam.Title;

            // 2. Manage Attempt (Start or Resume)
            int userId = (int)Session["UserId"];
            var attempt = db.StudentAttempts.FirstOrDefault(a => a.ExamId == examId && a.UserId == userId);

            if (attempt == null)
            {
                // Start New Attempt
                attempt = new StudentAttempt
                {
                    ExamId = examId,
                    UserId = userId,
                    StartTime = DateTime.Now,
                    Status = "InProgress"
                };
                db.StudentAttempts.Add(attempt);
                db.SaveChanges();
            }
            else if (attempt.Status == "Completed")
            {
                // If done, go back to dashboard
                Response.Redirect("Dashboard.aspx");
            }

            // 3. Calculate Remaining Seconds
            // Duration is in Minutes, so convert to seconds
            double maxSeconds = exam.DurationMinutes * 60;
            double elapsedSeconds = (DateTime.Now - attempt.StartTime).TotalSeconds;
            double remaining = maxSeconds - elapsedSeconds;

            if (remaining <= 0)
            {
                // Time expired logic here (Force submit or kick out)
                SubmitExam(attempt.AttemptId);
                return;
            }

            // Inject the calculated time into the JavaScript function
            ClientScript.RegisterStartupScript(this.GetType(), "timer", $"startTimer({(int)remaining});", true);

            // 4. Load Questions (via the Pool table)
            // Join ExamQuestionPool with Questions table
            var questionIds = db.ExamQuestionPools.Where(p => p.ExamId == examId).Select(p => p.QuestionId).ToList();
            var questions = db.Questions.Where(q => questionIds.Contains(q.QuestionId)).ToList();

            rptQuestions.DataSource = questions;
            rptQuestions.DataBind();
        }

        // This runs for EVERY question in the list to load its options
        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Find the RadioButtonList in the HTML
                RadioButtonList rbl = (RadioButtonList)e.Item.FindControl("rblOptions");
                HiddenField hf = (HiddenField)e.Item.FindControl("hfQuestionId");

                int qId = int.Parse(hf.Value);

                // Load Options for this specific Question
                var options = db.QuestionOptions.Where(o => o.QuestionId == qId).ToList();

                rbl.DataSource = options;
                rbl.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int examId = int.Parse(Request.QueryString["id"]);
            int userId = (int)Session["UserId"];
            var attempt = db.StudentAttempts.FirstOrDefault(a => a.ExamId == examId && a.UserId == userId);

            if (attempt != null)
            {
                SubmitExam(attempt.AttemptId);
            }
        }

        private void SubmitExam(int attemptId)
        {
            var attempt = db.StudentAttempts.Find(attemptId);
            if (attempt == null) return;

            decimal correctCount = 0;
            int totalQuestions = 0;

            // Loop through the Repeater items to see what user picked
            foreach (RepeaterItem item in rptQuestions.Items)
            {
                RadioButtonList rbl = (RadioButtonList)item.FindControl("rblOptions");

                if (rbl.SelectedValue != "")
                {
                    int selectedOptionId = int.Parse(rbl.SelectedValue);

                    // Check database if this option is correct
                    var option = db.QuestionOptions.Find(selectedOptionId);
                    if (option != null && option.IsCorrect == true)
                    {
                        correctCount++;
                    }
                }
                totalQuestions++;
            }

            // Calculate Score
            decimal score = totalQuestions > 0 ? (correctCount / totalQuestions) * 100 : 0;

            // Save Result
            attempt.TotalScore = score;
            attempt.EndTime = DateTime.Now;
            attempt.Status = "Completed";
            db.SaveChanges();

            // Redirect to a simple result page (or back to dashboard)
            Response.Redirect("Dashboard.aspx");
        }
    }
}