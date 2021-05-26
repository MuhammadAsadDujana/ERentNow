using Core.Common.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class ResponseModel
    {
        public dynamic data { get; set; }
        public string msg { get; set; }
        public bool isError { get; set; }
        public bool isInserted { get; set; }
        public bool isSuccess { get; set; }
        public bool isAuthorized { get; set; }
        public ValidateModel Validate { get; set; }
        public dynamic CartData { get; set; }
        public int? recordsTotal { get; set; }
        public int? recordsFiltered { get; set; }
        public int Count { get; set; }

    }
}
