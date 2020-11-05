using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Application.Core.ResponseModel
{
    public class ListResult<T> : BaseResult<IEnumerable<T>> where T : class
    {
        public ListResult(ResponseResultCode code, string message, IEnumerable<T> data) : base(code, message, data)
        {
        }
    }
}
