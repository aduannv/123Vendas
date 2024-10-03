using _123Vendas.Application.Dtos;
using _123Vendas.Application.Services;
using _123Vendas.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace _123Vendas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly SaleService _saleService;

    public SalesController(SaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] SaleCreateDto saleDto)
    {
        var sale = await _saleService.CreateSaleAsync(saleDto);
        return CreatedAtAction(nameof(GetSaleById), new { id = sale.Id }, sale);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSaleById(Guid id)
    {
        var sale = await _saleService.GetSaleByIdAsync(id);
        if (sale == null) return NotFound();
        return Ok(sale);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sale>>> GetAllSales()
    {
        var sales = await _saleService.GetAllSalesAsync();
        return Ok(sales);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(Guid id)
    {
        var result = await _saleService.DeleteSaleAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSale(Guid id, [FromBody] SaleUpdateDto saleUpdateDto)
    {
        var result = await _saleService.UpdateSaleAsync(id, saleUpdateDto);
        if (!result) return NotFound();
        return NoContent();
    }
}