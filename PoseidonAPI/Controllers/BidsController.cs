using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Services;
using PoseidonAPI.Dtos;
using PoseidonAPI.Validators;
using FluentValidation.Results;

namespace PoseidonAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IService<BidDTO> _bidService;

        public BidsController(IService<BidDTO> bidService)
        {
            _bidService = bidService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = _bidService.GetAll();
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
            
        }

        [HttpGet, Route("/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _bidService.Get(id);
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost, Route("/")]
        public IActionResult Add(BidDTO bid)
        {
            BidDTOValidator validator = new BidDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(bid);

            if (ValidatorResult.IsValid)
            {
                _bidService.Save(bid);
                return Ok();
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var failure in ValidatorResult.Errors)
                {
                    string error = "Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage;
                    errors.Add(error);
                }

                return BadRequest(errors);
            }
        }

        [HttpPut, Route("/")]
        public IActionResult Update(BidDTO bid)
        {
            BidDTOValidator validator = new BidDTOValidator();
            ValidationResult ValidatorResult = validator.Validate(bid);

            if (ValidatorResult.IsValid)
            {
                _bidService.Update(bid);
                return Ok();
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var failure in ValidatorResult.Errors)
                {
                    string error = "Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage;
                    errors.Add(error);
                }

                return BadRequest(errors);
            }
        }

        [HttpDelete, Route("/{id}")]
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
