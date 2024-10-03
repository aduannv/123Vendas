﻿using _123Vendas.Application.Dtos;
using _123Vendas.Domain.Entities;
using _123Vendas.Domain.Events;
using _123Vendas.Domain.Repositories;

namespace _123Vendas.Application.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IEventPublisher _eventPublisher;

    public SaleService(ISaleRepository saleRepository, IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Sale> CreateSaleAsync(SaleCreateDto saleDto)
    {
        var sale = new Sale(
            saleDto.CustomerId,
            saleDto.CustomerName,
            saleDto.BranchId,
            saleDto.BranchName
        );

        foreach (var item in saleDto.Items)
        {
            sale.AddItem(
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.Discount
            );
        }

        await _saleRepository.AddAsync(sale);

        var eventToPublish = new CompraCriada { SaleId = sale.Id };
        _eventPublisher.Publish(eventToPublish);

        return sale;
    }

    public async Task<Sale> GetSaleByIdAsync(Guid saleId) => await _saleRepository.GetByIdAsync(saleId);

    public async Task<IEnumerable<Sale>> GetAllSalesAsync() => await _saleRepository.GetAllAsync();

    public async Task<bool> UpdateSaleAsync(Guid saleId, SaleUpdateDto saleUpdateDto)
    {
        var sale = await GetSaleByIdAsync(saleId);
        if (sale == null) return false;

        sale.UpdateDetails(saleUpdateDto.CustomerId, saleUpdateDto.CustomerName, saleUpdateDto.BranchId, saleUpdateDto.BranchName);

        await _saleRepository.UpdateAsync(sale);
        return true;
    }

    public async Task<bool> DeleteSaleAsync(Guid saleId)
    {
        var sale = await GetSaleByIdAsync(saleId);
        if (sale == null) return false;

        await _saleRepository.DeleteAsync(sale);
        return true;
    }
}
