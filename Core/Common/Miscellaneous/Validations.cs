using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Common.Miscellaneous
{
    public class ValidateModel
    {
        public bool res { get; set; }
        public List<string> ValidationErrors { get; set; }

    }


    public class Validations
    {
        static Regex IntRegex = new Regex(@"^[0-9]+$");
        static Regex AlphaRegex = new Regex(@"^[a-zA-Z]+$");
        static Regex EmailRegex = new Regex(@"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");


        public static ValidateModel ValidateFields(List<string> data)
        {
            ValidateModel validate = new ValidateModel();

            validate.res = true;
            validate.ValidationErrors = new List<string>();
            for (int i = 0; i < data.Count; i++)
            {
                if (!string.IsNullOrEmpty(data[i]))
                {
                    validate.res = false;
                    validate.ValidationErrors.Add(data[i]);
                }
            }
            return validate;
        }
        public static string RequiredValidator(string FieldID, string FieldName)
        {
            if (!string.IsNullOrWhiteSpace(FieldID))
            {
                return "";
            }
            else
            {
                return FieldName + " is required.";
            }
        }

        public static string LimitValidator(string FieldID, string Msg, int Limit)
        {
            if (!string.IsNullOrWhiteSpace(FieldID))
            {
                return FieldID + " is required.";
            }
            else if (FieldID.Length > Limit)
            {
                return Msg + " can not be greater than " + Limit + " characters.";
            }
            else
            {
                return "";
            }
        }



        public static string IntValidator(object data, string Msg)
        {
            if (!IntRegex.IsMatch(data.ToString()))
            {
                return "Please type numeric value only in " + Msg;
            }
            else
            {
                return "";
            }
        }
        public static string AlphaValidator(object data, string Msg)
        {
            if (!AlphaRegex.IsMatch(data.ToString()))
            {
                return "Please type Alphabetic value only in " + Msg;
            }
            else
            {
                return "";
            }
        }

        public static string EmailValidator(string Msg, string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return Msg + " is required.";
            }
            else if (!EmailRegex.IsMatch(data))
            {
                return "Please type valid email in " + Msg;
            }
            else
            {
                return "";
            }
        }
    }
}
