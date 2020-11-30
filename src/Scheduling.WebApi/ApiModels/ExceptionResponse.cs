using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduling.WebApi.ApiModels
{
    public class ExceptionResponse
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
