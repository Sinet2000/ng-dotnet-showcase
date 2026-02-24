namespace Webstore.API.Domain.Entities;

public sealed class CatalogItem
{
    public int CatalogId { get; set; }
    public Catalog Catalog { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int SortOrder { get; set; }
    public DateTimeOffset AddedAt { get; set; }
}
