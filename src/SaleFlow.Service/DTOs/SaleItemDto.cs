namespace SaleFlow.Service.DTOs
{
    public class SaleItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        // Discount is calculated on the domain side; you may include it if required.
        public decimal Discount { get; set; }
        public decimal TotalItemAmount { get; set; }
        public bool IsCancelled { get; set; }
    }
}
