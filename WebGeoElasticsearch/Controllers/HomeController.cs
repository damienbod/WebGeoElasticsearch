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

    public IActionResult Index()
    {
        var searchResult = _searchProvider.SearchForClosest(0, 7.44461, 46.94792);
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

    public IActionResult Search(uint maxDistanceInMeter, double centerLongitude, double centerLatitude)
    {
        var searchResult = _searchProvider.SearchForClosest(maxDistanceInMeter, centerLongitude, centerLatitude);
        var mapModel = new MapModel
        {
            MapData = JsonSerializer.Serialize(searchResult),
            CenterLongitude = centerLongitude,
            CenterLatitude = centerLatitude,
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
