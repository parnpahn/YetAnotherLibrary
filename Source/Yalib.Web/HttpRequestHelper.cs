using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Yalib.Web
{
    public static class HttpRequestHelper
    {
        public static string FormToQueryString(HttpRequest req)
        {
            StringBuilder sb = new StringBuilder("?");

            foreach (string key in req.Form.AllKeys)
            {
                if (key.StartsWith("__"))
                {
                    continue;   // Skip __VIEWSTATE and __EVENT* 
                }
                sb.Append(key);
                sb.Append("=");
                sb.Append(req.Form[key]);
                sb.Append("&");
            }

            // remove last '&'
            if (sb.Length > 2)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public static HttpStatusCode FormPost(string uri, string postString, out string result)
        {
            if (postString.StartsWith("?"))
            {
                postString = postString.Remove(0, 1);
            }

            var req = WebRequest.Create(uri) as HttpWebRequest;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] buf = System.Text.Encoding.UTF8.GetBytes(postString);
            req.ContentLength = buf.Length;

            Stream requestStream = req.GetRequestStream();
            requestStream.Write(buf, 0, buf.Length);
            requestStream.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse) req.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();

            StringBuilder sb = new StringBuilder();
            
            using (StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sb.Append(line);
                }
            }
            result = sb.ToString();
            return httpWebResponse.StatusCode;
        }

        /// <summary>
        /// Set object properties from an URL query string. The properties should be type of String. The other types are not tested.
        /// Note: If a property name does not exist in the object, it will be simply ignored (which mean no exception).
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="queryString">Form example</param>
        public static void PopulateQueryStringToObject(object theObject, string queryString)
        {
            string[] parameters = queryString.Split('&');
            foreach (string param in parameters)
            {
                string[] nameValue = param.Split('=');
                if (nameValue.Length == 2)
                {
                    string name = nameValue[0];
                    string value = HttpUtility.UrlDecode(nameValue[1]);

                    var aType = theObject.GetType();
                    var aProperty = aType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (aProperty != null)
                    {
                        if (aProperty.PropertyType.IsEnum)
                        {
                            var valueObj = Enum.Parse(aProperty.PropertyType, value);
                            aProperty.SetValue(theObject, valueObj, null);
                        }
                        else
                        {
                            aProperty.SetValue(theObject, value, null);
                        }
                    }
                }
            }
        }
    }
}