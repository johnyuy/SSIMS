using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSIMS.DAL;
using SSIMS.Models;
using SSIMS.ViewModels;
using SSIMS.Service;
using Newtonsoft.Json;

namespace SSIMS.Controllers
{
    public class LoginApiController : ApiController
    {
        UnitOfWork uow = new UnitOfWork();
        readonly ILoginService LoginService = new LoginService();

        //Session States should be done in the andriod application because of RESTful protocol
        //In Essense the controller is only to read and write from the database 

        //POST: api/LoginApi
        [HttpPost]
        public UserAccount Authenticate([FromBody] UserLogin userLogin)

        {
            if (LoginService.VerifyPasswordApi(userLogin.Username, userLogin.Password))
            {
                UserAccount account = uow.UserAccountRepository.GetByID(userLogin.Username);
                account.Password = "";
                return account;
            }

            return null;
        }

        [HttpGet]
        public Staff GetStaff([FromBody] string username)
        {
            if (username != "" )
            {
                Staff staff = uow.StaffRepository.Get(filter: x=> x.UserAccountID == username).FirstOrDefault();
                return staff;
            }
            return null;
        }


    }
}
