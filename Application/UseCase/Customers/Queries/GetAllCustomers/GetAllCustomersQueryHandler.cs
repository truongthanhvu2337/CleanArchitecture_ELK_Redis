using AutoMapper;
using Domain.Models.Response;
using Domain.Repository;
using Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCase.Customers.Queries.GetAllCustomers
{

    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public GetAllCustomersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserRepo userRepo)
        {
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepo.GetAllCustomers();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "OK",
                Data = _mapper.Map<IEnumerable<CustomerResponseDto>>(users),
            };
        }
    }
}
