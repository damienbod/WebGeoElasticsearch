using Elastic.Clients.Elasticsearch;

namespace WebGeoElasticsearch.Models;

public class MapDetail
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Information { get; set; } = string.Empty;
    public string DetailsType { get; set; } = string.Empty;

    public GeoLocation DetailsCoordinates { get; set; } = GeoLocation.Coordinates([7.47348, 46.95404]);
}