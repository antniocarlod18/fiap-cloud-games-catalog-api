using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FiapCloudGamesCatalog.Domain.Exceptions
{
    public class UserAlreadyHasAnOpenOrderException : BaseException
    {
        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Conflict;
        public override LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public UserAlreadyHasAnOpenOrderException() : base("Oops! An active order already exists for this user. Finish or cancel it to continue..")
        {
        }
    }
}
