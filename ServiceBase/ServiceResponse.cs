using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR44.VendingMachine.ServiceBase
{
    public class ServiceResponse
    {
        public readonly bool Success;
        public readonly string? Message;

        public ServiceResponse()
        {
            Success = true;
            Message = null;
        }

        public ServiceResponse(string message)
        {
            Success = false;
            Message = message;
        }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public readonly T? ReturnValue;

        public ServiceResponse(T returnValue) : base()
        {
            ReturnValue = returnValue;
        }

        public ServiceResponse(string message) : base(message)
        {
            ReturnValue = default(T);
        }

    }
}
