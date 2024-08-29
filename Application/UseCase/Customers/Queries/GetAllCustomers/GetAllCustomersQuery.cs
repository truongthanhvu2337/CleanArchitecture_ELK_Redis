using Domain.Models.Response;
using MediatR;

namespace Application.UseCase.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersQuery : IRequest<APIResponse>
    {
        public GetAllCustomersQuery() { }
    }
}
