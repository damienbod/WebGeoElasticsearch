using Elastic.Clients.Elasticsearch;
using WebGeoElasticsearch.Models;

namespace WebGeoElasticsearch.ElasticsearchApi;

public class SearchProvider
{
    private const string IndexName = "mapdetails";
    private readonly ElasticsearchClient _client;

    public SearchProvider(ElasticClientProvider elasticClientProvider)
    {
        _client = elasticClientProvider.GetClient();
    }

    public async Task AddMapDetailDataAsync()
    {
        var dotNetGroup = new MapDetail 
        { 
            DetailsCoordinates = GeoLocation.LatitudeLongitude( new LatLonGeoLocation { Lat = 7.47348, Lon = 46.95404}), 
            Id = 1, Name = ".NET User Group Bern", Details = "http://www.dnug-bern.ch/", DetailsType = "Work" 
        };
        var dieci = new MapDetail 
        { 
            DetailsCoordinates = GeoLocation.LatitudeLongitude(new LatLonGeoLocation { Lat = 7.41148, Lon = 46.94450 }), 
            Id = 2, Name = "Dieci Pizzakurier Bern", Details = "http://www.dieci.ch", DetailsType = "Pizza" 
        };
        var babylonKoeniz = new MapDetail 
        { 
            DetailsCoordinates = GeoLocation.LatitudeLongitude(new LatLonGeoLocation { Lat = 7.41635, Lon = 46.92737 }), 
            Id = 3, Name = "PIZZERIA BABYLON Köniz", Details = "http://www.pizza-babylon.ch/home-k.html", DetailsType = "Pizza" 
        };
        var babylonOstermundigen = new MapDetail 
        { 
            DetailsCoordinates = GeoLocation.LatitudeLongitude(new LatLonGeoLocation { Lat = 7.48256, Lon = 46.95578 }), 
            Id = 4, Name = "PIZZERIA BABYLON Ostermundigen", Details = "http://www.pizza-babylon.ch/home-o.html", DetailsType = "Pizza" 
        };

        var exist = await _client.Indices.ExistsAsync(IndexName);
        if (exist.Exists)
        {
            await _client.Indices.DeleteAsync(IndexName);
        }

        var response = await _client.IndexAsync(dotNetGroup, IndexName, "1");
        response = await _client.IndexAsync(dieci, IndexName, "2");
        response = await _client.IndexAsync(babylonKoeniz, IndexName, "3");
        response = await _client.IndexAsync(babylonOstermundigen, IndexName, "4");
    }

    //{
    //  "query" :
    //  {
    //	"filtered" : {
    //		"query" : {
    //			"match_all" : {}
    //		},
    //		"filter" : {
    //			"geo_distance" : {
    //				"distance" : "300m",
    //				 "detailscoordinates" : [7.41148,46.9445]
    //			}
    //		}
    //	}
    //  },
    // "sort" : [
    //		{
    //			"_geo_distance" : {
    //				"detailscoordinates" : [7.41148,46.9445],
    //				"order" : "asc",
    //				"unit" : "km"
    //			}
    //		}
    //	]
    //	}
    //}
    public List<MapDetail> SearchForClosest(uint maxDistanceInMeter, double centerLongitude, double centerLatitude)
    {
        //if (maxDistanceInMeter == 0)
        //{
        //    maxDistanceInMeter = 1000000;
        //}
        //var search = new Search
        //{
        //    Query = new Query(
        //        new Filtered(
        //            new Filter(
        //                new GeoDistanceFilter(
        //                    "detailscoordinates",
        //                    new GeoPoint(centerLongitude, centerLatitude),
        //                    new DistanceUnitMeter(maxDistanceInMeter)
        //                )
        //            )
        //        )
        //        {
        //            Query = new Query(new MatchAllQuery())
        //        }
        //    ),
        //    Sort = new SortHolder(
        //        new List<ISort>
        //        {
        //            new SortGeoDistance("detailscoordinates", DistanceUnitEnum.m, new GeoPoint(centerLongitude, centerLatitude))
        //            {
        //                Order = OrderEnum.asc
        //            }
        //        }
        //    )
        //};

        List<MapDetail> result = new();
        
        return result;
    }
}