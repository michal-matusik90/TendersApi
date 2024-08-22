using TendersApi.Application.DTOs;
using TendersApi.Domain.Entities;

namespace TendersApi.Application.Mappers;

public static class TenderToTenderDtoMapper
{
    public static TenderDto Map(Tender tender) => new()
    {
        Id = tender.Id,
        Date = tender.Date.ToString("yyyy-MM-dd"),
        Description = tender.Description,
        Title = tender.Title,
        ValueEur = tender.ValueEur,
        Suppliers = tender.TenderSuppliers.Select(s => new SupplierDto { Name = s.Supplier.Name, Id = s.SupplierId }).ToList()
    };
}
