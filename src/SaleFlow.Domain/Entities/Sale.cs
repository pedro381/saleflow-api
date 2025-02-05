using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SaleFlow.Domain.Entities
{
    public class Sale
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleId { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        [Required]
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = new Customer();
        [Required]
        public string Branch { get; set; } = string.Empty;
        public IEnumerable<SaleItem> SaleItems { get; set; } = Enumerable.Empty<SaleItem>();
        public bool IsCancelled { get; set; }

        // Calculate the total amount based on the sale items.
        public decimal TotalAmount => _saleItems.Sum(item => item.TotalItemAmount);

        private readonly List<SaleItem> _saleItems = new List<SaleItem>();

        public Sale()
        {
        }

        public Sale(string saleNumber, DateTime saleDate, Customer customer, string branch)
        {
            if (string.IsNullOrWhiteSpace(saleNumber))
                throw new ArgumentException("SaleNumber cannot be null or empty.", nameof(saleNumber));
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));
            if (string.IsNullOrWhiteSpace(branch))
                throw new ArgumentException("Branch cannot be null or empty.", nameof(branch));

            SaleNumber = saleNumber;
            SaleDate = saleDate;
            Customer = customer;
            Branch = branch;
        }

        /// <summary>
        /// Adds a sale item to the sale.
        /// </summary>
        /// <param name="saleItem">The sale item to add.</param>
        public void AddSaleItem(SaleItem saleItem)
        {
            if (saleItem == null)
                throw new ArgumentNullException(nameof(saleItem));

            // The SaleItem constructor already enforces the maximum quantity per product.
            _saleItems.Add(saleItem);
        }

        /// <summary>
        /// Cancels the entire sale and marks all sale items as cancelled.
        /// </summary>
        public void CancelSale()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Sale is already cancelled.");

            IsCancelled = true;

            foreach (var item in _saleItems)
            {
                item.Cancel();
            }
        }
    }
}
