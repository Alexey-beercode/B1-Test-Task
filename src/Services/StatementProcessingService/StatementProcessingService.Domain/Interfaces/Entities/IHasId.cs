﻿namespace StatementProcessingService.Domain.Interfaces.Entities;

public interface IHasId
{
    Guid Id { get; set; }
}