using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.CurvePoint;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;
using AutoMapper;

namespace PoseidonAPI.Controllers
{
    [Route("/curvePoints")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CurvePointsController : ControllerBase
    {
        private readonly IService<CurvePointDTO> _curvePointService;
        private readonly IMapper _mapper;
        private readonly ILogger<CurvePointsController> _logger;

        public CurvePointsController(IService<CurvePointDTO> curvePointService, IMapper mapper, ILogger<CurvePointsController> logger)
        {
            _curvePointService = curvePointService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Returns all curve points entities
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /curvePoints
        ///     
        /// </remarks>
        /// <response code="200">Returns all curve points</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(List<CurvePointResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"User : {User.Identity.Name}, route : GET /curvePoints, callback : GetAll()", DateTime.UtcNow.ToLongTimeString());
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

        /// <summary>
        /// Return a curve point for a given ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of an entry</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /curvePoints/1
        ///     
        /// </remarks>
        /// <response code="200">Returns the entity corresponding to the id</response>
        /// <response code="400">The id sent does not exist</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(CurvePointResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdNotFound), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            _logger.LogInformation($"User : {User.Identity.Name}, route : GET /curvePoints/{id}, callback : Get()", DateTime.UtcNow.ToLongTimeString());
            var result = _curvePointService.Get(id);
            if (result != null)
            {
                CurvePointResponse response = _mapper.Map<CurvePointResponse>(result);
                return Ok(response);
            }
            else
            {
                return BadRequest(new IdNotFound(id));
            }
        }

        /// <summary>
        /// Create a curve point and adds it to the database
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     POST /curvePoints
        ///     {
        ///         "curveId": 1,
        ///         "asOfDate": "2022-01-01T00:00:00",
        ///         "term": 1,
        ///         "value": 1,
        ///         "creationDate": "2022-01-01T00:00:00"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Creation succesfull, returns the entity that just got created</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        /// <response code="404">You must be logged in to access this ressource</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CurvePointResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ErrorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Add(CreateCurvePointRequest request)
        {
            _logger.LogInformation($"User : {User.Identity.Name}, route : POST /curvePoints, callback : Add()", DateTime.UtcNow.ToLongTimeString());

            CurvePointCreateValidator validator = new CurvePointCreateValidator();
            ValidationResult ValidatorResult = validator.Validate(request);
            if (ValidatorResult.IsValid)
            {
                var curvePointDTO = _mapper.Map<CurvePointDTO>(request);
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

        /// <summary>
        /// Update the curve point corresponding to the ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of the entity to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     PUT /curvePoints/1
        ///     {
        ///         "curveId": 1,
        ///         "asOfDate": "2022-01-01T00:00:00",
        ///         "term": 1,
        ///         "value": 1,
        ///         "creationDate": "2022-01-01T00:00:00"
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
        public IActionResult Update(int id, UpsertCurvePointRequest curvePoint)
        {
            _logger.LogInformation($"User : {User.Identity.Name}, route : PUT /curvePoints/{id}, callback : Update()", DateTime.UtcNow.ToLongTimeString());

            CurvePointUpsertValidator validator = new CurvePointUpsertValidator();
            ValidationResult ValidatorResult = validator.Validate(curvePoint);

            if (ValidatorResult.IsValid)
            {
                CurvePointDTO curvePointDTO = _mapper.Map<CurvePointDTO>(curvePoint);
                curvePointDTO.CurvePointId = id;
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

        /// <summary>
        /// Delete the curve point corresponding to the ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of the entity to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     DELETE /curvePoints/1
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
            _logger.LogInformation($"User : {User.Identity.Name}, route : DELETE /curvePoints/{id}, callback : Delete()", DateTime.UtcNow.ToLongTimeString());
            try
            {
                _curvePointService.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest(new IdNotDeleted(id));
            }
        }
    }
}
