using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromMemory(GetRoutes(), GetClusters());

var app = builder.Build();

app.MapReverseProxy();

app.Run();

static RouteConfig[] GetRoutes()
{
    return [
        new RouteConfig
        {
            RouteId = "author-route",
            ClusterId = "author-cluster",
            Match = new RouteMatch { Path = "/api/authors/{**catch-all}" }
        },
        new RouteConfig
        {
            RouteId = "book-route",
            ClusterId = "book-cluster",
            Match = new RouteMatch { Path = "/api/books/{**catch-all}" }
        },
        new RouteConfig
        {
            RouteId = "shoppingcart-route",
            ClusterId = "shoppingcart-cluster",
            Match = new RouteMatch { Path = "/api/shopping-carts/{**catch-all}" }
        }
    ];
}

static ClusterConfig[] GetClusters()
{
    return [
        new ClusterConfig
        {
            ClusterId = "author-cluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                ["author"] = new() { Address = "http://localhost:5261/" }
            }
        },
        new ClusterConfig
        {
            ClusterId = "book-cluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                ["book"] = new() { Address = "http://localhost:5006/" }
            }
        },
        new ClusterConfig
        {
            ClusterId = "shoppingcart-cluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                ["shoppingcart"] = new() { Address = "http://localhost:5290/" }
            }
        }
    ];
}
