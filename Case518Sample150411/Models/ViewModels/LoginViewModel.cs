﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Case518Sample150411.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ValidationCode { get; set; }
    }
}