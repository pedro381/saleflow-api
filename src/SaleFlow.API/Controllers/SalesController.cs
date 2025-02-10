using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaleFlow.Service.Commands;
using SaleFlow.Service.DTOs;
using SaleFlow.Service.Queries;

namespace SaleFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SalesController> _logger;

        public SalesController(IMediator mediator, ILogger<SalesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of sales.
        /// </summary>
        /// <param name="_page">Page number (default: 1)</param>
        /// <param name="_size">Number of items per page (default: 10)</param>
        /// <param name="_order">Ordering criteria (e.g., "saleDate desc")</param>
        /// <returns>Paginated list of sales.</returns>
        [HttpGet]
        public async Task<IActionResult> GetSales(
            [FromQuery(Name = "_page")] int page = 1,
            [FromQuery(Name = "_size")] int size = 10,
            [FromQuery(Name = "_order")] string order = "asc")
        {
            try
            {
                // For demonstration, GetSalesQuery returns all sales.
                // You can extend the query with pagination, filtering, and ordering parameters.
                var query = new GetSalesQuery();
                query.PageNumber = page;
                query.PageSize = size;
                var (sales, total) = await _mediator.Send(query);

                var response = new
                {
                    data = sales,
                    total,
                    currentPage = page,
                    totalPages = (int)Math.Ceiling(total / (double)size)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sales.");
                return StatusCode(500, new { type = "ServerError", error = "An error occurred.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a specific sale by sale number.
        /// </summary>
        /// <param name="saleNumber">Sale number</param>
        /// <returns>Sale record</returns>
        [HttpGet("{saleNumber}")]
        public async Task<IActionResult> GetSaleByNumber(string saleNumber)
        {
            try
            {
                var query = new GetSaleByNumberQuery(saleNumber);
                var sale = await _mediator.Send(query);
                return Ok(sale);
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Sale not found.");
                return NotFound(new { type = "ResourceNotFound", error = knfEx.Message, detail = knfEx.InnerException?.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sale.");
                return StatusCode(500, new { type = "ServerError", error = "An error occurred.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new sale record.
        /// </summary>
        /// <param name="saleDto">Sale data</param>
        /// <returns>The created sale record</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] SaleDto saleDto)
        {
            try
            {
                var command = new CreateSaleCommand(saleDto);
                var createdSale = await _mediator.Send(command);

                // Simulate event logging for SaleCreated.
                _logger.LogInformation("SaleCreated: {SaleNumber}", createdSale.SaleNumber);

                return CreatedAtAction(nameof(GetSaleByNumber), new { saleNumber = createdSale.SaleNumber }, createdSale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sale.");
                return StatusCode(500, new { type = "ServerError", error = "An error occurred.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing sale record.
        /// </summary>
        /// <param name="saleNumber">Sale number</param>
        /// <param name="saleDto">Updated sale data</param>
        /// <returns>The updated sale record</returns>
        [HttpPut("{saleNumber}")]
        public async Task<IActionResult> UpdateSale(string saleNumber, [FromBody] SaleDto saleDto)
        {
            try
            {
                if (!saleNumber.Equals(saleDto.SaleNumber, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { type = "ValidationError", error = "Sale number in URL does not match body." });
                }

                var command = new UpdateSaleCommand(saleDto);
                var updatedSale = await _mediator.Send(command);

                // Simulate event logging for SaleModified.
                _logger.LogInformation("SaleModified: {SaleNumber}", updatedSale.SaleNumber);

                return Ok(updatedSale);
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Sale not found.");
                return NotFound(new { type = "ResourceNotFound", error = knfEx.Message, detail = knfEx.InnerException?.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sale.");
                return StatusCode(500, new { type = "ServerError", error = "An error occurred.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a specific sale record.
        /// </summary>
        /// <param name="saleNumber">Sale number</param>
        /// <returns>A success message</returns>
        [HttpDelete("{saleNumber}")]
        public async Task<IActionResult> DeleteSale(string saleNumber)
        {
            try
            {
                var command = new DeleteSaleCommand(saleNumber);
                var result = await _mediator.Send(command);

                // Simulate event logging for SaleCancelled (or ItemCancelled if applied per item).
                _logger.LogInformation("SaleCancelled: {SaleNumber}", saleNumber);

                return Ok(new { message = $"Sale {saleNumber} has been deleted." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sale.");
                return StatusCode(500, new { type = "ServerError", error = "An error occurred.", detail = ex.Message });
            }
        }
    }
}
