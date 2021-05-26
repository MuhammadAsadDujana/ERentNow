using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERentApi.Controllers
{
    public class BaseController : ApiController
    {
        public int UserId { get; set; }
        public string UserToken { get; set; }
        public User CurrentUser { get; set; }
    }
}
