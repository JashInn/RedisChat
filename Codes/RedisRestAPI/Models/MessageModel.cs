using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedisRestAPI.Models
{
    public static class MessageModel
    {
        static MessageModel()
        {
            Subscriptions = string.Empty;
            Messages = new List<Message>();
        }
        public static string Subscriptions { get; set; }
        public static List<Message> Messages { get; set; }
    }
    public class Message
    {
        public Message()
        {
            MessageContent = string.Empty;
            ForChannel = string.Empty;
        }
        public string MessageContent { get; set; }
        public string ForChannel { get; set; }
    }
    public static class RedisServerChat
    {
        static RedisServerChat()
        {
            redisClient = new RedisClient();
        }
        public static RedisClient redisClient { get; set; }
    }
    public class SubscriptionMap 
    {
        public string UKey { get; set; }
        public string Channel { get; set; }
    }
}