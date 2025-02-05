using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleFlow.Domain.Entities
{
    public class SaleItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Discount represented as a decimal fraction (0, 0.10, or 0.20).
        /// </summary>
        public decimal Discount { get; set; }
        public decimal TotalItemAmount { get; set; }
        public bool IsCancelled { get; set; }

        public SaleItem()
        {
        }

        public SaleItem(int productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            if (quantity > 20)
                throw new ArgumentException("Quantity cannot exceed 20 items per product.", nameof(quantity));

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = CalculateDiscount(quantity);
            TotalItemAmount = CalculateTotalItemAmount();
        }

        private decimal CalculateDiscount(int quantity)
        {
            if (quantity < 4)
                return 0m;
            if (quantity < 10)
                return 0.10m;
            if (quantity <= 20)
                return 0.20m;

            // This should never be reached because quantity > 20 is already prevented.
            return 0m;
        }

        private decimal CalculateTotalItemAmount()
        {
            decimal grossAmount = UnitPrice * Quantity;
            decimal discountAmount = grossAmount * Discount;
            return grossAmount - discountAmount;
        }

        /// <summary>
        /// Cancels the sale item.
        /// </summary>
        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}
