using Application.UseCase.Customers.Command.CreateCustomer;
using Application.UseCase.Customers.Command.DeleteCustomer;
using Application.UseCase.Customers.Command.UpdateCustomer;
using Application.UseCase.Customers.Queries.GetAllByPagination;
using Application.UseCase.Customers.Queries.GetAllCustomers;
using Application.UseCase.Customers.Queries.GetCustomerById;
using Domain.DTOs.Customer.Request;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using System.ComponentModel.DataAnnotations;
using System.Net;
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
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }

        [HttpGet("id")]
        public async Task<ActionResult<APIResponse>> GetById([FromQuery] int id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> CreateCustomer([FromBody] CreateCustomerCommand command,CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("")]
        public async Task<ActionResult<APIResponse>> Delete(DeleteCustomerCommand command,CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }

        [HttpPut("")]
        public async Task<ActionResult<APIResponse>> Update(
            [FromQuery] int id,
            [FromBody] CustomerRequestDto dto,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new UpdateCustomerCommand(id, dto.Name!, dto.Address!), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }
    }
}
