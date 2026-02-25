using System.ComponentModel.DataAnnotations;

namespace Webstore.API.Domain.Entities;

public class Catalog : BaseEntity
{
    private readonly List<CatalogItem> _items = [];

    [MaxLength(256)]
    public required string Name { get; set; }

    public IReadOnlyCollection<CatalogItem> Items => _items.AsReadOnly();
}
