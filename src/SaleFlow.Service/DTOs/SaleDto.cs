namespace SaleFlow.Service.DTOs
{
    public class SaleDto
    {
        public string SaleNumber { get; set; } = default!;
        public DateTime SaleDate { get; set; }
        public string CustomerExternalId { get; set; } = default!;
        public string CustomerName { get; set; } = default!;
        public string Branch { get; set; } = default!;
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public List<SaleItemDto> SaleItems { get; set; } = new();
    }
}
