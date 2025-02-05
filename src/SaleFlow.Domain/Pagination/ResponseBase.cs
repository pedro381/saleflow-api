namespace SaleFlow.Domain.Responses
{
    public class ResponseBase
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// An error message describing what went wrong if Success is false.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
