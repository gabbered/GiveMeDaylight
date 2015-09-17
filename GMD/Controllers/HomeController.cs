using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GMD.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string SendCommand(string command, string auth)
        {
            if (command == "setDayTime")
            {
                command = "settimeofday 07:30";
            }
            else {
                command = "settimeofday 01:00";
            }

            StreamReader sr = new StreamReader(Server.MapPath("~/App_Data/config.txt"));
            string ipAddress="";
            string port="";
            string pass="";
            string testAuth = "";

            while(!sr.EndOfStream)
            {
                var r = sr.ReadLine();

                var end = r.IndexOf("=");

                var val = r.Substring(0, end);
                
                

                switch(val)
                {
                    case "ipAddress":
                        ipAddress = r.Substring(end+1, r.Length - end -1);
                        break;
                    case "port":
                        port = r.Substring(end + 1, r.Length - end - 1);
                        break;
                    case "pass":
                        pass = r.Substring(end + 1, r.Length - end - 1);
                        break;
                    case "auth":
                        testAuth = r.Substring(end + 1, r.Length - end - 1).Replace("\"", "");
                        break;
                }

            }
            
            string str_Path = Server.MapPath("~/App_Data/mcrcon.exe");
            string args = "-t -H " + ipAddress + " -P " + port + " -p " + pass + "";
            if (testAuth.ToString() == auth)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(str_Path, args);

                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;

                Process process = Process.Start(processStartInfo);

                if (process != null)
                {
                    process.StandardInput.WriteLine(command);
                    process.StandardInput.Close(); // line added to stop process from hanging on ReadToEnd()

                    string outputString = process.StandardOutput.ReadToEnd();

                    process.Close();

                    return outputString;
                }
            }
            return string.Empty;
        }

        public ActionResult Index2()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
