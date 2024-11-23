using AutoMapper;
using StatementProcessingService.Application.Dtos.Response.File;
using StatementProcessingService.Domain.Entities;

namespace StatementProcessingService.Application.MappingProfiles;

public class BankStatementFileProfile : Profile
{
    public BankStatementFileProfile()
    {
        CreateMap<BankStatementFile, BankStatementDetailsResponse>()
            .ForMember(dest => dest.Entries, opt => opt.Ignore()); 
        
        CreateMap<BankStatementFile, BankStatementListItemResponse>()
            .ForMember(dest => dest.EntriesCount, opt => opt.MapFrom(src => src.Entries != null ? src.Entries.Count() : 0));
    }
}