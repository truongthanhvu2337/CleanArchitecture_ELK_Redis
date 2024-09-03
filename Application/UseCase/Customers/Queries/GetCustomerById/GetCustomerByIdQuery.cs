using Domain.Models.Response;
using MediatR;

namespace Application.UseCase.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<APIResponse>
    {
        public int Id { get; set; }

        public GetCustomerByIdQuery(int id)
        {
            Id = id;
        }
    }
}
