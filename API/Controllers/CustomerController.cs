using Application.UseCase.Customers.Command.CreateCustomer;
using Application.UseCase.Customers.Queries.GetAllByPagination;
using Application.UseCase.Customers.Queries.GetAllCustomers;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/customers")]
    //[Authorize]
    public class CustomerController : ControllerBase
    {
        private ISender _mediator;
        public CustomerController(ISender mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("")]
        public async Task<ActionResult<APIResponse>> GetAllByPagination([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
                                                            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllCustomerQueryByPagination(pageNo, eachPage), cancellationToken);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<APIResponse>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllCustomersQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> CreateCustomer([FromBody] CreateCustomerCommand command,CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
