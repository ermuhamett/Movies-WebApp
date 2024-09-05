using System.Text.Json;
using Refit;
using Movies.Api.Sdk;

var moviesApi=RestService.For<IMoviesApi>("https://localhost:7026");
var movie=await moviesApi.GetMoviesAsync("nick-the-greek-2023");
Console.WriteLine(JsonSerializer.Serialize(movie));