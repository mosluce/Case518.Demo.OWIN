using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sample.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [PasswordPropertyText]
        [Display(Name = "密碼")]
        public string Password { get; set; }
        [DefaultValue(0)]
        [Display(Name = "性別")]
        public Gender Gender { get; set; }
        [DefaultValue(true)]
        [Display(Name = "訂閱電子報")]
        public bool Subscription { get; set; }
        [Required]
        [Display(Name = "驗證碼")]
        public string Captcha { get; set; }
        [Required]
        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }
    }
}