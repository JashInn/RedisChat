using RedisRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;

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
                return Json.Encode("NOTFOUND");
            }
            else
            {
                return Json.Encode(MyChannel);
            }
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
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