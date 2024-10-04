using _123Vendas.Application.Dtos;
using _123Vendas.Application.Services;
using _123Vendas.Domain.Entities;
using _123Vendas.Domain.Events;
using _123Vendas.Domain.Repositories;
using Bogus;
using FluentAssertions;
using NSubstitute;

namespace _123Vendas.UnitTests.Services;

public class SaleServiceTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly SaleService _saleService;

    public SaleServiceTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _eventPublisher = Substitute.For<IEventPublisher>();
        _saleService = new SaleService(_saleRepository, _eventPublisher);
    }

    [Fact]
    public async Task CreateSaleAsync_Should_Create_Sale_And_Publish_CompraCriada_Event()
    {
        // Arrange
        var saleDto = new Faker<SaleCreateDto>()
            .RuleFor(s => s.CustomerId, Guid.NewGuid)
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.BranchId, Guid.NewGuid)
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => [
                new()
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = f.Commerce.ProductName(),
                    Quantity = 2,
                    UnitPrice = Convert.ToDecimal(f.Commerce.Price()),
                    Discount = 0
                }
            ])
            .Generate();

        // Act
        var createdSale = await _saleService.CreateSaleAsync(saleDto);

        // Assert
        await _saleRepository.Received(1).AddAsync(Arg.Is<Sale>(s => s.CustomerId == saleDto.CustomerId));
        _eventPublisher.Received(1).Publish(Arg.Is<CompraCriada>(e => e.SaleId == createdSale.Id));
        createdSale.Should().NotBeNull();
        createdSale.CustomerId.Should().Be(saleDto.CustomerId);
    }

    [Fact]
    public async Task GetSaleByIdAsync_Should_Return_Sale_If_Found()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var sale = new Sale(customerId, "CustomerName", branchId, "BranchName");

        _saleRepository.GetByIdAsync(sale.Id).Returns(sale);

        // Act
        var result = await _saleService.GetSaleByIdAsync(sale.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(sale.Id);
    }

    [Fact]
    public async Task GetSaleByIdAsync_Should_Return_Null_If_Not_Found()
    {
        // Arrange
        var saleId = Guid.NewGuid();

        _saleRepository.GetByIdAsync(saleId).Returns(null as Sale);

        // Act
        var result = await _saleService.GetSaleByIdAsync(saleId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateSaleAsync_Should_Update_Sale_And_Publish_CompraAlterada_Event()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var sale = new Sale(customerId, "CustomerName", branchId, "BranchName");
        var saleUpdateDto = new SaleUpdateDto { CustomerId = Guid.NewGuid(), CustomerName = "Updated Name", BranchId = Guid.NewGuid(), BranchName = "Updated Branch" };

        _saleRepository.GetByIdAsync(sale.Id).Returns(sale);

        // Act
        var result = await _saleService.UpdateSaleAsync(sale.Id, saleUpdateDto);

        // Assert
        result.Should().BeTrue();
        sale.CustomerName.Should().Be(saleUpdateDto.CustomerName);
        _eventPublisher.Received(1).Publish(Arg.Is<CompraAlterada>(e => e.SaleId == sale.Id));
    }

    [Fact]
    public async Task UpdateSaleAsync_Should_Return_False_If_Sale_Not_Found()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var saleUpdateDto = new SaleUpdateDto { CustomerId = Guid.NewGuid(), CustomerName = "Updated Name" };

        _saleRepository.GetByIdAsync(saleId).Returns((Sale)null);

        // Act
        var result = await _saleService.UpdateSaleAsync(saleId, saleUpdateDto);

        // Assert
        result.Should().BeFalse();
        _eventPublisher.DidNotReceive().Publish(Arg.Any<CompraAlterada>());
    }

    [Fact]
    public async Task CancelItemAsync_Should_Cancel_Item_And_Publish_ItemCancelado_Event()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var sale = new Sale(customerId, "CustomerName", branchId, "BranchName");
        sale.AddItem(productId, "Product", 2, 100, 0);
        var itemId = sale.Items.FirstOrDefault().Id;


        _saleRepository.GetByIdAsync(sale.Id).Returns(sale);

        // Act
        await _saleService.CancelItemAsync(sale.Id, itemId);

        // Assert
        var item = sale.Items.First(i => i.Id == itemId);
        item.IsCanceled.Should().BeTrue();
        _eventPublisher.Received(1).Publish(Arg.Is<ItemCancelado>(e => e.ItemId == itemId && e.SaleId == sale.Id));
    }

    [Fact]
    public async Task CancelItemAsync_Should_Throw_Exception_If_Item_Not_Found()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var sale = new Sale(customerId, "CustomerName", branchId, "BranchName");

        _saleRepository.GetByIdAsync(sale.Id).Returns(sale);

        // Act
        Func<Task> act = async () => await _saleService.CancelItemAsync(sale.Id, Guid.NewGuid());

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Sale item not found.");
    }

    [Fact]
    public async Task GetAllSalesAsync_ShouldReturnAllSales()
    {
        // Arrange
        var sales = new List<Sale>
            {
                new(Guid.NewGuid(), "Customer A", Guid.NewGuid(), "Branch A"),
                new(Guid.NewGuid(), "Customer B", Guid.NewGuid(), "Branch B"),
            };

        _saleRepository.GetAllAsync().Returns(sales);

        // Act
        var result = await _saleService.GetAllSalesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(sales);
    }
}

