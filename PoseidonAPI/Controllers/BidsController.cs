﻿using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.Bid;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace PoseidonAPI.Controllers
{
    [Route("api/bids")]
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

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _bidService.GetAll();
            if(result.Count() > 0)
            {
                List<BidResponse> response = new List<BidResponse>();
                foreach (var item in result)
                {
                    response.Add(_mapper.Map<BidResponse>(item));
                }
                return Ok(response);
            } else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet, Route("{id}")]
        public IActionResult Get(int id)
        {
            var result = _bidService.Get(id);
            if(result != null)
            {
                BidResponse response = _mapper.Map<BidResponse>(result);
                return Ok(response);
            } else
            {
                return NotFound(id);
            }
        }

        [Authorize]
        [HttpPost]
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

        [Authorize]
        [HttpPut, Route("{id}")]
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

        [Authorize]
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _bidService.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
