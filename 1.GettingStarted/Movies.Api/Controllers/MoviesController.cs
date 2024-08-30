using Microsoft.AspNetCore.Mvc;
using Movies.Application.Repositories;

namespace Movies.Api.Controllers;

[ApiController]
[Route("api")]
public class MoviesController:ControllerBase
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
}