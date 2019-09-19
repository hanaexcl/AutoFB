using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoFB
{
    public partial class FBClient : WebClient
    {
        private System.Net.CookieContainer CookieContainer;
        private const string accept = "text/html, application/xhtml+xml, image/jxr, */*";
        private const string acceptEncoding = "gzip, deflate";
        private const string acceptLanguage = "zh-Hant-TW,zh-Hant;q=0.7,ja;q=0.3";
        private const string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
        bool redirect;

        public FBClient()
        {
            this.redirect = true;
            this.CookieContainer = new System.Net.CookieContainer();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public new string UploadValues(string skey, NameValueCollection payload)
        {
            return Encoding.UTF8.GetString(base.UploadValues(skey, payload));
        }


        //public new byte[] UploadValues(string skey, NameValueCollection payload)
        //{
        //    return base.UploadValues(skey, payload);
        //}

        protected override WebRequest GetWebRequest(Uri address)
        {
            this.Headers.Set("Accept", accept);
            this.Headers.Set("Accept-Encoding", acceptEncoding);
            this.Headers.Set("Accept-Language", acceptLanguage);
            this.Headers.Set("User-Agent", userAgent);
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            HttpWebRequest request2 = request as HttpWebRequest;

            if (request2 != null)
            {
                request2.CookieContainer = this.CookieContainer;
                request2.AllowAutoRedirect = this.redirect;
            }

            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse webResponse = base.GetWebResponse(request);
            return webResponse;
        }

        public new string DownloadString(string Uri)
        {
            var ret = (Encoding.UTF8.GetString(base.DownloadData(Uri)));
            return ret;
        }

        public string DownloadString(string Uri, Encoding Encoding)
        {
            var ret = (Encoding.GetString(base.DownloadData(Uri)));
            return ret;
        }
    }
}
