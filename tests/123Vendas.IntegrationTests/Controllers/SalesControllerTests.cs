using _123Vendas.Application.Dtos;
using _123Vendas.Domain.Entities;
using _123Vendas.IntegrationTests.Setup;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace _123Vendas.IntegrationTests.Controllers;

public class SalesControllerTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetAllSales_ReturnsOk_WithListOfSales()
    {
        // Arrange
        var saleCreateDto = new SaleCreateDto
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            BranchId = Guid.NewGuid(),
            BranchName = "Branch XYZ",
            Items = new List<SaleItemDto>
            {
                new SaleItemDto
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product A",
                    Quantity = 2,
                    UnitPrice = 100.00m,
                    Discount = 10
                }
            }
        };

        await _client.PostAsJsonAsync("/api/sales", saleCreateDto);

        // Act
        var response = await _client.GetAsync("/api/sales");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var sales = await response.Content.ReadFromJsonAsync<List<SaleCreateDto>>();
        sales.Should().NotBeNull();
        sales.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetSaleById_ReturnsOk_WithSale()
    {
        // Arrange
        var saleCreateDto = new SaleCreateDto
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            BranchId = Guid.NewGuid(),
            BranchName = "Branch XYZ",
            Items = new List<SaleItemDto>
            {
                new SaleItemDto
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product A",
                    Quantity = 2,
                    UnitPrice = 100.00m,
                    Discount = 10
                }
            }
        };

        await _client.PostAsJsonAsync("/api/sales", saleCreateDto);
        var responseSales = await _client.GetAsync("/api/sales");
        var sales = await responseSales.Content.ReadFromJsonAsync<List<Sale>>();
        var saleId = sales.FirstOrDefault().Id;

        // Act
        var response = await _client.GetAsync($"/api/sales/{saleId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        var sale = JsonConvert.DeserializeObject<Sale>(responseContent);
        sale.Should().NotBeNull();
        sale.Id.Should().Be(saleId);
    }

    [Fact]
    public async Task CreateSale_ReturnsCreated()
    {
        // Arrange
        var saleDto = new SaleCreateDto
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "Test Customer",
            BranchId = Guid.NewGuid(),
            BranchName = "Test Branch",
            Items = new List<SaleItemDto>
                {
                    new SaleItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Test Product",
                        Quantity = 2,
                        UnitPrice = 50.00m,
                        Discount = 5.00m
                    }
                }
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(saleDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/sales", jsonContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadAsStringAsync();
        var createdSale = JsonConvert.DeserializeObject<Sale>(responseContent);
        createdSale.Should().NotBeNull();
        createdSale.CustomerName.Should().Be(saleDto.CustomerName);
    }

    [Fact]
    public async Task UpdateSale_ReturnsNoContent()
    {
        // Arrange
        var saleCreateDto = new SaleCreateDto
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            BranchId = Guid.NewGuid(),
            BranchName = "Branch XYZ",
            Items = new List<SaleItemDto>
            {
                new SaleItemDto
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product A",
                    Quantity = 2,
                    UnitPrice = 100.00m,
                    Discount = 10
                }
            }
        };

        await _client.PostAsJsonAsync("/api/sales", saleCreateDto);
        var responseSales = await _client.GetAsync("/api/sales");
        var sales = await responseSales.Content.ReadFromJsonAsync<List<Sale>>();
        var saleId = sales.FirstOrDefault().Id;
        var saleUpdateDto = new SaleUpdateDto
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "Updated Customer",
            BranchId = Guid.NewGuid(),
            BranchName = "Updated Branch"
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(saleUpdateDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/sales/{saleId}", jsonContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task CancelSale_ReturnsNoContent()
    {
        // Arrange
        var saleCreateDto = new SaleCreateDto
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            BranchId = Guid.NewGuid(),
            BranchName = "Branch XYZ",
            Items = new List<SaleItemDto>
            {
                new SaleItemDto
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product A",
                    Quantity = 2,
                    UnitPrice = 100.00m,
                    Discount = 10
                }
            }
        };

        await _client.PostAsJsonAsync("/api/sales", saleCreateDto);
        var responseSales = await _client.GetAsync("/api/sales");
        var sales = await responseSales.Content.ReadFromJsonAsync<List<Sale>>();
        var saleId = sales.FirstOrDefault().Id;

        // Act
        var response = await _client.DeleteAsync($"/api/sales/{saleId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task CancelItem_ReturnsNoContent()
    {
        // Arrange
        var saleCreateDto = new SaleCreateDto
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            BranchId = Guid.NewGuid(),
            BranchName = "Branch XYZ",
            Items = new List<SaleItemDto>
            {
                new SaleItemDto
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product A",
                    Quantity = 2,
                    UnitPrice = 100.00m,
                    Discount = 10
                }
            }
        };

        await _client.PostAsJsonAsync("/api/sales", saleCreateDto);
        var responseSales = await _client.GetAsync("/api/sales");
        var sales = await responseSales.Content.ReadFromJsonAsync<List<Sale>>();
        var saleId = sales.FirstOrDefault().Id;
        var itemId = sales.FirstOrDefault().Items.FirstOrDefault().Id;

        // Act
        var response = await _client.PatchAsync($"/api/sales/{saleId}/items/{itemId}/cancel", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
