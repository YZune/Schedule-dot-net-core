using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Schedule.Helper;

namespace Schedule.Controllers
{
    [Route("api/Login")]
    public class LoginController : Controller
    {
        readonly CookieContainer mycookie = new CookieContainer();

        [HttpGet("getCheckCode")]
        public Image GetCheck()
        {
            return HttpHelper.GetCheckCode("http://xk.suda.edu.cn/CheckCode.aspx", mycookie);
        }

        [HttpPost("loginPost")]
        public string LoginPost(string id, string pwd, string checkCode)
        {
            string poststr = "__VIEWSTATE=dDwtMTE5ODQzMDQ1NDt0PDtsPGk8MT47PjtsPHQ8O2w8aTw0PjtpPDc%2BO2k8OT47PjtsPHQ8cDw7cDxsPHZhbHVlOz47bDx5ejFiZXN0Ym95Oz4%2BPjs7Pjt0PHA8O3A8bDxvbmNsaWNrOz47bDx3aW5kb3cuY2xvc2UoKVw7Oz4%2BPjs7Pjt0PHQ8OztsPGk8Mj47Pj47Oz47Pj47Pj47PrgxSkC%2B4UlEA8IwZoJEoMAIEqvo&TextBox1="
       + id + "&TextBox2=" + pwd + "&TextBox3=" + checkCode + "&Button1=";
            byte[] data = Encoding.UTF8.GetBytes(poststr);
            return HttpHelper.LoginPost("http://xk.suda.edu.cn/", data, mycookie, "gb2312");
        }
    }
}