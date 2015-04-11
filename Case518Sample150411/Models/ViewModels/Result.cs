using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Case518Sample150411.Models.ViewModels
{
    public class Result
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        public static Result CreateFailure(string message)
        {
            return new Result {Message = message, Success = false};
        }

        public static Result CreateSuccess(object data)
        {
            return new Result { Data = data , Success = true};
        }
    }
}