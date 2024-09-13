
namespace WebGeoElasticsearch.Models;

public class MapDetail
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Information { get; set; } = string.Empty;
    public string DetailsType { get; set; } = string.Empty;

    //[ElasticsearchGeoPoint]
	//public GeoPoint DetailsCoordinates { get; set; }
}