using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Corex.Web.Helpers
{
    public class UrlHelper
    {
        public static NameValueCollection BuildQueryString()
        {
            return ParseQueryString("");
        }
        public static NameValueCollection ParseQueryString(string qs)
        {
            return HttpUtility.ParseQueryString(qs);
        }
    }
}
