using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PoseidonAPI.Contracts.Bid;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;
using AutoMapper;

namespace PoseidonAPI.Controllers
{
    [Route("/bids")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IService<BidDTO> _bidService;
        private readonly IMapper _mapper;

        public BidsController(IService<BidDTO> bidService, IMapper mapper)
        {
            _bidService = bidService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all bids entities
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /bids
        ///     
        /// </remarks>
        /// <response code="200">Returns all bids</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(List<BidResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var result = _bidService.GetAll();
            if (result.Count() > 0)
            {
                List<BidResponse> response = new List<BidResponse>();
                foreach (var item in result)
                {
                    response.Add(_mapper.Map<BidResponse>(item));
                }
                return Ok(response);
            } 
            else
            {
                return NotFound(new ErrorMessage("the list is empty"));
            }
        }

        /// <summary>
        /// Return a bid for a given ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of an entry</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /bids/1
        ///     
        /// </remarks>
        /// <response code="200">Returns the entity corresponding to the id</response>
        /// <response code="400">The id sent does not exist</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(BidResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdNotFound),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var result = _bidService.Get(id);
            if(result != null)
            {
                BidResponse response = _mapper.Map<BidResponse>(result);
                return Ok(response);
            } else
            {
                return BadRequest(new IdNotFound(id));
            }
        }

        /// <summary>
        /// Create a bid and adds it to the database
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     POST /bids
        ///     {
        ///         "account": "account",
        ///         "type": "type",
        ///         "bidQuantity": 1,
        ///         "askQuantity": 1.5,
        ///         "bidValue": 20.6,
        ///         "ask": 1.5,
        ///         "benchmark": "benchmark",
        ///         "bidDate": "2022-01-01T00:00:00",
        ///         "commentary": "commentary",
        ///         "security": "security",
        ///         "status": "status",
        ///         "trader": "trader",
        ///         "book": "book",
        ///         "creationName": "creation",
        ///         "creationDate": "2022-01-01T00:00:00",
        ///         "revisionName": "revisionname",
        ///         "revisionDate": "2022-01-01T00:00:00",
        ///         "dealName": "dealname",
        ///         "dealType": "dealtype",
        ///         "sourceListId": "sourceList",
        ///         "side": "side"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Creation succesfull, returns the entity that just got created</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        /// <response code="404">You must be logged in to access this ressource</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(BidResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ErrorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Add(CreateBidRequest request)
        {
            var bidDTO = _mapper.Map<BidDTO>(request);

            BidDTOValidator validator = new BidDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(bidDTO);
            if (ValidatorResult.IsValid)
            {
                BidResponse response = _mapper.Map<BidResponse>(_bidService.Save(bidDTO));

                return CreatedAtAction(
                    nameof(Get),
                    new { id = response.BidId },
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
        /// Update the bid corresponding to the ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of the entity to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     PUT /bids/1
        ///     {
        ///         "account": "account",
        ///         "type": "type",
        ///         "bidQuantity": 1,
        ///         "askQuantity": 1.5,
        ///         "bidValue": 20.6,
        ///         "ask": 1.5,
        ///         "benchmark": "benchmark",
        ///         "bidDate": "2022-01-01T00:00:00",
        ///         "commentary": "commentary",
        ///         "security": "security",
        ///         "status": "status",
        ///         "trader": "trader",
        ///         "book": "book",
        ///         "creationName": "creation",
        ///         "creationDate": "2022-01-01T00:00:00",
        ///         "revisionName": "revisionname",
        ///         "revisionDate": "2022-01-01T00:00:00",
        ///         "dealName": "dealname",
        ///         "dealType": "dealtype",
        ///         "sourceListId": "sourceList",
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
        public IActionResult Update(int id, UpsertBidRequest bid)
        {
            BidDTO bidDTO = _mapper.Map<BidDTO>(bid);
            bidDTO.BidId = id;

            BidDTOValidator validator = new BidDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(bidDTO);

            if (ValidatorResult.IsValid)
            {
                _bidService.Update(bidDTO);
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
        /// Delete the bid corresponding to the ID
        /// </summary>
        /// <param name="id"> the id is an integer that represent the primaty key of the entity to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     DELETE /bids/1
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
                _bidService.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest(new IdNotDeleted(id));
            }
        }
    }
}
