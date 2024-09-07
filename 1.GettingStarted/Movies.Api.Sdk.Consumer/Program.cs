using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Movies.Api.Sdk;
using Movies.Api.Sdk.Consumer;
using Movies.Contracts.Requests;

//var moviesApi=RestService.For<IMoviesApi>("https://localhost:7026");

var services = new ServiceCollection();
services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>(s => new RefitSettings
    {
        AuthorizationHeaderValueGetter = async (request, cancellationToken) =>
        {
            var token = await s.GetRequiredService<AuthTokenProvider>().GetTokenAsync();
            Console.WriteLine($"Token used: {token}");
            return token;
        }
    })
    .ConfigureHttpClient(x =>
    {
        x.BaseAddress = new Uri("https://localhost:7026");
        x.DefaultRequestHeaders.Add("Accept", "application/json");
    });

var provider = services.BuildServiceProvider();
var moviesApi = provider.GetRequiredService<IMoviesApi>();

var movie = await moviesApi.GetMoviesAsync("nick-the-greek-2023");
Console.WriteLine("Movie retrieved successfully");

var newMovie = await moviesApi.CreateMovieAsync(new CreateMovieRequest
{
    Title = "The Boys",
    YearOfRelease = 2018,
    Genres = new[] { "Action" }
});
Console.WriteLine("Movie created successfully");

await moviesApi.UpdateMovieAsync(newMovie.Id, new UpdateMovieRequest
{
    Title = "The Boys",
    YearOfRelease = 2018,
    Genres = new[] { "Action", "Superheros" }
});
Console.WriteLine("Movie updated successfully");

await moviesApi.DeleteMovieAsync(newMovie.Id);
Console.WriteLine("Movie deleted successfully");

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