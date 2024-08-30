using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.Customers.Command.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<APIResponse>
    {
        public string Name { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;

        public UpdateCustomerCommand(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
