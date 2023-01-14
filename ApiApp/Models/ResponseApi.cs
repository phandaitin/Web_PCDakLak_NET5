using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    public class ResponseApi
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public object Data { get; set; }
            

    }
}
