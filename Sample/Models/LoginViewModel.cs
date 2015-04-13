using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sample.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        [Display(Name = "密碼")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "驗證碼")]
        public string Captcha { get; set; }
    }
}