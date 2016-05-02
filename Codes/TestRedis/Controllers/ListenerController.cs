using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestRedis.Models;

namespace TestRedis.Controllers
{
    public class ListenerController : AsyncController
    {
        //
        // GET: /Listener/

        public ActionResult Index()
        {

            ViewBag.Content = "Hello";
            ViewBag.Title = "Hello World";
            ViewBag.Message = "Subscribed";
            return View();

        }
        [HttpPost]
        public ActionResult Index_Send(string message)
        {
            return Content(message);

        }
        [HttpPost]
        public void Index_sub(string message)
        {
            if (MessageModel.Subscriptions.Contains(message))
            {
                RedisServerChat.redisClient.UnSubscribe(message);
            }
            else
            {
                using (var subscription = RedisServerChat.redisClient.CreateSubscription())
                {
                    MessageListener(subscription);
                    subscription.SubscribeToChannels(message); //blocks thread
                }

            }
        }
        public void Index_Unsub()
        {
                RedisServerChat.redisClient.UnSubscribe(MessageModel.Subscriptions);
        }
        public void MessageListener(IRedisSubscription subscription)
        {
            subscription.OnMessage = (channel, msg) =>
            {
                MessageModel.Messages.Add(new Message { ForChannel=channel,MessageContent=msg});
            };
        }
        [HttpPost]
        public JsonResult Index_GetMessages() 
        {
            string[] firstNames = MessageModel.Messages.Where(m => m.ForChannel.Equals("redischat")).Select(message => message.MessageContent).ToArray();
            MessageModel.Messages.Where(m => m.ForChannel.Equals("redischat")).ToList().ForEach(mes=>MessageModel.Messages.Remove(mes));
            return Json(firstNames);
        }
        public RedirectResult IndexCompleted(string headlines)
        {
            ViewBag.Message = headlines;
            return new RedirectResult("Index");
        }

    }
}
