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

        public TradeController(IService<TradeDTO> tradeService, IMapper mapper)
        {
            _tradeService = tradeService;
            _mapper = mapper;
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
            var tradeDTO = _mapper.Map<TradeDTO>(request);

            TradeDTOValidator validator = new TradeDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(tradeDTO);
            if (ValidatorResult.IsValid)
            {
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
            TradeDTO tradeDTO = _mapper.Map<TradeDTO>(trade);
            tradeDTO.TradeId = id;

            TradeDTOValidator validator = new TradeDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(tradeDTO);

            if (ValidatorResult.IsValid)
            {
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
