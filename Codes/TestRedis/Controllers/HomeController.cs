using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TestRedis.Models;

namespace TestRedis.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            string UKey = WebConfigurationManager.AppSettings["UKey"].ToString();
            RestClient restClient = new RestClient("http://localhost:53636");
            RestRequest request = new RestRequest("api/Values", Method.GET);
            request.Parameters.Add(new Parameter() { ContentType = "json", Name = "UKey", Type = ParameterType.GetOrPost, Value = UKey });
            var response = restClient.Execute(request);
            if (response.Content != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string Channel = (string)JsonConvert.DeserializeObject(response.Content, typeof(string));
                    if (Channel.Equals("NOTFOUND"))
                    {
                        return View("SetChannel");
                    }
                    else
                    {
                        TempData["ChannelName"] = Channel;
                        return View("MessageListener");
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult SetChannel(string ChannelName) 
        {
            string UKey = WebConfigurationManager.AppSettings["UKey"].ToString();
            RestClient restClient = new RestClient("http://localhost:53636");
            RestRequest request = new RestRequest("api/Values", Method.POST);
            SubscriptionMap map = new SubscriptionMap();
            map.UKey = UKey;
            map.Channel = ChannelName;
            string ChannelParameter = JsonConvert.SerializeObject(map);
            //request.Parameters.Add(new Parameter() { ContentType = "json", Name = "Value", Type = ParameterType.GetOrPost, Value = ChannelParameter });
            request.AddJsonBody(ChannelParameter);
            var response = restClient.Execute(request);
            if (response.Content != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string Status = (string)JsonConvert.DeserializeObject(response.Content, typeof(string));
                    if (Status.Equals("SAVED"))
                    {
                        TempData["ChannelName"] = ChannelName;
                        return View("MessageListener");
                    }
                }
            }
            ViewBag.ErrorMessage = "Could not save";
            return View();
        }
        public JsonResult MessageListener() 
        {
            TempData.Keep("ChannelName");
            string ChannelName = TempData["ChannelName"].ToString();
            string UKey = WebConfigurationManager.AppSettings["UKey"].ToString();
            RestClient restClient = new RestClient("http://localhost:53636");
            RestRequest request = new RestRequest("api/Values", Method.GET);
            SubscriptionMap map = new SubscriptionMap();
            map.UKey = UKey;
            map.Channel = ChannelName;
            //string ChannelParameter = JsonConvert.SerializeObject(map);
            request.Parameters.Add(new Parameter() { ContentType = "string", Name = "ChannelName", Type = ParameterType.GetOrPost, Value = ChannelName });
            request.Parameters.Add(new Parameter() { ContentType = "string", Name = "UKey", Type = ParameterType.GetOrPost, Value = UKey });
            var response = restClient.Execute(request);
            if (response.Content != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string MessageResult = (string)JsonConvert.DeserializeObject(response.Content, typeof(string));
                    if (!string.IsNullOrEmpty(MessageResult))
                    {
                        MessageModel.Messages = (List<Message>)JsonConvert.DeserializeObject(MessageResult, typeof(List<Message>));
                        string[] DisplayMessages = MessageModel.Messages.Select(m=>m.MessageContent).ToArray();
                        return Json(DisplayMessages);
                    }
                }
            }
            return Json("ERROR");
        }

    }
}
