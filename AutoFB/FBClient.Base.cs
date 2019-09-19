﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoFB
{
    partial class FBClient
    {
        public string userName = "";
        public Boolean isLogin = false;

        public Boolean loginFB(string ac, string pw)
        {
            isLogin = false;

            string response = this.DownloadString("https://m.facebook.com/");

            Regex regex = new Regex("<input type=\"hidden\" name=\"lsd\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response))
            {
                return false;
            }
            string lsd = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"jazoest\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response))
            {
                return false;
            }
            string jazoest = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"m_ts\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response))
            {
                return false;
            }
            string m_ts = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"li\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response))
            {
                return false;
            }
            string li = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"try_number\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response))
            {
                return false;
            }
            string try_number = regex.Match(response).Groups[1].Value;

            regex = new Regex("<input type=\"hidden\" name=\"unrecognized_tries\" value=\"([^\"]+)\"");
            if (!regex.IsMatch(response))
            {
                return false;
            }
            string unrecognized_tries = regex.Match(response).Groups[1].Value;

            regex = new Regex("<form method=\"post\" action=\"([^\"]+)\" class=\"([^\"]+)\" id=\"login_form\"");
            if (!regex.IsMatch(response))
            {
                return false;
            }
            string url = regex.Match(response).Groups[1].Value;

            url = "https://m.facebook.com" + url.Replace("&amp;", "&");

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
            if (!regex.IsMatch(response))
            {
                return false;
            }
            url = regex.Match(response).Groups[1].Value;
            url = "https://m.facebook.com" + url.Replace("&amp;", "&");

            response = DownloadString(url);

            regex = new Regex("id=\"mbasic_logout_button\">([^\"]+)</a>");
            if (!regex.IsMatch(response))
            {
                return false;
            }
            userName = regex.Match(response).Groups[1].Value.Split('（')[1].Split('）')[0];
            isLogin = true;
            //id="mbasic_logout_button">登出（鄭智嘉）</a>
            return true;
        }
    }

}
