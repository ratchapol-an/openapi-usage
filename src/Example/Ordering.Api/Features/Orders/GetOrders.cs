namespace Ordering.Api.Features.Orders;

public static class GetOrders
{
    public static IEndpointRouteBuilder MapGetOrders(this IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", () => OrderStorage.Orders)
            .WithName("GetOrders");

        return app;
    }
}
