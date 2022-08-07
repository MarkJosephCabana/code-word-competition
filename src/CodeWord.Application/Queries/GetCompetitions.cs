using Microsoft.Extensions.Logging;

namespace CodeWord.Application.Queries
{
    public static class GetCompetitions
    {
        #region Command/Query

        public record Query(int pageIndex, int pageSize) : IRequest<GetCompetitionsResponse> { }

        #endregion

        #region Validation

        public class GetCompetitionsRequestValidation : AbstractValidator<Query>
        {
            public GetCompetitionsRequestValidation()
            {
                RuleFor(x => x.pageIndex).GreaterThanOrEqualTo(0);
                RuleFor(x => x.pageSize).GreaterThanOrEqualTo(1);
            }
        }

        #endregion

        #region Handler

        public class Handler : IRequestHandler<Query, GetCompetitionsResponse>
        {
            private readonly ILogger<Handler> _logger;
            private readonly ICompetitionRepository _competitionRepository;
            private readonly IMapper _mapper;

            public Handler(ILogger<Handler> logger, ICompetitionRepository competitionRepository, IMapper mapper)
            {
                _logger = logger;
                _competitionRepository = competitionRepository;
                _mapper = mapper;
            }

            public async Task<GetCompetitionsResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var competitions = await _competitionRepository.GetCompetitions(request.pageIndex, request.pageSize, cancellationToken);
                    return new GetCompetitionsResponse
                    {
                        Competitions = _mapper.Map<IEnumerable<CompetitionEntry>>(competitions)
                    };
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "{@request}", request);
                    throw;
                }
            }
        }

        #endregion

        #region Response Model

        public class GetCompetitionsResponse
        {
            public IEnumerable<CompetitionEntry> Competitions { get; set; } = new List<CompetitionEntry>();
        }

        public class CompetitionEntry : IMapFrom<Competition>
        {
            public int Id { get; set; }
            public Guid CompetitionGUID { get; set; }
            public string CompetitionName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public void Mapping(Profile profile)
            {
                profile.CreateMap<Competition, CompetitionEntry>()
                    .ForMember(x => x.Id, x => x.MapFrom(c => c.Id))
                    .ForMember(x => x.CompetitionGUID, x => x.MapFrom(c => c.CompetitionGUID))
                    .ForMember(x => x.CompetitionName, x => x.MapFrom(c => $"CodeWord #{c.Id} ({c.CompetitionDates.StartDate.ToString("dd-MMM-yyyy")} - {c.CompetitionDates.EndDate.ToString("dd-MMM-yyyy")})"))
                    .ForMember(x => x.StartDate, x => x.MapFrom(c => c.CompetitionDates.StartDate))
                    .ForMember(x => x.EndDate, x => x.MapFrom(c => c.CompetitionDates.EndDate));
            }
        }


        #endregion
    }
}
