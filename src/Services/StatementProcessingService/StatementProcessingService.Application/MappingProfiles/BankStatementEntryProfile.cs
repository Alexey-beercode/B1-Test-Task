using AutoMapper;
using StatementProcessingService.Application.Dtos.Response.Entries;
using StatementProcessingService.Domain.Entities;

namespace StatementProcessingService.Application.MappingProfiles;

public class BankStatementEntryProfile : Profile
{
    public BankStatementEntryProfile()
    {
        CreateMap<BankStatementEntry, BankStatementEntryResponse>().ReverseMap();
    }
}