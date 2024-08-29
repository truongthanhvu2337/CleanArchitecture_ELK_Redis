using AutoMapper;
using Domain.Models.Response;
using Domain.Repository;
using Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCase.Customers.Queries.GetAllByPagination
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllCustomerQueryByPagination, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserRepo userRepo)
        {
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetAllCustomerQueryByPagination request, CancellationToken cancellationToken)
        {
            var users = await _userRepo.GetAll(request.Page, request.EachPage, "Name");
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "OK",
                Data = _mapper.Map<IEnumerable<CustomerResponseDto>>(users),
            };
        }
    }

}
