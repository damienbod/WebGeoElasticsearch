using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using WebGeoElasticsearch.ElasticsearchApi;
using WebGeoElasticsearch.Models;

namespace WebGeoElasticsearch.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SearchProvider _searchProvider;

    public HomeController(ILogger<HomeController> logger, SearchProvider searchProvider)
    {
        _logger = logger;
        _searchProvider = searchProvider;
    }

    public async Task<IActionResult> IndexAsync()
    {
        // TODO : Uncomment this line to add data to Elasticsearch
        // TODO move to service
        // await _searchProvider.AddMapDetailDataAsync();

        var searchResult = await _searchProvider.SearchForClosestAsync(0, 46.94792, 7.44461);
        var mapModel = new MapModel
        {
            MapData = JsonSerializer.Serialize(searchResult),
            // Bern	Lat 46.94792, Long 7.44461
            CenterLatitude = 46.94792,
            CenterLongitude = 7.44461,
            MaxDistanceInMeter = 0
        };

        return View(mapModel);
    }

    public async Task<IActionResult> SearchAsync(uint maxDistanceInMeter, double centerLatitude, double centerLongitude)
    {
        var searchResult = await _searchProvider.SearchForClosestAsync(maxDistanceInMeter, centerLatitude, centerLongitude);
        var mapModel = new MapModel
        {
            MapData = JsonSerializer.Serialize(searchResult),
            CenterLatitude = centerLatitude,
            CenterLongitude = centerLongitude,
            MaxDistanceInMeter = maxDistanceInMeter
        };

        return View("Index", mapModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
