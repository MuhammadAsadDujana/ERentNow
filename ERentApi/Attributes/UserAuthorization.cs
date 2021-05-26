using ERentApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Core.Common.Helper;
using Core.Services.UserService;
using Core.Model;
using System.Net.Http;

namespace ERentApi.Attributes
{
    public class UserAuthorization: ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                BaseController baseController = (BaseController)actionContext.ControllerContext.Controller;
                baseController.UserToken = actionContext.Request.Headers.GetValues("Token").FirstOrDefault();

                if (!string.IsNullOrEmpty(baseController.UserToken))
                {
                    var Data = TokkenManager.ValidateToken(baseController.UserToken);
                    baseController.UserId = int.Parse(Data.Value);
                    UserService UserDataAccess = new UserService();
                    baseController.CurrentUser = UserDataAccess.getUserByIdService(baseController.UserId);
                    if (baseController.CurrentUser == null)
                    {
                        var response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "User is not Autheticated");
                        actionContext.Response = response;
                    }
                    else
                    {
                        if (baseController.CurrentUser.TokenExpiryDate < DateTime.Now)
                        {
                            var response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "Token Expired");
                            actionContext.Response = response;
                        }
                    }
                }
                else
                {
                    var response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "User is not Authorized");
                    actionContext.Response = response;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString() == "IDX10223: Lifetime validation failed. The token is expired. ValidTo: '[PII is hidden]', Current time: '[PII is hidden]'.")
                {
                    var response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "Token Expired");
                    actionContext.Response = response;
                }
                else
                {
                    var response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message.ToString());
                    actionContext.Response = response;
                }
            }
        }

	
	}
}