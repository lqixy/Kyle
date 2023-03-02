using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Extensions.WebApi.Models
{
    public class AjaxResponse<TResult>
    {

        public bool Success { get; set; }

        public TResult Data { get; set; }

        public int Code { get; private set; }

        public string Message { get; private set; } = "成功";

        public void SetMessage(string message)
        {
            this.Message = message;
        }

        public AjaxResponse(TResult result)
        {
            Data = result;

            Success = true;
            Code = (int)HttpStatusCode.OK;
        }

        public AjaxResponse(TResult result, string message) : base()
        {
            Data = result;
            Message = message;
        }

        public AjaxResponse()
        {
            Success = true;
            Code = (int)HttpStatusCode.OK;
        }

        public AjaxResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
        public AjaxResponse(int code, string message, TResult data)
        {
            Code = code;
            Message = message;
            Data = data;
            Success = code == (int)HttpStatusCode.OK;
        }
    }

    public class AjaxResponse : AjaxResponse<object>
    {
        public AjaxResponse() : base() { }

        public AjaxResponse(int code, string message) : base(code, message) { }

        public AjaxResponse(int code, string message, object result) : base(code, message, result)
        {

        }

        public AjaxResponse(object result) : base(result) { }

        public AjaxResponse(object result, string message) : base(result, message) { }


    }
}
