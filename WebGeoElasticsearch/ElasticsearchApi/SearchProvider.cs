using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Clients.Elasticsearch.QueryDsl;
using WebGeoElasticsearch.Models;

namespace WebGeoElasticsearch.ElasticsearchApi;

public class SearchProvider
{
    private const string IndexName = "mapdetails";
    private readonly ElasticsearchClient _client;
    private readonly ILogger<SearchProvider> _logger;

    public SearchProvider(ElasticClientProvider elasticClientProvider, ILogger<SearchProvider> logger)
    {
        _client = elasticClientProvider.GetClient();
        _logger = logger;
    }

    public async Task AddMapDetailDataAsync()
    {
        var dotNetGroup = new MapDetail 
        { 
            DetailsCoordinates = GeoLocation.LatitudeLongitude( new LatLonGeoLocation { Lon = 7.47348, Lat = 46.95404}), 
            Id = 1, Name = ".NET User Group Bern", Details = "https://www.dnug-bern.ch/", DetailsType = "Work" 
        };
        var dieci = new MapDetail 
        { 
            DetailsCoordinates = GeoLocation.LatitudeLongitude(new LatLonGeoLocation { Lon = 7.41148, Lat = 46.94450 }), 
            Id = 2, Name = "Dieci Pizzakurier Bern", Details = "https://www.dieci.ch", DetailsType = "Pizza" 
        };
        var babylonKoeniz = new MapDetail 
        { 
            DetailsCoordinates = GeoLocation.LatitudeLongitude(new LatLonGeoLocation { Lon = 7.41635, Lat = 46.92737 }), 
            Id = 3, Name = "PIZZERIA BABYLON Köniz", Details = "https://www.pizza-babylon.ch/home-k.html", DetailsType = "Pizza" 
        };
        var babylonOstermundigen = new MapDetail 
        { 
            DetailsCoordinates = GeoLocation.LatitudeLongitude(new LatLonGeoLocation { Lon = 7.48256, Lat = 46.95578 }), 
            Id = 4, Name = "PIZZERIA BABYLON Ostermundigen", Details = "https://www.pizza-babylon.ch/home-o.html", DetailsType = "Pizza" 
        };

        var exist = await _client.Indices.ExistsAsync(IndexName);
        if (exist.Exists)
        {
            await _client.Indices.DeleteAsync(IndexName);
        }

        var response1 = await _client.Indices.CreateAsync<MapDetail>(IndexName, c => c
           .Mappings(map => map
               .Properties(
                   new Properties<MapDetail>()
                   {
                        { "details", new TextProperty() },
                        { "detailsCoordinates", new GeoPointProperty() },
                        { "detailsType", new TextProperty() },
                        { "id", new TextProperty() },
                        { "information", new TextProperty() },
                        { "name", new TextProperty() }
                   }
               )
           )
        );

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
    public async Task<List<MapDetail>> SearchForClosestAsync(uint maxDistanceInMeter, double centerLatitude, double centerLongitude)
    {
        // Bern	Lat 46.94792, Long 7.44461
        if (maxDistanceInMeter == 0)
        {
            maxDistanceInMeter = 1000000;
        }
        var searchRequest = new SearchRequest(IndexName)
        {
            // Query = new MatchAllQuery{},
            Query = new GeoDistanceQuery
            {
                DistanceType = GeoDistanceType.Plane,
                Field = "detailsCoordinates",
                Distance = $"{maxDistanceInMeter}m",
                Location = GeoLocation.LatitudeLongitude(new LatLonGeoLocation
                {
                    Lat = centerLatitude,
                    Lon = centerLongitude
                })
            },
            Sort = BuildGeoDistanceSort(centerLatitude, centerLongitude)
        };

        searchRequest.ErrorTrace = true;

        _logger.LogInformation("SearchForClosestAsync: {SearchBody}", searchRequest);

        var searchResponse = await _client.SearchAsync<MapDetail>(searchRequest);

        
        return searchResponse.Documents.ToList();
    }

    // "sort" : [
    //		{
    //			"_geo_distance" : {
    //				"detailscoordinates" : [7.41148,46.9445],
    //				"order" : "asc",
    //				"unit" : "km"
    //			}
    //		}
    //	]
    private static List<SortOptions> BuildGeoDistanceSort(double centerLatitude, double centerLongitude)
    {
        var sorts = new List<SortOptions>();

        var sort = SortOptions.GeoDistance(
            new GeoDistanceSort 
            {
                Field = new Field("detailsCoordinates"),
                Location = new List<GeoLocation>
                { 
                    GeoLocation.LatitudeLongitude(new LatLonGeoLocation
                    {
                        Lat = centerLatitude,
                        Lon = centerLongitude
                    })
                },
                Order = SortOrder.Asc,
                Unit = DistanceUnit.Meters
            }
        );

        sorts.Add(sort);

        return sorts;
    }
}