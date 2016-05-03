using RedisRestAPI.Models;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace RedisRestAPI
{
    public static class Worker
    {
        public static bool StopListener() 
        {
            Common com = new Common();
            List<SubscriptionMap> map = com.GetAllSubscriptions();
            string[] Channels = map.Select(m => m.Channel).ToArray();
            if (Channels.Count() > 0)
            {
                RedisServerChat.redisClient.UnSubscribe(Channels);
            }
            return true;
        }
        public static void StartListener() 
        {
            Common com = new Common();
            List<SubscriptionMap> map = com.GetAllSubscriptions();
            string[] Channels = map.Select(m => m.Channel).ToArray();
            if (Channels.Count() > 0)
            {
                Thread _work = new Thread(newListener);
                _work.Start(Channels);
            }
        }
        private static void newListener(Object Channels) 
        {
            RedisClient _Client = new RedisClient();
            using (var subscription = _Client.CreateSubscription())
            {
                MessageListener(subscription);
                subscription.SubscribeToChannels((string[])Channels); //blocks thread
            }
        }
        private static void MessageListener(IRedisSubscription subscription)
        {
            subscription.OnMessage = (channel, msg) =>
            {
                MessageModel.Messages.Add(new Message { ForChannel = channel, MessageContent = msg });
                Common com = new Common();
                com.SaveMessage(channel, msg);
            };
        }
    }

}