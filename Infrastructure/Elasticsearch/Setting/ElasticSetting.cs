namespace Infrastructure.Elasticsearch.Setting
{
    public class ElasticSetting
    {
        public string Url { get; set; } = string.Empty;
        public string? DefaultIndex { get; set; }
    }
}
