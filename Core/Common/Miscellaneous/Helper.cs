using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Common.Miscellaneous
{
   public class Helper
    {

        public static bool FileWritter(string contents, string strPath, string FileName, string Extension)
        {
            Path.Combine(strPath, FileName, Extension);
            StreamWriter writer = new StreamWriter(strPath, true);
            writer.Write(contents);
            writer.Flush();
            writer.Close();
            writer.Dispose();
            writer = null;
            return true;
        }

        public static bool FileWritter(string contents, string strPath)
        {
            StreamWriter writer = new StreamWriter(strPath, true);
            writer.Write(contents);
            writer.Flush();
            writer.Close();
            writer.Dispose();
            writer = null;
            return true;
        }

        public static string getVisitorIP()
        {
            string VisitorsIPAddr = string.Empty;
            //Users IP Address.                
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                //To get the IP address of the machine and not the proxy
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;

            return VisitorsIPAddr;
        }

        public static string getVisitorBrowserInfo()
        {
            string VisitorsBrowserInfo = string.Empty;
            HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            string UserAgent = HttpContext.Current.Request.UserAgent;
            VisitorsBrowserInfo = "{Browser-Capabilities "
                     + "(Type = '" + browser.Type + "' "
                     + "Name = '" + browser.Browser + "' "
                     + "Version = '" + browser.Version + "' "
                     + "Major Version = '" + browser.MajorVersion + "' "
                     + "Minor Version = '" + browser.MinorVersion + "' "
                     + "Platform = '" + browser.Platform + "' "
                     + "Is Win32 = '" + browser.Win32 + "' "
                     + "Is Beta = '" + browser.Beta + "' "
                     + "Supports Cookies = '" + browser.Cookies + "' "
                     + "Supports ECMAScript = '" + browser.EcmaScriptVersion.ToString() + "' "
                     + "Supports JavaScript Version = '" + browser.JScriptVersion + "' "
                     + "UserAgent = '" + UserAgent + "')}";
            return VisitorsBrowserInfo;
        }
    }
}
