using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Movies.Api.Sdk;
using Movies.Contracts.Requests;

//var moviesApi=RestService.For<IMoviesApi>("https://localhost:7026");

var services = new ServiceCollection();
services.AddRefitClient<IMoviesApi>()
    .ConfigureHttpClient(x=>
        x.BaseAddress=new Uri("https://localhost:7026"));

var provider = services.BuildServiceProvider();
var moviesApi = provider.GetRequiredService<IMoviesApi>();
//var movie=await moviesApi.GetMoviesAsync("nick-the-greek-2023");

var request = new GetAllMoviesRequest
{
    Title = null,
    Year = null,
    SortBy = null,
    Page = 1,
    PageSize = 10
};

var movies = await moviesApi.GetAllMoviesAsync(request);

foreach (var movieResponse in movies.Items)
{
    Console.WriteLine(JsonSerializer.Serialize(movieResponse));
}
