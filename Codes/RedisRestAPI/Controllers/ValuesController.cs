using RedisRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using Newtonsoft.Json;
namespace RedisRestAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(string Ukey)
        {
            Common com = new Common();
            string MyChannel = com.GetAllSubscriptions().Where(sub => sub.UKey.Equals(Ukey)).Select(sub => sub.Channel).ToList().FirstOrDefault();
            if (string.IsNullOrEmpty(MyChannel))
            {
                return "NOTFOUND";
            }
            else
            {
                return MyChannel;
            }
        }
        public string Get(string ChannelName,string Ukey) 
        {
            Common com = new Common();
            return JsonConvert.SerializeObject(MessageModel.Messages.Where(m=>m.ForChannel==ChannelName).ToList());
        }
        // POST api/values
        public void Post([FromBody]string value)
        {
            Common com = new Common();
            SubscriptionMap map = System.Web.Helpers.Json.Decode(value, typeof(SubscriptionMap));
            if (map != null) 
            {
                com.SetSubscription(map.Channel, map.UKey);
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}