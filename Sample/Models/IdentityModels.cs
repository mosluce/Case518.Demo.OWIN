using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sample.Models
{
    public enum Gender
    {
        Male,
        Female
    }

    public class ApplicationUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Captcha { get; set; }
        public bool Subscription { get; set; }
        public Gender Gender { get; set; }
        public bool Activated { get; set; }
        public string ActivationCode { get; set; }
        public DateTime? ActivatedTime { get; set; }
        public DateTime? RegisterTime { get; set; }
        public DateTime? Birthday { get; set; }
    }
}