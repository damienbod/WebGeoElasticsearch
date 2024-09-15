using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace WebGeoElasticsearch.ElasticsearchApi;

public class ElasticClientProvider
{
    private readonly ElasticsearchClient? _client = null;

    public ElasticClientProvider(IConfiguration configuration)
    {
        if (_client == null)
        {
            var settings = new ElasticsearchClientSettings(new Uri(configuration["ElasticsearchUrl"]!))
                .Authentication(new BasicAuthentication(configuration["ElasticsearchUserName"]!, 
                    configuration["ElasticsearchPassword"]!));

            _client = new ElasticsearchClient(settings);
        }
    }
  
    public ElasticsearchClient GetClient()
    {
        if(_client != null)
        {
            return _client;
        }

        throw new Exception("Elasticsearch client not initialized");
    }

}