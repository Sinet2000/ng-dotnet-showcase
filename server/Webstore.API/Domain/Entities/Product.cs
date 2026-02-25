using System.ComponentModel.DataAnnotations;

namespace Webstore.API.Domain.Entities;

public sealed class Product : BaseEntity
{
    private readonly List<CatalogItem> _catalogItems = [];

    [MaxLength(32)]
    public required string Sku { get; set; }

    [MaxLength(256)]
    public required string Name { get; set; }

    public decimal Price { get; set; }

    public IReadOnlyCollection<CatalogItem> CatalogItems => _catalogItems.AsReadOnly();

    public void AddCatalogItem(CatalogItem catalogItem)
    {
        _catalogItems.Add(catalogItem);
        UpdateTimestamp();
    }
}
