using _123Vendas.Application.Dtos;
using _123Vendas.Domain.Entities;

namespace _123Vendas.Application.Services
{
    public interface ISaleService
    {
        Task<Sale> CreateSaleAsync(SaleCreateDto saleDto);
        Task<bool> DeleteSaleAsync(Guid saleId);
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task<Sale> GetSaleByIdAsync(Guid saleId);
        Task<bool> UpdateSaleAsync(Guid saleId, SaleUpdateDto saleUpdateDto);
    }
}