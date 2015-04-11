using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Case518Sample150411.Models.ViewModels
{

    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ValidationCode { get; set; }
        public UserGender Gender { get; set; }
        public bool Subscribe { get; set; }
    }
}