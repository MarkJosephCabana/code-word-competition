using Microsoft.Extensions.Logging;

namespace CodeWord.Application.Queries
{
    public static class GetCompetition
    {
        #region Command/Query

        public record Query(Guid CompetitionGUID) : IRequest<GetCompetitionResponse> { }

        #endregion

        #region Validation

        public class GetCompetitionsRequestValidation : AbstractValidator<Query>
        {
            public GetCompetitionsRequestValidation(ICompetitionRepository competitionRepository)
            {
                RuleFor(x => x.CompetitionGUID).NotNull().NotEmpty().NotEqual(Guid.Empty).WithMessage("Invalid GUID");
                RuleFor(x => x.CompetitionGUID).MustAsync(async (guid, token) => {
                    return await competitionRepository.CompetitionExists(guid, token);
                }).WithMessage("Competition not found");
            }
        }

        #endregion

        #region Handler

        public class Handler : IRequestHandler<Query, GetCompetitionResponse>
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

            public async Task<GetCompetitionResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var competition = await _competitionRepository.GetCompetition(request.CompetitionGUID, cancellationToken);
                    return _mapper.Map<GetCompetitionResponse>(competition);
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

        public class GetCompetitionResponse : IMapFrom<Competition>
        {
            public int Id { get; set; }
            public Guid CompetitionGUID { get; set; }
            public string CompetitionName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public IEnumerable<CompetitionRoundEntry> Rounds { get; set; } = new List<CompetitionRoundEntry>();

            public void Mapping(Profile profile)
            {
                profile.CreateMap<Competition, GetCompetitionResponse>()
                    .ForMember(x => x.Id, x => x.MapFrom(c => c.Id))
                    .ForMember(x => x.CompetitionGUID, x => x.MapFrom(c => c.CompetitionGUID))
                    .ForMember(x => x.CompetitionName, x => x.MapFrom(c => $"CodeWord #{c.Id} ({c.CompetitionDates.StartDate.ToString("dd-MMM-yyyy")} - {c.CompetitionDates.EndDate.ToString("dd-MMM-yyyy")})"))
                    .ForMember(x => x.StartDate, x => x.MapFrom(c => c.CompetitionDates.StartDate))
                    .ForMember(x => x.EndDate, x => x.MapFrom(c => c.CompetitionDates.EndDate))
                    .ForMember(x => x.Rounds, x => x.MapFrom(c => c.CompetitionRounds));
            }
        }

        public class CompetitionRoundEntry : IMapFrom<CompetitionRound>
        {
            public Guid CompetitionRoundGUID { get; set; }
            public DateTime Date { get; set; }
            public string CodeWord { get; set; }
        }


        #endregion
    }
}
