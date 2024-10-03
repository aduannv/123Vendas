using _123Vendas.Application.Dtos;
using _123Vendas.Domain.Entities;
using _123Vendas.Domain.Repositories;

namespace _123Vendas.Application.Services;

public class SaleService
{
    private readonly ISaleRepository _saleRepository;

    public SaleService(ISaleRepository saleRepository) => _saleRepository = saleRepository;

    public async Task<Sale> CreateSaleAsync(SaleCreateDto saleDto)
    {
        var sale = new Sale(
            saleDto.CustomerId,
            saleDto.CustomerName,
            saleDto.BranchId,
            saleDto.BranchName
        );

        foreach (var item in saleDto.Items)
        {
            sale.AddItem(
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.Discount
            );
        }

        await _saleRepository.AddAsync(sale);
        return sale;
    }

    public async Task<Sale> GetSaleByIdAsync(Guid saleId) => await _saleRepository.GetByIdAsync(saleId);

    public async Task<IEnumerable<Sale>> GetAllSalesAsync() => await _saleRepository.GetAllAsync();

    public async Task<bool> UpdateSaleAsync(Guid saleId, SaleUpdateDto saleUpdateDto)
    {
        var sale = await GetSaleByIdAsync(saleId);
        if (sale == null) return false;

        sale.UpdateDetails(saleUpdateDto.CustomerId, saleUpdateDto.CustomerName, saleUpdateDto.BranchId, saleUpdateDto.BranchName);

        await _saleRepository.UpdateAsync(sale);
        return true;
    }

    public async Task<bool> DeleteSaleAsync(Guid saleId)
    {
        var sale = await GetSaleByIdAsync(saleId);
        if (sale == null) return false;

        await _saleRepository.DeleteAsync(sale);
        return true;
    }
}
