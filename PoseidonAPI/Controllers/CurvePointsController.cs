using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.CurvePoint;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace PoseidonAPI.Controllers
{
    [Route("/curvePoints")]
    [ApiController]
    public class CurvePointsController : ControllerBase
    {
        private readonly IService<CurvePointDTO> _curvePointService;
        private readonly IMapper _mapper;

        public CurvePointsController(IService<CurvePointDTO> curvePointService, IMapper mapper)
        {
            _curvePointService = curvePointService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _curvePointService.GetAll();
            if (result.Count() > 0)
            {
                List<CurvePointResponse> response = new List<CurvePointResponse>();
                foreach (var item in result)
                {
                    response.Add(_mapper.Map<CurvePointResponse>(item));
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
            var result = _curvePointService.Get(id);
            if (result != null)
            {
                CurvePointResponse response = _mapper.Map<CurvePointResponse>(result);
                return Ok(response);
            }
            else
            {
                return NotFound(id);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(CreateCurvePointRequest request)
        {
            var curvePointDTO = _mapper.Map<CurvePointDTO>(request);

            CurvePointDTOValidator validator = new CurvePointDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(curvePointDTO);
            if (ValidatorResult.IsValid)
            {
                CurvePointResponse response = _mapper.Map<CurvePointResponse>(_curvePointService.Save(curvePointDTO));

                return CreatedAtAction(
                    nameof(Get),
                    new { id = response.CurvePointId },
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
        public IActionResult Update(int id, UpsertCurvePointRequest curvePoint)
        {
            CurvePointDTO curvePointDTO = _mapper.Map<CurvePointDTO>(curvePoint);
            curvePointDTO.CurvePointId = id;

            CurvePointDTOValidator validator = new CurvePointDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(curvePointDTO);

            if (ValidatorResult.IsValid)
            {
                _curvePointService.Update(curvePointDTO);
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
                _curvePointService.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
