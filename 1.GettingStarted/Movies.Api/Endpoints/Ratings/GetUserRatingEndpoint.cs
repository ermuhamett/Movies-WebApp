using Movies.Api.Auth;
using Movies.Application.Services;

namespace Movies.Api.Endpoints.Ratings;

public static class GetUserRatingEndpoint
{
    public const string Name = "GetUserRating";

    public static IEndpointRouteBuilder MapGetUserRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Ratings.GetUserRatings,
                async (HttpContext context, IRatingService ratingService,
                    CancellationToken token) =>
                {
                    var userId = context.GetUserId();
                    var ratings = await ratingService.GetRatingsForUserAsync(userId!.Value, token);
                    return TypedResults.Ok(ratings);
                })
            .WithName(Name)
            .RequireAuthorization();
        return app;
    }
}