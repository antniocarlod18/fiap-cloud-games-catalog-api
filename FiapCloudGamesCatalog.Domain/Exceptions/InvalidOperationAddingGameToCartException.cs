using FiapCloudGamesCatalog.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FiapCloudGamesCatalog.Domain.Exceptions
{
    [Serializable]
    public class InvalidOperationAddingGameToCartException : BaseException
    {
        private static string _customMessage = "Oops! This game is already in your cart or library.";
        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Conflict;
        public override LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public InvalidOperationAddingGameToCartException() : base(_customMessage)
        {
        }
    }
}