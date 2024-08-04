using System.Collections.Concurrent;
using System.Globalization;
using TendersApi.Models;

namespace TendersApi.Mappers;

public sealed class ExternalApiDataToTenderMapper
{
    private readonly ConcurrentDictionary<string, Context.Models.Supplier> _supplierCache = [];

    public Context.Models.Tender Map(Data data) => new()
    {
        Id = data.Id,
        Date = data.Date,
        Category = data.Category,
        Description = data.Description,
        Title = data.Title,
        Value = decimal.Parse(data.AwardedValue, CultureInfo.InvariantCulture),
        ValueEur = decimal.Parse(data.AwardedValueEur, CultureInfo.InvariantCulture),
        Suppliers = MapToSuppliers(data).ToList()
    };

    private IEnumerable<Context.Models.Supplier> MapToSuppliers(Data data)
    {
        foreach (var awarded in data.Awarded)
        {
            foreach (var externalSupplier in awarded.Suppliers)
            {
                var id = externalSupplier.Id.ToString();
                var supplier = new Context.Models.Supplier
                {
                    Id = externalSupplier.Id.ToString(),
                    Name = externalSupplier.Name,
                    Value = decimal.Parse(awarded.Value, CultureInfo.InvariantCulture),
                    ValueEur = awarded.ValueEur,
                };
                yield return _supplierCache.GetOrAdd(id, supplier);
            }
        }
    }
}
