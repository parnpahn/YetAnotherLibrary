using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Yalib.Web.WebForms
{
    public static class WebFormHelper
    {
        /// <summary>
        /// Note: For Web Forms only!
        /// </summary>
        public static string GetRootUrl()
        {
            if (HttpContext.Current == null) 
            {
                throw new Exception("Calling WebFormHelper.GetRootUrl() but HttpContext.Current is NULL!");
            }
            //return RelativeToAbsoluteUrl("~/");
            string rootUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;
            if (!rootUrl.EndsWith("/"))
            {
                rootUrl += "/";
            }
            return rootUrl;
        }

        /// <summary>
        /// Note: For Web Forms only!
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static string RelativeToAbsoluteUrl(string relativeUrl)
        {
            if (HttpContext.Current == null) 
            {
                throw new Exception("Calling WebFormHelper.RelativeToAbsoluteUrl() but HttpContext.Current is NULL!");
            }
            var page = HttpContext.Current.CurrentHandler as System.Web.UI.Page;
            if (page != null) 
            {
                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + page.ResolveUrl(relativeUrl);
            }
            return relativeUrl;
        }
    }
}
