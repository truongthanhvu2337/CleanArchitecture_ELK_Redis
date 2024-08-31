using Domain.Entities;
using Elastic.Clients.Elasticsearch;

namespace Infrastructure.Persistence.Elasticsearch
{
    public interface IElasticService<TDomain> where TDomain : class
    {
        Task<bool> IndexDocumentAsync(TDomain customer);
        Task<bool> BulkIndexDocumentsAsync(IEnumerable<TDomain> customer/*, string indexName*/);
        Task<TDomain> Get(int key);
        Task<IEnumerable<TDomain>?> GetAll();
        Task<bool> Remove(int key);
        Task<long?> RemoveAll();
        Task<IEnumerable<TDomain>> FilterAsync(SearchRequestDescriptor<TDomain> descriptor);

        /// <summary>
        /// Ahihiohihhihihii
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<bool> IndexDocumentWithKeywordAsync(TDomain domain, int keyword);
    }
}

