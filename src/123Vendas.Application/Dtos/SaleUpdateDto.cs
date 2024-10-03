namespace _123Vendas.Application.Dtos;

public class SaleUpdateDto
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; }
}
