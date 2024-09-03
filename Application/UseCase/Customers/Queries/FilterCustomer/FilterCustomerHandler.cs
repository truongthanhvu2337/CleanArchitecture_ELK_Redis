using Application.Abstractions.ElasticService;
using AutoMapper;
using Domain.Entities;
using Domain.Models.Response;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using MediatR;
using System.Net;

namespace Application.UseCase.Customers.Queries.FilterCustomer
{
    public class FilterCustomerHandler : IRequestHandler<FilterCustomer, APIResponse>
    {
        private readonly IElasticService<Customer> _elasticService;
        private readonly IMapper _mapper;

        public FilterCustomerHandler(IElasticService<Customer> elasticService, IMapper mapper)
        {
            _elasticService = elasticService;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(FilterCustomer request, CancellationToken cancellationToken)
        {
            //var filter = new SearchRequestDescriptor<Customer>()
            //    .Query(q => q
            //    .Bool(b => b
            //    .Should(
            //        s => s.Match(m => m
            //            .Field(f => f.Name)
            //            .Query(!string.IsNullOrEmpty(request.name) ? request.name : "")
            //        ),
            //        s => s.Match(m => m
            //            .Field(f => f.Address)
            //            .Query(!string.IsNullOrEmpty(request.name) ? request.name : "")
            //        )
            //    )));


            var filter = new SearchRequestDescriptor<Customer>()
                .Query(q => q
                .Bool(b => b
                .Should(
                    s => s.Wildcard(m => m
                        .Field(f => f.Name)
                        .Value(!string.IsNullOrEmpty(request.name) ? $"*{request.name}*" : "")
                    ),
                    s => s.Wildcard(m => m
                        .Field(f => f.Address)
                        .Value(!string.IsNullOrEmpty(request.address) ? $"*{request.address}*" : "")
                    )
                )));

            var result = await _elasticService.FilterAsync(filter);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "OK",
                Data = _mapper.Map<IEnumerable<CustomerResponseDto>>(result),
            };
        }
    }
}
