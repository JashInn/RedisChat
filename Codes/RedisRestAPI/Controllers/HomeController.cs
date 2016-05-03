using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedisRestAPI.Models;
namespace RedisRestAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Common com = new Common();
            //string MyChannel = com.GetAllSubscriptions().Where(sub => sub.UKey.Equals(Ukey)).Select(sub => sub.Channel).ToList().FirstOrDefault();

            return View();
        }
    }
}
