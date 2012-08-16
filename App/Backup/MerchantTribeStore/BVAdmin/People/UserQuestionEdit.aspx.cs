using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_People_UserQuestionEdit : BaseAdminPage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.Page.Form.DefaultButton = CancelImageButton.UniqueID;
                if (Request.QueryString["id"] != null)
                {
                    UserQuestion question = MTApp.MembershipServices.UserQuestions.Find(Request.QueryString["id"]);
                    ViewState["Question"] = question;
                    if (question.Type == UserQuestionType.MultipleChoice)
                    {
                        QuestionTextBox.Text = question.Values[question.Values.Count - 1].Value;
                        question.Values.RemoveAt(question.Values.Count - 1);
                    }
                }
                else
                {
                    ViewState["Question"] = new UserQuestion();
                }

                InitializeInput();
            }
            else
            {
                UserQuestion question = (UserQuestion)ViewState["Question"];
                if (QuestionTypeRadioButtonList.SelectedIndex == (int)UserQuestionType.MultipleChoice)
                {
                    foreach (GridViewRow row in ValuesGridView.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            question.Values[row.RowIndex].Value = ((TextBox)row.FindControl("ValueTextBox")).Text;
                        }
                    }
                }
                ViewState["Question"] = question;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (QuestionTypeRadioButtonList.SelectedIndex == (int)UserQuestionType.MultipleChoice)
            {
                UserQuestion question = (UserQuestion)ViewState["Question"];
                InitializeGrid(question);
            }
        }

        protected void InitializeGrid(UserQuestion question)
        {
            foreach (GridViewRow row in ValuesGridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    ((TextBox)row.FindControl("ValueTextBox")).Text = question.Values[row.RowIndex].Value;
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "User Signup Config";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(SystemPermissions.PeopleView);
        }

        protected void InitializeInput()
        {
            UserQuestion question = (UserQuestion)ViewState["Question"];
            QuestionTypeRadioButtonList.SelectedIndex = (int)question.Type;
            NameTextBox.Text = question.Name;
            if (question.Type == UserQuestionType.FreeAnswer)
            {
                MultipleChoicePanel.Visible = false;
                if (question.Values.Count == 0)
                {
                    UserQuestionOption questionOption = new UserQuestionOption();
                    questionOption.Bvin = System.Guid.NewGuid().ToString();
                    question.Values.Add(questionOption);
                }
                QuestionTextBox.Text = question.Values[0].Value;
            }
            else if (question.Type == UserQuestionType.MultipleChoice)
            {
                MultipleChoicePanel.Visible = true;
                QuestionTypeRadioButtonList.SelectedIndex = (int)question.Type;
                BindQuestionOptionsGrid(question);
            }
        }

        protected void QuestionTypeRadioButtonList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UserQuestion question = (UserQuestion)ViewState["Question"];
            question.Values.Clear();
            if (QuestionTypeRadioButtonList.SelectedIndex == (int)UserQuestionType.FreeAnswer)
            {
                MultipleChoicePanel.Visible = false;
            }
            else if (QuestionTypeRadioButtonList.SelectedIndex == (int)UserQuestionType.MultipleChoice)
            {
                MultipleChoicePanel.Visible = true;
            }
            ViewState["Question"] = question;
        }

        protected void NewOptionImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            UserQuestion question = (UserQuestion)ViewState["Question"];
            UserQuestionOption questionOption = new UserQuestionOption();
            questionOption.Bvin = System.Guid.NewGuid().ToString();
            question.Values.Add(questionOption);
            BindQuestionOptionsGrid(question);
            ViewState["Question"] = question;
        }

        protected void BindQuestionOptionsGrid(UserQuestion question)
        {
            ValuesGridView.DataSource = question.Values;
            ValuesGridView.DataKeyNames = new string[] { "bvin" };
            ValuesGridView.DataBind();
        }

        protected void SaveImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            UserQuestion question = (UserQuestion)ViewState["Question"];
            question.Type = (UserQuestionType)QuestionTypeRadioButtonList.SelectedIndex;
            question.Name = NameTextBox.Text;
            if (question.Type == UserQuestionType.FreeAnswer)
            {
                question.Values.Clear();
                UserQuestionOption item = new UserQuestionOption(QuestionTextBox.Text);
                question.Values.Add(item);
            }
            else if (question.Type == UserQuestionType.MultipleChoice)
            {
                foreach (GridViewRow row in ValuesGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        question.Values[row.RowIndex].Value = ((TextBox)row.FindControl("ValueTextBox")).Text;
                    }
                }
                question.Values.Add(new UserQuestionOption(QuestionTextBox.Text));
            }

            if (question.Bvin == string.Empty)
            {
                MTApp.MembershipServices.UserQuestions.Create(question);
            }
            else
            {
                MTApp.MembershipServices.UserQuestions.Update(question);
            }
            Response.Redirect("~/BVAdmin/People/UserSignupConfig.aspx");
        }

        protected void ValuesGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            UserQuestion question = (UserQuestion)ViewState["Question"];
            UserQuestionOption itemToRemove = null;
            string key = (string)ValuesGridView.DataKeys[e.RowIndex].Value;
            foreach (UserQuestionOption item in question.Values)
            {
                if (item.Bvin == key)
                {
                    itemToRemove = item;
                }
            }
            question.Values.Remove(itemToRemove);
            BindQuestionOptionsGrid(question);
            ViewState["Question"] = question;
        }

        protected void CancelImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/BVAdmin/People/UserSignupConfig.aspx");
        }
    }
}