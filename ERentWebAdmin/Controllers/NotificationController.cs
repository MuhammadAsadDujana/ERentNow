using Core.Dto;
using Core.Model;
using Core.Services.NotificationService;
using Core.Services.UserService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ERentWebAdmin.Controllers
{
    public class NotificationController : Controller
    {
        private readonly UserService _serviceUser;
        private readonly NotificationService _serviceNotification;
        public NotificationController()
        {
            _serviceUser = new UserService();
            _serviceNotification = new NotificationService();
        }

        // GET: Notification
        public ActionResult PushNotifcation()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> PushNotifcation(string title, string message)
        {
            var userList = _serviceUser.GetUserProfileList();
            if (userList.Count() > 0)
            {
                var fcmList = userList.Where(x => x.FCMDeviceToken != null).ToList();
                List<string> fckTokenList = new List<string>();
                List<Notification> notificationsList = new List<Notification>();
                foreach (var item in fcmList)
                {
                    if (item.FCMDeviceToken != null)
                    {
      
                        fckTokenList.Add(item.FCMDeviceToken);
                      
                        Notification notification = new Notification()
                        {
                            UserId = item.Id,
                            FCMToken = item.FCMDeviceToken,
                            Title = title,
                            Message = message,
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                            IsViewed = false
                        };
                        notificationsList.Add(notification);

                    }
                }

                var res = _serviceNotification.InsertAllNotifications(notificationsList);

                if(res.Count > 0) 
                {
                    var str = await NotifyAsync(fckTokenList, title, message);

                    if (str)
                        return Json(str, JsonRequestBehavior.AllowGet);
                    else
                        return Json(false, JsonRequestBehavior.AllowGet);
                }


            }
            return null;
        }

        public async Task<bool> NotifyAsync(List<string> deviceIds, string title, string body)
        {
            try
            {

                string serverKey = string.Format("key={0}", ConfigurationManager.AppSettings["FCM_Server_Key"]);
                string fcm_Url = ConfigurationManager.AppSettings["FCM_URL"].ToString();
                // Get the sender id from FCM console
                string senderId = string.Format("id={0}", ConfigurationManager.AppSettings["FCM_SenderId"]);

                var data = new
                {
                    registration_ids = deviceIds, // Recipient device token
                    priority = "high",
                    content_available = true,
                    notification = new { title, body, badge = 1 }
                    //  data = new { sessionId = room.SessionId, token = room.Token, role = room.Role, videoCategory = room.VideoCategory, userId = room.UserId }
                };

                // Using Newtonsoft.Json
                var jsonBody = JsonConvert.SerializeObject(data);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, fcm_Url))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                    httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);

                        if (result.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                // _logger.LogError($"Exception thrown in Notify Service: {ex}");
            }

        }

    }
}