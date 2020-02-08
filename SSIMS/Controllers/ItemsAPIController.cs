using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http;
using SSIMS.DAL;
using SSIMS.Models;
using SSIMS.ViewModels;
using SSIMS.Service;
using Newtonsoft.Json;


namespace SSIMS.Controllers
{
    public class ItemsAPIController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        //POST: api/ItemsAPI
        [HttpGet]
        public string Get()
        {

            List<Item> items = unitOfWork.ItemRepository.Get().ToList();

            return JsonConvert.SerializeObject(items);
        }



    }
}
