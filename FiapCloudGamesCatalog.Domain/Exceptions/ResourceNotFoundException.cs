using FiapCloudGamesCatalog.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FiapCloudGamesCatalog.Domain.Exceptions
{
    [Serializable]
    public class ResourceNotFoundException : BaseException
    {
        private static string _customMessage = "Oops! {0} not found or may have been removed.";
        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;
        public override LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public ResourceNotFoundException(string resourceName) : base(string.Format(_customMessage, resourceName))
        {
        }
    }
}