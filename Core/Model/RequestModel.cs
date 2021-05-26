using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Model
{
    public class RequestModel
    {
        public string search { get; set; }
        public int? start { get; set; }
        public int? length { get; set; }

        public int? Offset { get { return start; } set { start = value; } }
        public int? PageSize { get { return length; } set { length = value; } }
        public int UserID { get; set; }
    }

}