using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MimeKit;

namespace AutoFB {
    public partial class FBClient : WebClient {
        private System.Net.CookieContainer CookieContainer;
        private const string accept = "text/html, application/xhtml+xml, image/jxr, */*";
        private const string acceptEncoding = "gzip, deflate";
        private const string acceptLanguage = "zh-Hant-TW,zh-Hant;q=0.7,ja;q=0.3";
        private const string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
        bool redirect;
        private Uri ResponseUri;
        HtmlDocument HtmEle = new HtmlDocument();

        public FBClient() {
            this.redirect = true;
            //this.CookieContainer = new System.Net.CookieContainer();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //ReadCookies();
        }

        public new string UploadValues(string skey, NameValueCollection payload) {
            return Encoding.UTF8.GetString(base.UploadValues(skey, payload));
        }


        public string UploadFiles() {
            List<MimePart> mimeParts = new List<MimePart>();



            return "";
        }

        //public new byte[] UploadValues(string skey, NameValueCollection payload)
        //{
        //    return base.UploadValues(skey, payload);
        //}

        protected override WebRequest GetWebRequest(Uri address) {
            this.Headers.Set("Accept", accept);
            this.Headers.Set("Accept-Encoding", acceptEncoding);
            this.Headers.Set("Accept-Language", acceptLanguage);
            Console.Write("###" + this.Headers.Get("User-Agent") + "###");
            this.Headers.Set("User-Agent", userAgent);
            
            if (ResponseUri != null) this.Headers.Set("Referer", ResponseUri.ToString());
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.KeepAlive = true;

            HttpWebRequest request2 = request as HttpWebRequest;

            if (request2 != null) {
                request2.CookieContainer = this.CookieContainer;
                request2.AllowAutoRedirect = this.redirect;
            }

            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request) {
            WebResponse webResponse = base.GetWebResponse(request);
            this.ResponseUri = webResponse.ResponseUri;
            return webResponse;
        }

        public new string DownloadString(string Uri) {
            var ret = (Encoding.UTF8.GetString(base.DownloadData(Uri)));
            return ret;
        }

        public string DownloadString(string Uri, Encoding Encoding) {
            var ret = (Encoding.GetString(base.DownloadData(Uri)));
            return ret;
        }

        public void SaveCookies() {
            WriteCookiesToDisk(System.Environment.CurrentDirectory + "\\cookies.dat", this.CookieContainer);
        }

        public void ReadCookies() {
            this.CookieContainer = ReadCookiesFromDisk(System.Environment.CurrentDirectory + "\\cookies.dat");
        }

        public void WriteCookiesToDisk(string file, CookieContainer cookieJar) {
            using (Stream stream = File.Create(file)) {
                try {
                    Console.Out.Write("Writing cookies to disk... ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, cookieJar);
                    Console.Out.WriteLine("Done.");
                } catch (Exception e) {
                    Console.Out.WriteLine("Problem writing cookies to disk: " + e.GetType());
                }
            }
        }

        public CookieContainer ReadCookiesFromDisk(string file) {
            try {
                using (Stream stream = File.Open(file, FileMode.Open)) {
                    Console.Out.Write("Reading cookies from disk... ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    Console.Out.WriteLine("Done.");
                    return (CookieContainer)formatter.Deserialize(stream);
                }
            } catch (Exception e) {
                Console.Out.WriteLine("Problem reading cookies from disk: " + e.GetType());
                return new CookieContainer();
            }
        }

        public string UrlTransfer(string Uri) {
            string ret = Uri;
            ret = ret.Replace("amp;", "");

            return ret;
        }

    }
}
