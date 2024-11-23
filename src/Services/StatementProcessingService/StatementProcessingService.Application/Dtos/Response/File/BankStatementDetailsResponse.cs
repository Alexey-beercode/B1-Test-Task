using System;
using System.Collections.Generic;
using StatementProcessingService.Application.Dtos.Response.Entries;

namespace StatementProcessingService.Application.Dtos.Response.File;

public class BankStatementDetailsResponse
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public DateTime UploadDate { get; set; }
    public List<BankStatementEntryResponse> Entries { get; set; }
}