using Azure;
using Elastic.Clients.Elasticsearch;

namespace Infrastructure.Persistence.Elasticsearch
{
    public class ElasticService<TDomain> : IElasticService<TDomain> where TDomain : class
    {
        private readonly ElasticsearchClient _client;
        private readonly string _indexName;

        public ElasticService(ElasticsearchClient client)
        {
            _client = client;
            _indexName = typeof(TDomain).Name.ToLower();
        }

        public async Task<IEnumerable<TDomain>> FilterAsync(SearchRequestDescriptor<TDomain> descriptor)
        {
            var indexResponse = await _client.SearchAsync(descriptor);
            return indexResponse.Documents.ToList();
        }

        public async Task<bool> IndexDocumentAsync(TDomain domain)
        {

            var indexResponse = await _client.IndexAsync(domain, idx => idx.Index(_indexName));
            if (!indexResponse.IsValidResponse)
            {
                // Xử lý lỗi
                throw new Exception($"Error searching customers: {indexResponse.TryGetOriginalException}");
            }
            return true;
        }

        public async Task<bool> IndexDocumentWithKeywordAsync(TDomain domain, int keyword)
        {

            var indexResponse = await _client.IndexAsync(domain, idx => idx.Index(_indexName).Id(keyword));
            if (!indexResponse.IsValidResponse)
            {
                // Xử lý lỗi
                throw new Exception($"Error searching customers: {indexResponse.TryGetOriginalException}");
            }
            return true;
        }

        public async Task<bool> BulkIndexDocumentsAsync(IEnumerable<TDomain> customer/*, string indexName*/)
        {
            var response = await _client.BulkAsync(idx => idx.Index(_indexName)
                    .UpdateMany(customer, (ud, u) => ud.Doc(u).DocAsUpsert(true)));

            return response.IsValidResponse;
        }

        public async Task<TDomain> Get(int key)
        {
            var response = await _client.GetAsync<TDomain>(key, g => g.Index(_indexName));
            return response.Source;
        }

        public async Task<IEnumerable<TDomain>?> GetAll()
        {
            var response = await _client.SearchAsync<TDomain>(a => a.Index(_indexName));

            return response.IsValidResponse ? response.Documents.ToList() : default;
        }

        public async Task<bool> Remove(int key)
        {
            var response = await _client.DeleteAsync<TDomain>(key, d => d.Index(_indexName));
            return response.IsValidResponse;
        }

        public async Task<long?> RemoveAll()
        {
            var response = await _client.DeleteByQueryAsync<TDomain>(d => d.Indices(_indexName));
            return response.IsValidResponse ? response.Deleted : default;
        }

    }
}
