namespace _123Vendas.Domain.Entities;

public class SaleItem(Guid saleId, Guid productId, string productName, int quantity, decimal unitPrice, decimal discount)
{
    public Guid Id { get; set; }
    public Guid SaleId { get; private set; } = saleId;
    public Guid ProductId { get; private set; } = productId;
    public string ProductName { get; private set; } = productName;
    public int Quantity { get; private set; } = quantity;
    public decimal UnitPrice { get; private set; } = unitPrice;
    public decimal Discount { get; private set; } = discount;
    public bool IsCanceled { get; private set; } = false;
    public decimal TotalAmount => (UnitPrice - Discount) * Quantity;

    public void CancelItem()
    {
        IsCanceled = true;
    }
}
