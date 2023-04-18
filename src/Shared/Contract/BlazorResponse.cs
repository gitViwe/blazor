using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contract
{
    public class BlazorResponse : IBlazorResponse
    {
        public string Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
