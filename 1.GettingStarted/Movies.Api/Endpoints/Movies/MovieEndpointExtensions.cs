﻿namespace Movies.Api.Endpoints.Movies;

public static class MovieEndpointExtensions
{
    public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetMovie();
        app.MapCreateMovie();
        app.MapGetAllMovies();
        /*
        app.MapUpdateMovies();
        app.MapDeleteMovies();*/
        return app;
    }
}