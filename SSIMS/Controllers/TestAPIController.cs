using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSIMS.DAL;

namespace SSIMS.Controllers
{

    public class TestAPIController : ApiController
    {
        UnitOfWork uow = new UnitOfWork();
        // GET: api/TestAPI/
        public ActionResult<IEnumerable<string>> Get()
        {
            string[] test = { "helloworld", "heeee" };

            return test;
        }
    }
}
