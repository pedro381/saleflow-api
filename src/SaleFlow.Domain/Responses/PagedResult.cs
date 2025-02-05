using System;
using System.Collections.Generic;

namespace SaleFlow.Domain.Pagination
{
    public class PagedResult<T>
    {
        /// <summary>
        /// The items for the current page.
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// The total number of items available.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// The current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// The total number of pages available.
        /// </summary>
        public int TotalPages { get; set; }

        public PagedResult(IEnumerable<T> data, int totalItems, int currentPage, int pageSize)
        {
            Data = data;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }
    }
}
