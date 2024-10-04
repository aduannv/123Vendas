using _123Vendas.Application.Dtos;
using _123Vendas.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace _123Vendas.Api.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesController(ISaleService saleService, ILogger<SalesController> logger) : ControllerBase
    {
        private readonly ISaleService _saleService = saleService;
        private readonly ILogger<SalesController> _logger = logger;

        /// <summary>
        /// Obtém todas as vendas.
        /// </summary>
        /// <response code="200">Lista de vendas retornada com sucesso.</response>
        /// <response code="404">Nenhuma venda encontrada.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            _logger.LogInformation("Getting all sales.");
            try
            {
                var sales = await _saleService.GetAllSalesAsync();
                if (sales == null || !sales.Any())
                {
                    _logger.LogWarning("No sales found.");
                    return NotFound("No sales found.");
                }

                _logger.LogInformation("Successfully retrieved {Count} sales.", sales.Count());
                return Ok(sales);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting sales.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Obtém uma venda específica pelo ID.
        /// </summary>
        /// <param name="id">O ID da venda a ser obtida.</param>
        /// <response code="200">Venda encontrada com sucesso.</response>
        /// <response code="404">Venda não encontrada.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(Guid id)
        {
            _logger.LogInformation("Getting sale with ID {Id}.", id);
            try
            {
                var sale = await _saleService.GetSaleByIdAsync(id);
                if (sale == null)
                {
                    _logger.LogWarning("Sale with ID {Id} not found.", id);
                    return NotFound($"Sale with ID {id} not found.");
                }

                _logger.LogInformation("Successfully retrieved sale with ID {Id}.", id);
                return Ok(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the sale with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Cria uma nova venda.
        /// </summary>
        /// <param name="sale">Os dados da venda a serem criados.</param>
        /// <response code="201">Venda criada com sucesso.</response>
        /// <response code="400">Dados da venda inválidos.</response>
        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] SaleCreateDto sale)
        {
            _logger.LogInformation("Creating a new sale.");
            try
            {
                if (sale == null)
                {
                    _logger.LogWarning("Sale object is null.");
                    return BadRequest("Sale object is null.");
                }

                var createdSale = await _saleService.CreateSaleAsync(sale);
                _logger.LogInformation("Successfully created sale with ID {Id}.", createdSale.Id);
                return CreatedAtAction(nameof(GetSaleById), new { id = createdSale.Id }, createdSale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the sale.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Atualiza uma venda existente.
        /// </summary>
        /// <param name="id">O ID da venda a ser atualizada.</param>
        /// <param name="sale">Os novos dados da venda.</param>
        /// <response code="204">Venda atualizada com sucesso.</response>
        /// <response code="400">Dados da venda inválidos.</response>
        /// <response code="404">Venda não encontrada.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(Guid id, [FromBody] SaleUpdateDto sale)
        {
            _logger.LogInformation("Updating sale with ID {Id}.", id);
            try
            {
                if (sale == null)
                {
                    _logger.LogWarning("Sale object is null.");
                    return BadRequest("Sale object is null.");
                }

                var existingSale = await _saleService.GetSaleByIdAsync(id);
                if (existingSale == null)
                {
                    _logger.LogWarning("Sale with ID {Id} not found for update.", id);
                    return NotFound($"Sale with ID {id} not found.");
                }

                await _saleService.UpdateSaleAsync(id, sale);
                _logger.LogInformation("Successfully updated sale with ID {Id}.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the sale with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Cancela uma venda específica pelo ID.
        /// </summary>
        /// <param name="id">O ID da venda a ser excluída.</param>
        /// <response code="204">Venda excluída com sucesso.</response>
        /// <response code="404">Venda não encontrada.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            _logger.LogInformation("Canceling sale with ID {Id}.", id);
            try
            {
                var existingSale = await _saleService.GetSaleByIdAsync(id);
                if (existingSale == null)
                {
                    _logger.LogWarning("Sale with ID {Id} not found for cancel.", id);
                    return NotFound($"Sale with ID {id} not found.");
                }

                await _saleService.DeleteSaleAsync(id);
                _logger.LogInformation("Successfully cancel sale with ID {Id}.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the sale with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Cancela um item em uma venda.
        /// </summary>
        /// <param name="saleId">O ID da venda.</param>
        /// <param name="itemId">O ID do item na venda.</param>
        /// <response code="204">Item cancelado com sucesso.</response>
        /// <response code="404">Venda ou item não encontrados.</response>
        [HttpPatch("{saleId}/items/{itemId}/cancel")]
        public async Task<IActionResult> CancelItem(Guid saleId, Guid itemId)
        {
            _logger.LogInformation("Canceling sale item with ID {ItemId}.", itemId);
            try
            {
                await _saleService.CancelItemAsync(saleId, itemId);
                _logger.LogInformation("Successfully cancel sale item with ID {ItemId}.", itemId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while cancel the saleitem with ID {ItemId}.", itemId);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
