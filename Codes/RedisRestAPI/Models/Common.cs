using RedisRestAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace RedisRestAPI.Models
{
    public class Common
    {
        public List<SubscriptionMap> GetAllSubscriptions() 
        {
            //RedisServerChat.redisClient.Get("Subscriptions");
            byte[] Response = RedisServerChat.redisClient.Get("SubscriptionsMap");
            List<SubscriptionMap> Map = new List<SubscriptionMap>();
            if (Response != null && Response.Count() > 0) 
            {
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter BinForm = new BinaryFormatter();
                memStream.Write(Response, 0, Response.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Map = (List<SubscriptionMap>)BinForm.Deserialize(memStream);
            }
            return Map;
            
        }
        public bool SetSubscription(string Channel,string UKey) 
        {
            SubscriptionMap nmap = new SubscriptionMap() {Channel=Channel,UKey=UKey};
            if(GetAllSubscriptions().Exists(map=>map.UKey.Equals(UKey) && map.Channel.Equals(Channel)))
            {
                return true;
            }
            else
            {
                return RedisServerChat.redisClient.Set("SubscriptionMap",nmap);
            }

        }

    }
    
}