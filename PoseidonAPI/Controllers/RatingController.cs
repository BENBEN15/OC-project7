using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.Rating;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace PoseidonAPI.Controllers
{
    [Route("/ratings")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IService<RatingDTO> _ratingService;
        private readonly IMapper _mapper;

        public RatingController(IService<RatingDTO> ratingService, IMapper mapper)
        {
            _ratingService = ratingService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _ratingService.GetAll();
            if (result.Count() > 0)
            {
                List<RatingResponse> response = new List<RatingResponse>();
                foreach (var item in result)
                {
                    response.Add(_mapper.Map<RatingResponse>(item));
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
            var result = _ratingService.Get(id);
            if (result != null)
            {
                RatingResponse response = _mapper.Map<RatingResponse>(result);
                return Ok(response);
            }
            else
            {
                return NotFound(id);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(CreateRatingRequest request)
        {
            var ratingDTO = _mapper.Map<RatingDTO>(request);

            RatingDTOValidator validator = new RatingDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(ratingDTO);
            if (ValidatorResult.IsValid)
            {
                RatingResponse response = _mapper.Map<RatingResponse>(_ratingService.Save(ratingDTO));

                return CreatedAtAction(
                    nameof(Get),
                    new { id = response.RatingId },
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
        public IActionResult Update(int id, UpsertRatingRequest rating)
        {
            RatingDTO ratingDTO = _mapper.Map<RatingDTO>(rating);
            ratingDTO.RatingId = id;

            RatingDTOValidator validator = new RatingDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(ratingDTO);

            if (ValidatorResult.IsValid)
            {
                _ratingService.Update(ratingDTO);
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
                _ratingService.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
