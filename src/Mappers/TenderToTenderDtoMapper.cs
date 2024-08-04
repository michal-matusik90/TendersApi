using TendersApi.Models;

namespace TendersApi.Mappers;

public static class TenderToTenderDtoMapper
{
    public static Models.TenderDto Map(Context.Models.Tender tender) => new()
    {
        Id = tender.Id,
        Date = tender.Date.ToString("yyyy-MM-dd"),
        Description = tender.Description,
        Title = tender.Title,
        ValueEur = tender.ValueEur,
        Suppliers = tender.Suppliers.Select(s => new SupplierDto { Name = s.Name, Id = s.Id }).ToList()
    };
}
