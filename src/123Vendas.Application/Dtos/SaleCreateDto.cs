namespace _123Vendas.Application.Dtos;

public class SaleCreateDto
{
    public DateTime SaleDate { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; }
    public List<SaleItemDto> Items { get; set; } = new List<SaleItemDto>();
}
