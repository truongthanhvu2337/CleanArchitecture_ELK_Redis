using Domain.Models.Response;
using MediatR;

namespace Application.UseCase.Customers.Command.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<APIResponse>
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public CreateCustomerCommand(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
