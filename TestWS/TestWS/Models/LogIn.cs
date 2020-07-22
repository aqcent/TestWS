using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWS.Models
{
    public class LogIn
    {
        [MinLength(4, ErrorMessage ="Login >4 symb")]
        [Required]
        [DisplayName("login")]
        public string Login { get; set; }

        [MinLength(4, ErrorMessage = "Password >4 symb")]
        [Required]
        [DisplayName("password")]
        public string Password { get; set; }

    }
}