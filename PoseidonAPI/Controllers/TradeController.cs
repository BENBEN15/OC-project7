using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.Trade;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace PoseidonAPI.Controllers
{
    [Route("/trades")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly IService<TradeDTO> _tradeService;
        private readonly IMapper _mapper;

        public TradeController(IService<TradeDTO> tradeService, IMapper mapper)
        {
            _tradeService = tradeService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
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

        [Authorize]
        [HttpGet, Route("{id}")]
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
                return NotFound(id);
            }
        }

        [Authorize]
        [HttpPost]
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

        [Authorize]
        [HttpPut, Route("{id}")]
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

        [Authorize]
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _tradeService.Delete(id);
                return Ok();
            }
            catch
            {
                return NotFound(id);
            }
        }
    }
}
