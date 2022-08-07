using AutoMapper;
using CodeWord.API.Models;
using CodeWord.Application.Commands;
using CodeWord.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;

namespace CodeWord.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompetitionsController : ControllerBase
    {
       
        private readonly ILogger<CompetitionsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CompetitionsController(ILogger<CompetitionsController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{pageIndex:min(0)}/{pageSize:min(1)}")]
        public async Task<ActionResult> GetAllComptetitions([FromRoute]int pageIndex, [FromRoute]int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var competitions = await _mediator.Send(new GetCompetitions.Query(pageIndex, pageSize), cancellationToken);
                return Ok(competitions);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{pageIndex} {pageSize}", pageIndex, pageSize);
                throw;
            }
        }

        [HttpGet("{competitionGUID:guid}")]
        public async Task<ActionResult> GetCompetition([FromRoute] Guid competitionGUID, CancellationToken cancellationToken)
        {
            try
            {
                var competition = await _mediator.Send(new GetCompetition.Query(competitionGUID), cancellationToken);
                return Ok(competition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{competitionGUID}", competitionGUID);
                throw;
            }
        }


        [HttpPost("{competitionGUID:guid}")]
        [SwaggerOperation(
            Summary = "Creates a new entry"
        )]
        [SwaggerResponse(200, "The entry was created", typeof(SubmitCompetitionEntry.CompetitionEntryResponse))]
        [SwaggerResponse(500, "The entry was not created")]
        public async Task<ActionResult> GetAllComptetitions([FromRoute] Guid competitionGUID, [FromBody, SwaggerRequestBody(Required = true)] CompetitionEntryDTO competitionEntry, CancellationToken cancellationToken)
        {
            try
            {
                var command = _mapper.Map<SubmitCompetitionEntry.Command>(competitionEntry);
                command.CompetitionGUID = competitionGUID;
                var competition = await _mediator.Send(command, cancellationToken);
                return Ok(competition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{competitionGUID} {@competitionEntry}", competitionGUID, competitionEntry);
                throw;
            }
        }
    }
}