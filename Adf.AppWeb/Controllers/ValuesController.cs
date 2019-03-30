using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Adf.Core.Entity;
using CYQ.Data.Table;
using Decoration.Service;

namespace Adf.AppWeb.Controllers
{
    public class TestUser
    {
        public String UserName { get; set; }

    }

    public class ValuesController : ApiController
    {
        // GET api/values
        
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Authorize]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic GetInfo(String userName)
        {
            TestUser testUser = new TestUser();

            testUser.UserName = userName;

            MDataRow drCompanyUser = DecorationService.Instance().CompanyUser().GetEntityWithUserCode(userName);

            //MDataTable dtInfo = new MDataTable();
            //dtInfo.ToJson(false, false);

            return drCompanyUser.ToJson(true);
        }


        // POST api/values
        public ExeMsgInfo Post([FromBody]dynamic value)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            exeMsgInfo.RetStatus = 100;
            exeMsgInfo.RetValue = value.UserName;

            return exeMsgInfo;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            //
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

    
}
