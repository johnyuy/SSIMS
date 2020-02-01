using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSIMS.ViewModels
{
    public class UserLogin
    {
        [Display(Name ="Username")]
        [Required(ErrorMessage ="Enter Username")]
        [StringLength(30,MinimumLength =3,ErrorMessage ="Username 3-30 characters")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter Password")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Username 3-30 characters")]
        public string Password { get; set; }

        public UserLogin() { }

        public UserLogin(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}