using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Orders;
using MerchantTribe.Commerce.Orders;


namespace MerchantTribeStore.api.rest
{
    public class OrdersHandler: BaseRestHandler
    {
        public OrdersHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {
            string data = string.Empty;

            if (string.Empty == (parameters ?? string.Empty))
            {
                // List
                ApiResponse<List<OrderSnapshotDTO>> response = new ApiResponse<List<OrderSnapshotDTO>>();

                List<OrderSnapshot> results = MTApp.OrderServices.Orders.FindAll();
                List<OrderSnapshotDTO> dto = new List<OrderSnapshotDTO>();

                foreach (OrderSnapshot item in results)
                {
                    dto.Add(item.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<OrderDTO> response = new ApiResponse<OrderDTO>();
                string bvin = FirstParameter(parameters);
                Order item = MTApp.OrderServices.Orders.FindForCurrentStore(bvin);
                if (item == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that order. Check bvin and try again."));
                }
                else
                {
                    response.Content = item.ToDto();
                }
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }

            return data;
        }

        // Create or Update
        public override string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            //string bvin = FirstParameter(parameters);
            ApiResponse<OrderDTO> response = new ApiResponse<OrderDTO>();

            OrderDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<OrderDTO>(postdata);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);
            }

            Order item = new Order();            
            item.FromDTO(postedItem);

            Order existing = MTApp.OrderServices.Orders.FindForCurrentStore(item.bvin);
            if (existing == null || existing.bvin == string.Empty)
            {
                item.StoreId = MTApp.CurrentStore.Id;
                MTApp.OrderServices.Orders.Create(item);
            }            
            else
            {
                MTApp.OrderServices.Orders.Update(item);
            }
            Order resultItem = MTApp.OrderServices.Orders.FindForCurrentStore(item.bvin);
            if (resultItem != null) response.Content = resultItem.ToDto();

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            // Single Item Delete
            response.Content = MTApp.OrderServices.Orders.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}