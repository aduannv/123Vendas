using _123Vendas.Domain.Entities;
using _123Vendas.Domain.Repositories;
using _123Vendas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace _123Vendas.Infrastructure.Repositories;

public class SaleRepository(SalesDbContext context) : ISaleRepository
{
    private readonly SalesDbContext _context = context;

    public async Task<Sale> GetByIdAsync(Guid id)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Sale>> GetAllAsync() => await _context.Sales.Include(s => s.Items).ToListAsync();

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Sale sale)
    {
        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync();
    }
}
