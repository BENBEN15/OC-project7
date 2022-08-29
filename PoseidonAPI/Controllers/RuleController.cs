using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.Rule;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace PoseidonAPI.Controllers
{
    [Route("/rules")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly IService<RuleDTO> _ruleService;
        private readonly IMapper _mapper;

        public RuleController(IService<RuleDTO> ruleService, IMapper mapper)
        {
            _ruleService = ruleService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
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

        [Authorize]
        [HttpGet, Route("{id}")]
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
                return NotFound(id);
            }
        }

        [Authorize]
        [HttpPost]
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

        [Authorize]
        [HttpPut, Route("{id}")]
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

        [Authorize]
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _ruleService.Delete(id);
                return Ok();
            }
            catch
            {
                return NotFound(id);
            }
        }
    }
}
