using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.Rating;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;
using AutoMapper;

namespace PoseidonAPI.Controllers
{
    [Route("/ratings")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class RatingController : ControllerBase
    {
        private readonly IService<RatingDTO> _ratingService;
        private readonly IMapper _mapper;

        public RatingController(IService<RatingDTO> ratingService, IMapper mapper)
        {
            _ratingService = ratingService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all ratings entities
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /ratings
        ///     
        /// </remarks>
        /// <response code="200">Returns all ratings</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(List<RatingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Return a rating for a given ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of an entry</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /ratings/1
        ///     
        /// </remarks>
        /// <response code="200">Returns the entity corresponding to the id</response>
        /// <response code="400">The id sent does not exist</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(RatingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdNotFound), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                return BadRequest(new IdNotFound(id));
            }
        }

        /// <summary>
        /// Create a rating and adds it to the database
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     POST /ratings
        ///     {
        ///         "moodysRating": "moodysrating",
        ///         "sandPrating": "sandprating",
        ///         "fitchRating": "fitchrating",
        ///         "orderNumber": 1
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Creation succesfull, returns the entity that just got created</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        /// <response code="404">You must be logged in to access this ressource</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(RatingResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ErrorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Update the rating corresponding to the ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of the entity to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     PUT /ratings/1
        ///     {
        ///         "moodysRating": "moodysrating",
        ///         "sandPrating": "sandprating",
        ///         "fitchRating": "fitchrating",
        ///         "orderNumber": 1
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

        /// <summary>
        /// Delete the rating corresponding to the ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of the entity to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     DELETE /ratings/1
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
                _ratingService.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest(new IdNotDeleted(id));
            }
        }
    }
}
