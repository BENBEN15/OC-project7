using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.Rule;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;
using AutoMapper;

namespace PoseidonAPI.Controllers
{
    [Route("/rules")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class RuleController : ControllerBase
    {
        private readonly IService<RuleDTO> _ruleService;
        private readonly IMapper _mapper;

        public RuleController(IService<RuleDTO> ruleService, IMapper mapper)
        {
            _ruleService = ruleService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all rules entities
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /rules
        ///     
        /// </remarks>
        /// <response code="200">Returns all rules</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(List<RuleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var result = _ruleService.GetAll();
            if (result.Count() > 0)
            {
                List<RuleResponse> response = new List<RuleResponse>();
                foreach (var item in result)
                {
                    response.Add(_mapper.Map<RuleResponse>(item));
                }
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Return a rule for a given ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of an entry</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /rules/1
        ///     
        /// </remarks>
        /// <response code="200">Returns the entity corresponding to the id</response>
        /// <response code="400">The id sent does not exist</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(RuleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdNotFound), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var result = _ruleService.Get(id);
            if (result != null)
            {
                RuleResponse response = _mapper.Map<RuleResponse>(result);
                return Ok(response);
            }
            else
            {
                return BadRequest(new IdNotFound(id));
            }
        }

        /// <summary>
        /// Create a rule and adds it to the database
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     POST /rules
        ///     {
        ///         "name": "name",
        ///         "description": "desc",
        ///         "json": "json",
        ///         "template": "template",
        ///         "sqlStr": "sqlstr",
        ///         "sqlPart": "sqlPart"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Creation succesfull, returns the entity that just got created</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        /// <response code="404">You must be logged in to access this ressource</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(RuleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ErrorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Add(CreateRuleRequest request)
        {
            var ruleDTO = _mapper.Map<RuleDTO>(request);

            RuleDTOValidator validator = new RuleDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(ruleDTO);
            if (ValidatorResult.IsValid)
            {
                RuleResponse response = _mapper.Map<RuleResponse>(_ruleService.Save(ruleDTO));

                return CreatedAtAction(
                    nameof(Get),
                    new { id = response.RuleId },
                    response);
            }
            else
            {
                List<ErrorModel> errors = new List<ErrorModel>();
                foreach (var failure in ValidatorResult.Errors)
                {
                    ErrorModel error = new ErrorModel
                    {
                        errorCode = failure.ErrorCode,
                        errorField = failure.PropertyName,
                        errorMessage = failure.ErrorMessage,
                    };

                    errors.Add(error);
                }

                return BadRequest(errors);
            }
        }

        /// <summary>
        /// Update the rule corresponding to the ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of the entity to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     PUT /rules/1
        ///     {
        ///         "name": "name",
        ///         "description": "desc",
        ///         "json": "json",
        ///         "template": "template",
        ///         "sqlStr": "sqlstr",
        ///         "sqlPart": "sqlPart"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Update succesfull, the entity have been successfully updated</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        /// <response code="404">You must be logged in to access this ressource</response>
        [Authorize]
        [HttpPut, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, UpsertRuleRequest rule)
        {
            RuleDTO ruleDTO = _mapper.Map<RuleDTO>(rule);
            ruleDTO.RuleId = id;

            RuleDTOValidator validator = new RuleDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(ruleDTO);

            if (ValidatorResult.IsValid)
            {
                _ruleService.Update(ruleDTO);
                return Ok();
            }
            else
            {
                List<ErrorModel> errors = new List<ErrorModel>();
                foreach (var failure in ValidatorResult.Errors)
                {
                    ErrorModel error = new ErrorModel
                    {
                        errorCode = failure.ErrorCode,
                        errorField = failure.PropertyName,
                        errorMessage = failure.ErrorMessage,
                    };

                    errors.Add(error);
                }

                return BadRequest(errors);
            }
        }

        /// <summary>
        /// Delete the rule corresponding to the ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of the entity to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     DELETE /rules/1
        ///     
        /// </remarks>
        /// <response code="200">Deletion succesfull, the entity have been successfully deleted</response>
        /// <response code="400">The id sent does not exist</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpDelete, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdNotDeleted), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            try
            {
                _ruleService.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest(new IdNotDeleted(id));
            }
        }
    }
}
