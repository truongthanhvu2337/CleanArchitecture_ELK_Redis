using Domain.Models.Response;
using MediatR;

namespace Application.UseCase.Customers.Command.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<APIResponse>
    {
        public int Id { get; set; }

        public DeleteCustomerCommand(int id)
        {
            Id = id;
        }
    }
}
