using Microsoft.Extensions.Logging;

namespace CodeWord.Application.Commands
{
    public static class SubmitCompetitionEntry
    {
        #region Command 

        public record Command : IRequest<CompetitionEntryResponse>
        {
            public Command()
            {

            }
            public Command(Guid CompetitionGUID, Guid CompetitionRoundGUID, string Answer, string FirstName, string LastName,
            string Email, string AddressLine1, string AddressLine2, string Suburb, string State, string PostCode,
            string PhoneNumber, bool OptIn)
            { }

            public Guid CompetitionGUID { get; set; } 
            public Guid CompetitionRoundGUID { get; set; }
            public string Answer { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string Suburb { get; set; }
            public string State { get; set; }
            public string PostCode { get; set; }
            public string PhoneNumber { get; set; }
            public bool OptIn { get; set; }
        }

        #endregion

        #region Validation

        public class SubmitCompetitionEntryValidation : AbstractValidator<Command>
        {
            public SubmitCompetitionEntryValidation(ICompetitionRepository competitionRepository)
            {
                RuleFor(x => x.CompetitionGUID).NotEmpty().NotNull().NotEqual(Guid.Empty).WithMessage("Invalid competition");
                RuleFor(x => x.CompetitionRoundGUID).NotEmpty().NotNull().NotEqual(Guid.Empty).WithMessage("Invalid competition round");
                RuleFor(x => x.CompetitionRoundGUID).MustAsync(async (command, competitionRoundGUID, token) => { 
                    return await competitionRepository.CompetitionRoundExists(command.CompetitionGUID, competitionRoundGUID, token);
                }).WithMessage("missing competition round");
                RuleFor(x => x.Answer).NotNull().NotEmpty().MaximumLength(25).WithMessage("Answer must contain 25 letters or less");
                RuleFor(x => x.Answer).MustAsync(async (command, answer, token) => {
                    return await competitionRepository.CompetitionRoundExists(command.CompetitionGUID, command.CompetitionRoundGUID, answer, token);
                }).WithMessage("Invalid competition code word");
                RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("Invalid first name");
                RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Invalid last name");
                RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid email");
                RuleFor(x => x.AddressLine1).NotNull().NotEmpty().WithMessage("Invalid address line 1");
                RuleFor(x => x.Suburb).NotNull().NotEmpty().WithMessage("Invalid suburb");
                RuleFor(x => x.State).NotNull().NotEmpty().WithMessage("Invalid state");
                RuleFor(x => x.PostCode).NotNull().NotEmpty().WithMessage("Invalid post code");
            }
        }

        #endregion

        #region Handler

        public class Handler : IRequestHandler<Command, CompetitionEntryResponse>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IUserRepository _userRepository;
            private readonly ICompetitionRepository _competitionRepository;
            private readonly IMapper _mapper;

            public Handler(ILogger<Handler> logger, IUserRepository userRepository, IMapper mapper, ICompetitionRepository competitionRepository)
            {
                _logger = logger;
                _userRepository = userRepository;
                _mapper = mapper;
                _competitionRepository = competitionRepository;
            }

            public async Task<CompetitionEntryResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var competition = await _competitionRepository.GetCompetition(request.CompetitionGUID, cancellationToken);
                    var userSubmission = new User(Guid.NewGuid(), competition.CompetitionRounds.First(cr => cr.CompetitionRoundGUID == request.CompetitionRoundGUID).Id);
                    
                    userSubmission.SetPersonalDetails(request.FirstName, request.LastName);
                    userSubmission.SetContactDetails(request.Email, request.PhoneNumber);
                    userSubmission.SetAddress(request.AddressLine1, request.AddressLine2, request.Suburb,
                        request.State, request.PostCode);
                    userSubmission.ToggleOptIn(request.OptIn);

                    userSubmission = await _userRepository.Add(userSubmission);

                    await _userRepository.UnitOfWork.SaveChanges(cancellationToken);

                    return _mapper.Map<CompetitionEntryResponse>(userSubmission);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{@request}", request);
                    throw;
                }
            }
        }

        #endregion

        #region Response

        public record CompetitionEntryResponse : IMapFrom<User>
        {
            public int Id { get; set; }
        }

        #endregion
    }
}
