namespace SaleFlow.Domain.Responses
{
    public class ErrorResponse : ResponseBase
    {
        /// <summary>
        /// A machine-readable error type identifier.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence of the problem.
        /// </summary>
        public string Detail { get; set; }

        public ErrorResponse(string type, string error, string detail)
        {
            Success = false;
            Type = type;
            ErrorMessage = error;
            Detail = detail;
        }
    }
}
