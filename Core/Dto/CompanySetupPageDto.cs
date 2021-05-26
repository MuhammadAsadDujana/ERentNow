using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Core.Dto
{
    public class CompanySetupPageDto
    {
        public int CompanyId { get; set; }
        public HttpPostedFileWrapper Logo { get; set; }
        public string ProfileLogo { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string BannerMassage { get; set; }
        [AllowHtml]
        public string TermsOfUse { get; set; }
        [AllowHtml]
        public string PrivacyPolicy { get; set; }
        //public HttpPostedFileWrapper imageUpload { get; set; }
    }
}
