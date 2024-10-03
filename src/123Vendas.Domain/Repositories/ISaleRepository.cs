using _123Vendas.Domain.Entities;

namespace _123Vendas.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> GetByIdAsync(Guid id);
    Task AddAsync(Sale sale);
    Task<IEnumerable<Sale>> GetAllAsync();
    Task UpdateAsync(Sale sale);
    Task DeleteAsync(Sale sale);
}
