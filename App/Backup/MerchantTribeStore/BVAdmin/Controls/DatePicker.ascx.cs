using System;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_DatePicker : System.Web.UI.UserControl
    {

        public event EventHandler<BVModuleEventArgs> DateChanged;

        public System.DateTime SelectedDate
        {
            get
            {
                System.DateTime val;
                if (System.DateTime.TryParse(DateTextBox.Text, out val))
                {
                    return val;
                }
                else
                {
                    return System.DateTime.MinValue;
                }
            }
            set { DateTextBox.Text = value.ToShortDateString(); }
        }

        public string Text
        {
            get { return DateTextBox.Text; }
            set { DateTextBox.Text = value; }
        }

        public string RequiredErrorMessage
        {
            get { return DateRequiredValidator.ErrorMessage; }
            set { DateRequiredValidator.ErrorMessage = value; }
        }

        public string InvalidFormatErrorMessage
        {
            get { return DateCustomValidator.ErrorMessage; }
            set { DateCustomValidator.ErrorMessage = value; }
        }

        public int TabIndex
        {
            get { return DateTextBox.TabIndex; }
            set { DateTextBox.TabIndex = (short)value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                if (DateTextBox.Text == string.Empty)
                {
                    DateTextBox.Text = System.DateTime.Now.ToShortDateString();
                }
            }
        }

        protected void CalendarShowImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Calendar.Visible = (!Calendar.Visible);
            //Calendar.UpdateAfterCallBack = true;
        }

        protected void Calendar_SelectionChanged(object sender, System.EventArgs e)
        {
            DateTextBox.Text = Calendar.SelectedDate.ToString();
            //DateTextBox.UpdateAfterCallBack = true;
            Calendar.Visible = false;
            if (DateChanged != null)
            {
                DateChanged(this, new BVModuleEventArgs());
            }
        }

        protected void DateCustomValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            DateTime temp;
            if (System.DateTime.TryParse(args.Value,
                                        System.Threading.Thread.CurrentThread.CurrentUICulture,
                                        System.Globalization.DateTimeStyles.None, out temp))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }
    }
}