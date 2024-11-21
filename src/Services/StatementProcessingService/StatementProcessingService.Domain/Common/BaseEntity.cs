using StatementProcessingService.Domain.Interfaces.Entities;

namespace StatementProcessingService.Domain.Common;

public class BaseEntity:IHasId
{
    public Guid Id { get; set; }
}