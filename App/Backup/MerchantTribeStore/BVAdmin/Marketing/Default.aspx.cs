using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Marketing;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_Discounts : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Offers";
            this.CurrentTab = AdminTabType.Marketing;
            ValidateCurrentUserHasPermission(SystemPermissions.MarketingView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //if (!Page.IsPostBack)
            //{
            //    BindOffersGridView();
            //    BindOfferTypeDropDownList();
            //    InitializeErrorMessages();
            //}
        }

        //protected void InitializeErrorMessages()
        //{
        //    MessageBox1.ClearMessage();
        //    MessageBox2.ClearMessage();
        //}

        //protected BusinessEntityList<Offer> GetCurrentOffers()
        //{
        //    BusinessEntityList<Offer> offers = Offer.GetAllOffers();
        //    int index = 0;
        //    while (index < offers.Count)
        //    {
        //        if (offers[index].Priority != LastPriority)
        //        {
        //            LastPriority = offers[index].Priority;
        //            Offer offer = new Offer();
        //            offer.Name = "Separator";
        //            offer.Priority = LastPriority;
        //            offers.Insert(index, offer);
        //            index += 1;
        //        }
        //        index += 1;
        //    }
        //    return offers;
        //}

        //protected void BindOffersGridView()
        //{
        //    OffersGridView.DataSource = GetCurrentOffers();
        //    OffersGridView.DataKeyNames = new string[] { "bvin" };
        //    OffersGridView.DataBind();
        //}

        //protected void BindOfferTypeDropDownList()
        //{
        //    OfferTypeDropDownList.DataSource = Offer.GetOfferTypes();
        //    OfferTypeDropDownList.DataBind();
        //}

        //protected void OffersGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        //{
        //    if (Offer.Delete(OffersGridView.DataKeys[e.RowIndex].Value.ToString()))
        //    {
        //        MessageBox1.ShowOk("Offer successfully deleted from the database.");
        //        BindOffersGridView();
        //    }
        //    else
        //    {
        //        MessageBox1.ShowError("An error occurred while deleting the offer from the database.");
        //    }
        //}

        //protected void NewOfferImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/BVAdmin/Marketing/OffersEdit.aspx?type=" + Server.UrlEncode(OfferTypeDropDownList.SelectedValue));
        //}

        //protected void OffersGridView_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        //{
        //    Response.Redirect("~/BVAdmin/Marketing/OffersEdit.aspx?id=" + Server.UrlEncode(OffersGridView.DataKeys[e.NewEditIndex].Value.ToString()));
        //}

        //protected void OffersGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if (e.Row.DataItem != null)
        //        {
        //            //Dim offersTable As Table = CType(OffersGridView.Controls(0), Table)
        //            Offer offer = (Offer)e.Row.DataItem;
        //            if (offer.Name == "Separator")
        //            {
        //                TableCell cell = new TableCell();
        //                cell.ColumnSpan = e.Row.Cells.Count;
        //                cell.Text = "Priority: " + offer.Priority.ToString();
        //                e.Row.Cells.Clear();
        //                e.Row.Cells.Add(cell);
        //                e.Row.RowType = DataControlRowType.Separator;
        //                e.Row.ControlStyle.CssClass = "separator";
        //            }
        //            else
        //            {

        //                ImageButton enableImageButton = (ImageButton)e.Row.Cells[6].FindControl("EnableImageButton");
        //                ImageButton disableImageButton = (ImageButton)e.Row.Cells[6].FindControl("DisableImageButton");
        //                if (offer.Enabled)
        //                {
        //                    enableImageButton.Visible = false;
        //                    disableImageButton.Visible = true;
        //                    disableImageButton.CommandArgument = offer.Bvin;
        //                }
        //                else
        //                {
        //                    enableImageButton.Visible = true;
        //                    enableImageButton.CommandArgument = offer.Bvin;
        //                    disableImageButton.Visible = false;
        //                }

        //                ((ImageButton)e.Row.Cells[8].FindControl("MoveUpImageButton")).CommandArgument = offer.Bvin;
        //                ((ImageButton)e.Row.Cells[8].FindControl("MoveDownImageButton")).CommandArgument = offer.Bvin;

        //                Image statusImage = (Image)e.Row.Cells[0].FindControl("StatusImage");
        //                //TODO: put in status icon urls
        //                if (offer.IsExpired)
        //                {
        //                    statusImage.ImageUrl = "~/BVAdmin/Images/SalesStatus/Expired.gif";
        //                    statusImage.AlternateText = "Expired";
        //                    statusImage.Attributes.Add("title", "Expired");
        //                }
        //                else if (offer.IsPending)
        //                {
        //                    statusImage.ImageUrl = "~/BVAdmin/Images/SalesStatus/Pending.gif";
        //                    statusImage.AlternateText = "Pending";
        //                    statusImage.Attributes.Add("title", "Pending");
        //                }
        //                else if (offer.Enabled)
        //                {
        //                    statusImage.ImageUrl = "~/BVAdmin/Images/SalesStatus/Enabled.gif";
        //                    statusImage.AlternateText = "Enabled";
        //                    statusImage.Attributes.Add("title", "Enabled");
        //                }
        //                else if (!offer.Enabled)
        //                {
        //                    statusImage.ImageUrl = "~/BVAdmin/Images/SalesStatus/Disabled.gif";
        //                    statusImage.AlternateText = "Disabled";
        //                    statusImage.Attributes.Add("title", "Disabled");
        //                }
        //            }
        //        }
        //    }
        //}

        //protected void OffersGridView_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Enable")
        //    {
        //        Offer offer = Offer.FindByBvin((string)e.CommandArgument);
        //        offer.Enabled = true;
        //        if (!offer.Commit())
        //        {
        //            MessageBox1.ShowError("An error occurred while trying to save the offer to the database.");
        //        }
        //        BindOffersGridView();
        //    }
        //    else if (e.CommandName == "Disable")
        //    {
        //        Offer offer = Offer.FindByBvin((string)e.CommandArgument);
        //        offer.Enabled = false;
        //        if (!offer.Commit())
        //        {
        //            MessageBox1.ShowError("An error occurred while trying to save the offer to the database.");
        //        }
        //        BindOffersGridView();
        //    }
        //    else if ((e.CommandName == "MoveUp"))
        //    {
        //        BusinessEntityList<Offer> offers = GetCurrentOffers();
        //        if (offers.MoveDown((string)e.CommandArgument))
        //        {
        //            if (!offers.Commit())
        //            {
        //                MessageBox1.ShowError("An error occurred while trying to update the order.");
        //            }
        //        }
        //        BindOffersGridView();
        //    }
        //    else if (e.CommandName == "MoveDown")
        //    {
        //        BusinessEntityList<Offer> offers = GetCurrentOffers();
        //        if (offers.MoveUp((string)e.CommandArgument))
        //        {
        //            if (!offers.Commit())
        //            {
        //                MessageBox1.ShowError("An error occurred while trying to update the order.");
        //            }
        //        }
        //        BindOffersGridView();
        //    }
        //}
    }

}