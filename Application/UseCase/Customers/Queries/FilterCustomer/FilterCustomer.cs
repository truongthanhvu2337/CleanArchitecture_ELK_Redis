using Domain.Models.Response;
using MediatR;

namespace Application.UseCase.Customers.Queries.FilterCustomer
{
    public class FilterCustomer : IRequest<APIResponse>
    {
        public string? name { get; set; }
        public string? address { get; set; }

        public FilterCustomer()
        {
        }

        public FilterCustomer(string? name, string? address)
        {
            this.name = name;
            this.address = address;
        }
    }
}
