namespace TendersApi.Models;

public sealed class OrderByCriteria
{
    public string Field { get; set; } = default!;
    public string Direction { get; set; } = OrderDirection.Ascending.ToString();
    public int Index { get; set; } = 0;
}
