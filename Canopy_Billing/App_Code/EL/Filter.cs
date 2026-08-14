using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Vendor_Portal.App_Code.EL
{
    public class Filter
    {
        public static string SQLInjectionFilter(string Input)
        {
            string Filtered = null;
            Filtered = Input.Trim();
            try
            {
                Filtered = Filtered.Replace("'--", "`--");
                Filtered = Filtered.Replace("'", "`");
                Filtered = Filtered.Replace("\"", "`");
                Filtered = Filtered.Replace("%", "(percent)");
            }
            catch
            { }
            return Filtered;
        }
        public static string ValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"[{0}]+", invalidChars);
            return Regex.Replace(name, invalidReStr, "_");
        }
    }
}