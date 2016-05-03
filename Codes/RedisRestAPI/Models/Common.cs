using RedisRestAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using ServiceStack.Redis;

namespace RedisRestAPI.Models
{
    public class Common
    {
        public List<SubscriptionMap> GetAllSubscriptions()
        {
            //RedisServerChat.redisClient.Get("Subscriptions");
            string Response = RedisServerChat.redisClient.GetValue("SubscriptionMap");
            List<SubscriptionMap> Map = new List<SubscriptionMap>();
            if (Response != null && Response.Count() > 0)
            {
                Map = Json.Decode(Response,typeof(List<SubscriptionMap>));   
            }
            return Map;

        }
        public bool SetSubscription(string Channel, string UKey)
        {
            SubscriptionMap map = new SubscriptionMap() { Channel = Channel, UKey = UKey };
            List<SubscriptionMap> nmap = GetAllSubscriptions();
            if (nmap.Exists(m => m.UKey.Equals(UKey) && m.Channel.Equals(Channel)))
            {
                return true;
            }
            else
            {
                if (Worker.StopListener())
                {
                    nmap.Add(map);
                    RedisServerChat.redisClient.Set("SubscriptionMap", nmap);
                    Worker.StartListener();
                }
                return true;
            }
       }
        public bool SaveMessage(string Channel, string MessageContent)
        {
            Message msg = new Message() { ForChannel = Channel, MessageContent = MessageContent, Id = Guid.NewGuid() };
            RedisClient client = new RedisClient();
            client.SetEntryInHash("Messages", msg.Id.ToString(), Json.Encode(msg));
            return true;
        }
        public bool GetALLMessage()
        {
            Dictionary<string, string> Messages = RedisServerChat.redisClient.GetAllEntriesFromHash("Messages");
            foreach (KeyValuePair<string, string> pair in Messages)
            {
                Message msg = Json.Decode(pair.Value, typeof(Message));
                MessageModel.Messages.Add(msg);

            }
            return true;
        }
    }

}