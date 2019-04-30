using System;
using System.Collections.Generic;
using System.Text;

namespace DTL.Shared.Models
{
    public class ResponseModel
    {
        public string Status { get; set; }
        public Response Response { get; set; }
        public ResponseModel()
        {
            Response = new Response();
        }
    }

    public class Response
    {
        public List<int> Start { get; set; }
        public string[] Missing { get; set; }

        public Response()
        {
            Missing = new string[2];
        }
    }
}
