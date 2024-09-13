namespace WebGeoElasticsearch.Models;

public class MapModel
{
	public string MapData { get; set; } = string.Empty;
    public double CenterLongitude { get; set; }
	public double CenterLatitude { get; set; }
	public uint MaxDistanceInMeter { get; set; }
}