using AutoMapper;
using CodeWord.Application.Commands;

namespace CodeWord.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CompetitionEntryDTO, SubmitCompetitionEntry.Command>();
        }
    }
}
