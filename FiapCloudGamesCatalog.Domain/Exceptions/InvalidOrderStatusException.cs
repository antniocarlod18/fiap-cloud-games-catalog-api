using FiapCloudGamesCatalog.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FiapCloudGamesCatalog.Domain.Exceptions;

[Serializable]
public class InvalidOrderStatusException : BaseException
{
    private static string _customMessage = "Oops! You can only complete orders that are waiting for payment. Your order is {0}!";
    public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.UnprocessableContent;
    public override LogLevel LogLevel { get; set; } = LogLevel.Warning;

    public InvalidOrderStatusException(OrderStatusEnum statusEnum) : base(string.Format(_customMessage, statusEnum.ToString()))
    {
    }
}