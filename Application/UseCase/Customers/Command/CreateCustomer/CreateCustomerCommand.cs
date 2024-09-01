using Domain.Models.Response;
using MediatR;

namespace Application.UseCase.Customers.Command.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<APIResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public CreateCustomerCommand(int id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;
        }
    }
}
