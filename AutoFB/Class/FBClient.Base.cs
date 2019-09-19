using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoFB {
    partial class FBClient {
        public string userName = "";
        public Boolean isLogin = false;

        
        public Boolean LoginFB(string ac, string pw) {
            isLogin = false;

            string response = this.DownloadString("https://m.facebook.com/");

            Regex regex = new Regex("<input type=\"hidden\" name=\"lsd\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string lsd = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"jazoest\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string jazoest = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"m_ts\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string m_ts = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"li\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string li = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"try_number\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string try_number = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"unrecognized_tries\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string unrecognized_tries = regex.Match(response).Groups[1].Value;

            regex = new Regex("<form method=\"post\" action=\"([^\"]+)\" class=\"([^\"]+)\" id=\"login_form\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string url = regex.Match(response).Groups[1].Value;

            url = "https://m.facebook.com" + UrlTransfer(url);

            NameValueCollection payload = new NameValueCollection();
            payload.Add("lsd", lsd);
            payload.Add("jazoest", jazoest);
            payload.Add("m_ts", m_ts);
            payload.Add("li", li);
            payload.Add("try_number", try_number);
            payload.Add("unrecognized_tries", unrecognized_tries);
            payload.Add("email", ac); //"hcspoxymym_1568896683@tfbnw.net"
            payload.Add("pass", pw); //"zxczxczxc"
            payload.Add("login", "登入");

            response = UploadValues(url, payload);

            ///login/save-device/cancel/?flow=interstitial_nux&amp;nux_source=regular_login
            regex = new Regex("<a href=\"([^\"]+)\" class=\"([^\"]+)\" target=\"_self\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            url = regex.Match(response).Groups[1].Value;
            url = "https://m.facebook.com" + UrlTransfer(url);

            response = DownloadString(url);

            regex = new Regex("id=\"mbasic_logout_button\">([^\"]+)</a>");
            if (!regex.IsMatch(response)) {
                return false;
            }
            userName = regex.Match(response).Groups[1].Value.Split('（')[1].Split('）')[0];
            isLogin = true;
            //id="mbasic_logout_button">登出（鄭智嘉）</a>
            return true;
        }


        //public List<string> groupList = null;

        public bool PostInGroup(string context) {
            string response = DownloadString("https://m.facebook.com/groups/1248359792000849?view=group&refid=18");

            Regex regex = new Regex("<form method=\"post\" action=\"([^\"]+)\" class=\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string url = regex.Match(response).Groups[1].Value;
            url = "https://m.facebook.com" + UrlTransfer(url);

            regex = new Regex("<input type=\"hidden\" name=\"fb_dtsg\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string fb_dtsg = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"jazoest\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string jazoest = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"target\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string target = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"c_src\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string c_src = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"cwevent\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string cwevent = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"referrer\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string referrer = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"ctype\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string ctype = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"cver\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response)) {
                return false;
            }
            string cver = regex.Match(response).Groups[1].Value;


            NameValueCollection payload = new NameValueCollection();
            payload.Add("fb_dtsg", fb_dtsg);
            payload.Add("jazoest", jazoest);
            payload.Add("target", target);
            payload.Add("c_src", c_src);
            payload.Add("cwevent", cwevent);
            payload.Add("referrer", referrer);
            payload.Add("ctype", ctype);
            payload.Add("cver", cver);
            payload.Add("rst_icv", "");
            payload.Add("xc_message", context);
            payload.Add("view_post", "發佈");

            response = UploadValues(url, payload);

            //https://m.facebook.com/groups/1248359792000849?_rdr
            return ResponseUri.ToString().Contains("groups");
        }

        public List<string> GetGroupList() {
            string response = DownloadString("https://m.facebook.com/groups/?seemore&refid=27");
            HtmEle.LoadHtml(response);

            List<string> tempList = new List<string>();
            string temp;
            Regex regex = new Regex("/groups/(\\d+).refid=27");
            foreach (HtmlNode element in HtmEle.DocumentNode.SelectNodes("//a")) {

                temp = element.GetAttributeValue("href", "fasle");

                if (regex.IsMatch(temp)) tempList.Add(regex.Match(temp).Groups[1].Value);
            }

            return tempList;
        }

        public Boolean LoginOut() {
            string response = DownloadString("https://m.facebook.com/");
            HtmEle.LoadHtml(response);

            //If HtmEle.DocumentNode.SelectNodes("//table//tr") Is Nothing Then Return False
            if (HtmEle.DocumentNode.SelectNodes("//a") is null) {
                return false;
            }

            foreach (HtmlNode element in HtmEle.DocumentNode.SelectNodes("//a")) {
                if (element.InnerText.Contains("登出")) {
                    string url = element.GetAttributeValue("href", "false");
                    if (url != "false") {
                        url = "https://m.facebook.com" + UrlTransfer(url);
                        response = DownloadString(url);
                    }
                    break;
                }
            }
            CookieContainer = new System.Net.CookieContainer();
            return false;
        }

        public Boolean CheckLogin() {
            string response = DownloadString("https://m.facebook.com/");

            Regex regex = new Regex("id=\"mbasic_logout_button\">([^\"]+)</a>");
            if (!regex.IsMatch(response)) {
                return false;
            }
            userName = regex.Match(response).Groups[1].Value.Split('（')[1].Split('）')[0];
            isLogin = true;
            return true;
        }
    }
}
