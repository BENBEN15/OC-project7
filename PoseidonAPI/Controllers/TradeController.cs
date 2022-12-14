using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.Trade;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;
using AutoMapper;

namespace PoseidonAPI.Controllers
{
    [Route("/trades")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class TradeController : ControllerBase
    {
        private readonly IService<TradeDTO> _tradeService;
        private readonly IMapper _mapper;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IService<TradeDTO> tradeService, IMapper mapper, ILogger<TradeController> logger)
        {
            _tradeService = tradeService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Returns all trades entities
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /trades
        ///     
        /// </remarks>
        /// <response code="200">Returns all trades</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(List<TradeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"User : {User.Identity.Name}, route : GET /trades, callback : GetAll()", DateTime.UtcNow.ToLongTimeString());
            var result = _tradeService.GetAll();
            if (result.Count() > 0)
            {
                List<TradeResponse> response = new List<TradeResponse>();
                foreach (var item in result)
                {
                    response.Add(_mapper.Map<TradeResponse>(item));
                }
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Return a trade for a given ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of an entry</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /trades/1
        ///     
        /// </remarks>
        /// <response code="200">Returns the entity corresponding to the id</response>
        /// <response code="400">The id sent does not exist</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(TradeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdNotFound), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            _logger.LogInformation($"User : {User.Identity.Name}, route : GET /trades/{id}, callback : Get()", DateTime.UtcNow.ToLongTimeString());
            var result = _tradeService.Get(id);
            if (result != null)
            {
                TradeResponse response = _mapper.Map<TradeResponse>(result);
                return Ok(response);
            }
            else
            {
                return BadRequest(new IdNotFound(id));
            }
        }

        /// <summary>
        /// Create a trade and adds it to the database
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     POST /trades
        ///     {
        ///         "account": "account",
        ///         "type": "type",
        ///         "buyQuantity": 1,
        ///         "sellQuantity": 1,
        ///         "buyPrice": 1,
        ///         "sellPrice": 1,
        ///         "tradeDate": "2022-01-01T00:00:00",
        ///         "security": "security",
        ///         "status": "status",
        ///         "trader": "trader",
        ///         "benchmark": "benchmark",
        ///         "book": "book",
        ///         "creationName": "CreationName",
        ///         "creationDate": "2022-01-01T00:00:00",
        ///         "revisionName": "revisionName",
        ///         "revisionDate": "2022-01-01T00:00:00",
        ///         dealName": "dealName",
        ///         "dealType": "dealType",
        ///         "sourceListId": "SourceListId",
        ///         "side": "side"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Creation succesfull, returns the entity that just got created</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        /// <response code="404">You must be logged in to access this ressource</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(TradeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ErrorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Add(CreateTradeRequest request)
        {
            _logger.LogInformation($"User : {User.Identity.Name}, route : POST /trades, callback : Add()", DateTime.UtcNow.ToLongTimeString());

            TradeCreateValidator validator = new TradeCreateValidator();
            ValidationResult ValidatorResult = validator.Validate(request);
            if (ValidatorResult.IsValid)
            {
                var tradeDTO = _mapper.Map<TradeDTO>(request);
                TradeResponse response = _mapper.Map<TradeResponse>(_tradeService.Save(tradeDTO));

                return CreatedAtAction(
                    nameof(Get),
                    new { id = response.TradeId },
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
        /// Update the trade corresponding to the ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of the entity to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     PUT /trades/1
        ///     {
        ///         "account": "account",
        ///         "type": "type",
        ///         "buyQuantity": 1,
        ///         "sellQuantity": 1,
        ///         "buyPrice": 1,
        ///         "sellPrice": 1,
        ///         "tradeDate": "2022-01-01T00:00:00",
        ///         "security": "security",
        ///         "status": "status",
        ///         "trader": "trader",
        ///         "benchmark": "benchmark",
        ///         "book": "book",
        ///         "creationName": "CreationName",
        ///         "creationDate": "2022-01-01T00:00:00",
        ///         "revisionName": "revisionName",
        ///         "revisionDate": "2022-01-01T00:00:00",
        ///         dealName": "dealName",
        ///         "dealType": "dealType",
        ///         "sourceListId": "SourceListId",
        ///         "side": "side"
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
        public IActionResult Update(int id, UpsertTradeRequest trade)
        {
            _logger.LogInformation($"User : {User.Identity.Name}, route : PUT /trades/{id}, callback : Update()", DateTime.UtcNow.ToLongTimeString());

            TradeUpsertValidator validator = new TradeUpsertValidator();
            ValidationResult ValidatorResult = validator.Validate(trade);

            if (ValidatorResult.IsValid)
            {
                TradeDTO tradeDTO = _mapper.Map<TradeDTO>(trade);
                tradeDTO.TradeId = id;
                _tradeService.Update(tradeDTO);
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
        /// Delete the trade corresponding to the ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of the entity to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     DELETE /Trades/1
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
            _logger.LogInformation($"User : {User.Identity.Name}, route : DELETE /trades/{id}, callback : Delete()", DateTime.UtcNow.ToLongTimeString());
            try
            {
                _tradeService.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest(new IdNotDeleted(id));
            }
        }
    }
}
