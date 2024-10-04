namespace _123Vendas.Domain.Entities;

public class Sale(Guid customerId, string customerName, Guid branchId, string branchName)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime SaleDate { get; private set; } = DateTime.UtcNow;
    public Guid CustomerId { get; private set; } = customerId;
    public string CustomerName { get; private set; } = customerName;
    public Guid BranchId { get; private set; } = branchId;
    public string BranchName { get; private set; } = branchName;
    public decimal TotalAmount =>  Items
        .Where(item => !item.IsCanceled)
        .Sum(item => item.TotalAmount);

    public bool IsCanceled { get; private set; } = false;

    public List<SaleItem> Items { get; private set; } = new List<SaleItem>();

    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice, decimal discount)
    {
        var item = new SaleItem(Id, productId, productName, quantity, unitPrice, discount);
        Items.Add(item);
    }

    public void UpdateDetails(Guid customerId, string customerName, Guid branchId, string branchName)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
    }

    public void CancelSale()
    {
        IsCanceled = true;
    }
}
