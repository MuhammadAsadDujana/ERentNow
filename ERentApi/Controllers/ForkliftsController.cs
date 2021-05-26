using Core.Common.Communication;
using Core.Common.Miscellaneous;
using Core.Model;
using Core.Services.ForkliftService;
using Core.Services.UserService;
using ERentApi.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ERentApi.Controllers
{
    public class ForkliftsController : BaseController
    {
        #region Fields

        private readonly ForklistService _IforkliftsService = new ForklistService();

        #endregion

        #region Methods

        [HttpGet]
        public HttpResponseMessage Dashboard([FromUri] Forklift model, [FromUri] DataSourceRequest request)
        {
            HttpResponseMessage result = null;
            model.IsActive = true;

            var ForkliftList = _IforkliftsService.GetForkliftInfo(model);
            if (ForkliftList != null)
            {
                int PageSize = request.Page == 1 ? -1 : request.PageSize;
                var DataList = ForkliftList.Skip(1 * PageSize).Take(request.PageSize).ToList();

                result = Request.CreateResponse(HttpStatusCode.OK, DataList);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.NoContent, "No Record Found");
            }

            return result;
        }

        [HttpGet]
        public HttpResponseMessage Models([FromUri] ForkliftsModel model, [FromUri] DataSourceRequest request)
        {
            HttpResponseMessage result = null;
            model.IsActive = true;

            var ForkliftList = _IforkliftsService.GetForkliftModel(model);
            if (ForkliftList != null)
            {
                int PageSize = request.Page == 1 ? -1 : request.PageSize;
                var DataList = ForkliftList.Skip(1 * PageSize).Take(request.PageSize).ToList();

                result = Request.CreateResponse(HttpStatusCode.OK, DataList);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.NoContent, "No Record Found");
            }

            return result;
        }

        [HttpGet]
        [UserAuthorization]
        public HttpResponseMessage NotificationList()
        {
            HttpResponseMessage result = null;
            var userId = UserId;
            var notificationList = _IforkliftsService.GetNotificationListService(userId);
            if(notificationList != null)
            {
                result = Request.CreateResponse(HttpStatusCode.OK, notificationList);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.NoContent, "No Record Found");
            }

            return result;
        }

        [HttpGet]
        public HttpResponseMessage ViewNotification(int ReservationId)
        {
            HttpResponseMessage result = null;

            var notificationList = _IforkliftsService.ViewNotificationByIdService(ReservationId);
            if (notificationList != null)
            {
                result = Request.CreateResponse(HttpStatusCode.OK, notificationList);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.NoContent, "No Record Found");
            }

            return result;
        }

        [HttpGet]
        [UserAuthorization]
        public HttpResponseMessage ViewOrderHistory()
        {
            HttpResponseMessage result = null;
            var userId = UserId;
            var orderHistoryList = _IforkliftsService.ViewOrderHistoryByIdService(userId);
            if (orderHistoryList.Count() > 0)
            {
                result = Request.CreateResponse(HttpStatusCode.OK, orderHistoryList);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.NoContent, "No Record Found");
            }

            return result;
        }


        [HttpPost]
        [UserAuthorization]
        public HttpResponseMessage ForkliftReservation(ReservationModel model)
        {
            HttpResponseMessage result = null;

            try
            {
                var list = new List<string>();
                
                foreach (var rItem in model.ReservationList)
                {
                    string insertLine = "";
                    rItem.UserId = UserId;
                    insertLine = "(" + rItem.UserId + "," + rItem.ForkliftsModelId + "," + rItem.Quantity + ",'" + rItem.StartDate + "','" + rItem.ReturnDate + "','" + rItem.Location + "','" + rItem.InsurancePicture + "'," + 1 + "," + rItem.CreatedBy + ",'" + DateTime.Now + "',0,null,1,0,0)";
                    list.Add(insertLine);
                }

                model.ReservationLine = string.Join(",", list);

                var ForkliftList = _IforkliftsService.InsertReservations(model);
                if (ForkliftList != null)
                {
                    result = Request.CreateResponse(HttpStatusCode.OK, "Successfully Inserted");
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.NoContent, "No Record Found");
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }


            return result;
        }


        #endregion
    }
}
