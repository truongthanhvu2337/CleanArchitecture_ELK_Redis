using Domain.Models.Response;
using Domain.Repository;
using Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;

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
            bool a = await _userRepo.DeleteCustomer(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = a ? "Delete successfully" : "Failed",
                Data = null
            };
        }
    }
}
