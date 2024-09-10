namespace Movies.Api.Endpoints.Ratings;

public static class RatingEndpointExtensions
{
    public static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDeleteRating();
        app.MapGetUserRatings();
        app.MapRateMovie();
        return app;
    }
}