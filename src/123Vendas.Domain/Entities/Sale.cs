namespace _123Vendas.Domain.Entities;

public class Sale
{
    public Guid Id { get; private set; }
    public DateTime SaleDate { get; private set; }
    public Guid CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public Guid BranchId { get; private set; }
    public string BranchName { get; private set; }
    public decimal TotalAmount =>  Items.Sum(item => item.TotalAmount);
    public bool IsCanceled { get; private set; }

    public List<SaleItem> Items { get; private set; } = new List<SaleItem>();

    public Sale(Guid customerId, string customerName, Guid branchId, string branchName)
    {
        Id = Guid.NewGuid();
        SaleDate = DateTime.UtcNow;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
        IsCanceled = false;
    }

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
