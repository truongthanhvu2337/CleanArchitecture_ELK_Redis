using Domain.Models.Response;
using MediatR;

namespace Application.UseCase.Customers.Command.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<APIResponse>
    {
        public int id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;

        public UpdateCustomerCommand(int id, string name, string address)
        {
            this.id = id;
            Name = name;
            Address = address;
        }
    }
}
