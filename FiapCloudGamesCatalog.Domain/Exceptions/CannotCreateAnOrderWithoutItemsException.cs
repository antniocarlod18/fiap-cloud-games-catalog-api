using Microsoft.Extensions.Logging;
using System.Net;

namespace FiapCloudGamesCatalog.Domain.Exceptions;

public class CannotCreateAnOrderWithoutItemsException : BaseException
{
    public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
    public override LogLevel LogLevel { get; set; } = LogLevel.Warning;

    public CannotCreateAnOrderWithoutItemsException() : base("Oops! You need to have at least one game in your cart to complete your order.")
    {
    }
}
