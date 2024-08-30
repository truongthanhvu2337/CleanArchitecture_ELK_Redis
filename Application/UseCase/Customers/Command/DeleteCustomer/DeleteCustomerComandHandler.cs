using Domain.Models.Response;
using Domain.Repository;
using Domain.Repository.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.Customers.Command.DeleteCustomer
{
    public class DeleteCustomerComandHandler : IRequestHandler<DeleteCustomerCommand, APIResponse>
    {
        private readonly IUserRepo _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerComandHandler(IUserRepo userRepo, IUnitOfWork unitOfWork)
        {
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            await _userRepo.Delete(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Delete successfully",
                Data = null
            };
        }
    }
}
