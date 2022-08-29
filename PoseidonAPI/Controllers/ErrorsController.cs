using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PoseidonAPI.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class ErrorsController : ControllerBase
    {
        /// <summary>
        /// Returns an error when an exception is thrown in the api
        /// </summary>
        /// <returns></returns>
        /// <response code="500">an exception was thrown in the api</response>
        [HttpGet, Route("/error")]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}
