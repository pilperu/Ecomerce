using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Marketing;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_Marketing_OffersEdit : BaseAdminPage
    {

        //private OfferTemplate OfferEditor;

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            //InitializeErrorMessages();
            //if (!Page.IsPostBack)
            //{
            //    if (Request.QueryString["id"] != null)
            //    {
            //        Offer offer = Offer.FindByBvin(Request.QueryString["id"]);
            //        if (offer != null)
            //        {
            //            InitializeBaseForm(offer);
            //            LoadOfferEditor(offer, true);
            //        }
            //        else
            //        {
            //            Response.Redirect("~/BVAdmin/Marketing/Default.aspx");
            //        }
            //    }
            //    else if (Request.QueryString["type"] != null)
            //    {
            //        Offer offer = new Offer();
            //        offer.OfferType = Request.QueryString["type"];
            //        InitializeBaseForm(offer);
            //        LoadOfferEditor(offer, true);
            //    }
            //    else
            //    {
            //        Response.Redirect("~/BVAdmin/Marketing/Default.aspx");
            //    }
            //}
            //else
            //{
            //    if (ViewState["Offer"] != null)
            //    {
            //        LoadOfferEditor((Offer)ViewState["Offer"], false);
            //    }
            //}
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Offers Edit";
            this.CurrentTab = AdminTabType.Marketing;
            ValidateCurrentUserHasPermission(SystemPermissions.MarketingView);
        }

        //protected void InitializeErrorMessages()
        //{
        //    MessageBox1.ClearMessage();
        //    MessageBox2.ClearMessage();
        //    StartDatePicker.RequiredErrorMessage = "Start date is required.";
        //    EndDatePicker.RequiredErrorMessage = "End date is required.";
        //    StartDatePicker.InvalidFormatErrorMessage = "Start date format is invalid. Please enter a valid date.";
        //    EndDatePicker.InvalidFormatErrorMessage = "End date format is invalid. Please enter a valid date.";
        //}

        //protected void LoadOfferEditor(Offer offer, bool force)
        //{
        //    OfferEditor = (OfferTemplate)ModuleController.LoadOfferEditor(offer.OfferType, this);
        //    OfferEditor.ID = "OfferEditor";
        //    OfferEditor.BlockId = offer.Bvin;
        //    OfferEditor.Offer = offer;
        //    AddControlToEditPanel(OfferEditor);
        //    ViewState["Offer"] = offer;
        //    OfferEditor.Initialize(force);
        //}

        //protected void AddControlToEditPanel(System.Web.UI.UserControl control)
        //{
        //    EditPlaceHolder.Controls.Clear();
        //    EditPlaceHolder.Controls.Add(control);
        //}

        //protected void SaveImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    if (Page.IsValid)
        //    {
        //        Offer offer = (Offer)ViewState["Offer"];
        //        GetFormValues(offer);
        //        if (offer.Commit())
        //        {
        //            OfferEditor.Save();
        //            Response.Redirect("~/BVAdmin/Marketing/default.aspx");
        //        }
        //        else
        //        {
        //            MessageBox1.ShowError("An error occurred while trying to save the offer to the database.");
        //        }
        //    }
        //}

        //protected void GetFormValues(Offer offer)
        //{
        //    offer.Name = OfferNameTextBox.Text;
        //    offer.StartDate = StartDatePicker.SelectedDate;
        //    offer.EndDate = EndDatePicker.SelectedDate;
        //    offer.RequiresCouponCode = RequiresCouponCodeCheckBox.Checked;
        //    //offer.GenerateUniquePromotionalCodes = UniquePromotionalCodesCheckBox.Checked
        //    offer.PromotionalCode = PromotionCodeTextBox.Text;
        //    if (UnlimitedRadioButton.Checked)
        //    {
        //        offer.UseType = OfferUseTypes.Unlimited;
        //        offer.UseTimes = -1;
        //    }
        //    else if (PerStoreRadioButton.Checked)
        //    {
        //        offer.UseType = OfferUseTypes.PerStore;
        //        offer.UseTimes = int.Parse(UsePerStoreTextBox.Text);
        //    }
        //    else if (PerCustomerRadioButton.Checked)
        //    {
        //        offer.UseType = OfferUseTypes.PerCustomer;
        //        offer.UseTimes = int.Parse(UsePerPersonTextBox.Text);
        //    }

        //    offer.CantBeCombined = PromotionCodeCantBeCombinedCheckBox.Checked;
        //}

        //protected void CancelImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/BVAdmin/Marketing/Default.aspx");
        //}

        //protected void InitializeBaseForm(Offer offer)
        //{
        //    OfferNameTextBox.Text = offer.Name;
        //    StartDatePicker.Text = offer.StartDate.ToShortDateString();
        //    EndDatePicker.Text = offer.EndDate.ToShortDateString();
        //    RequiresCouponCodeCheckBox.Checked = offer.RequiresCouponCode;
        //    //UniquePromotionalCodesCheckBox.Checked = offer.GenerateUniquePromotionalCodes
        //    PromotionCodeTextBox.Text = offer.PromotionalCode;
        //    if (offer.UseType == OfferUseTypes.Unlimited)
        //    {
        //        UnlimitedRadioButton.Checked = true;
        //        PerStoreRadioButton.Checked = false;
        //        PerCustomerRadioButton.Checked = false;
        //        UsePerPersonTextBox.Text = "0";
        //        UsePerStoreTextBox.Text = "0";
        //    }
        //    else if (offer.UseType == OfferUseTypes.PerStore)
        //    {
        //        UnlimitedRadioButton.Checked = false;
        //        PerStoreRadioButton.Checked = true;
        //        PerCustomerRadioButton.Checked = false;
        //        UsePerPersonTextBox.Text = "0";
        //        UsePerStoreTextBox.Text = ((int)offer.UseTimes).ToString();
        //    }
        //    else if (offer.UseType == OfferUseTypes.PerCustomer)
        //    {
        //        UnlimitedRadioButton.Checked = false;
        //        PerStoreRadioButton.Checked = false;
        //        PerCustomerRadioButton.Checked = true;
        //        UsePerPersonTextBox.Text = ((int)offer.UseTimes).ToString();
        //        UsePerStoreTextBox.Text = "0";
        //    }
        //    PromotionCodeCantBeCombinedCheckBox.Checked = offer.CantBeCombined;
        //}

        //protected void PerCustomerCustomValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        //{
        //    if (PerCustomerRadioButton.Checked)
        //    {
        //        int val;
        //        if (int.TryParse(UsePerPersonTextBox.Text, out val))
        //        {
        //            if (val <= 0)
        //            {
        //                args.IsValid = false;
        //            }
        //        }
        //        else
        //        {
        //            args.IsValid = false;
        //        }
        //    }
        //}

        //protected void PerStoreCustomValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        //{
        //    if (PerStoreRadioButton.Checked)
        //    {
        //        int val;
        //        if (int.TryParse(UsePerStoreTextBox.Text, out val))
        //        {
        //            if (val <= 0)
        //            {
        //                args.IsValid = false;
        //            }
        //        }
        //        else
        //        {
        //            args.IsValid = false;
        //        }
        //    }
        //}

        //protected void CustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        //{
        //    if (System.DateTime.Compare(StartDatePicker.SelectedDate, EndDatePicker.SelectedDate) == 1)
        //    {
        //        args.IsValid = false;
        //    }
        //}
    }
}