namespace Webstore.API.Domain.Exceptions;

public class EntityNotFoundException(string entityName, string findByKey, object findByValue)
    : DomainException($"Entity '{entityName}' not found by {findByKey} = '{findByValue}'.");