using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Yalib.Web
{
    public static class WebHelper
    {
        /// <summary>
        /// Note: For Web Forms only!
        /// </summary>
        public static string GetRootUrl()
        {
            if (HttpContext.Current == null)
            {
                throw new Exception("Calling WebHelper.GetRootUrl() but HttpContext.Current is NULL!");
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
                throw new Exception("Calling WebHelper.RelativeToAbsoluteUrl() but HttpContext.Current is NULL!");
            }
            var page = HttpContext.Current.CurrentHandler as System.Web.UI.Page;
            if (page != null)
            {
                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + page.ResolveUrl(relativeUrl);
            }
            return relativeUrl;
        }

        public static string RelativeToAbsoluteUrlSafe(string relativeUrl)
        {
            if (String.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return string.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        [Obsolete("Use RelativeToAbsoluteUrl() or RelativeToAbsoluteUrlSafe() instead.")]
        public static string ToAbsoluteUrl(string relativeUrl)
        {
            return RelativeToAbsoluteUrlSafe(relativeUrl);
        }


        public static string GetIPv4Address()
        {
            string ipv4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4Address = IPA.ToString();
                    break;
                }
            }

            if (ipv4Address != String.Empty)
            {
                return ipv4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4Address = IPA.ToString();
                    break;
                }
            }

            return ipv4Address;
        }

        public static string GetClientIPAddress(HttpRequest req)
        {
            string szRemoteAddr = req.ServerVariables["REMOTE_ADDR"];
            string szXForwardedFor = req.ServerVariables["X_FORWARDED_FOR"];
            string szIP = "";

            if (szXForwardedFor == null)
            {
                szIP = szRemoteAddr;
            }
            else
            {
                szIP = szXForwardedFor;
                if (szIP.IndexOf(",") > 0)
                {
                    string[] arIPs = szIP.Split(',');

                    foreach (string item in arIPs)
                    {
                        if (item != "127.0.0.1")
                        {
                            return item;
                        }
                    }
                }
            }
            return szIP;
        }

        /// <summary>
        /// Download remote file via HTTP.
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="fileName">File name that does not include path (for user to see the saved file name).</param>
        public static void DownloadFile(string url, string fileName)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] buffer = wc.DownloadData(url);

            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Clear();
            string encodedFileName = HttpUtility.UrlEncode(fileName);
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + encodedFileName);
            HttpContext.Current.Response.AddHeader("Content-Length", buffer.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(buffer);
            HttpContext.Current.Response.End();
        }
    }
}
