namespace Ordering.Api.Features.Orders;

public static class GetOrders
{
    public static IEndpointRouteBuilder MapGetOrders(this IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", () => OrderStorage.Orders)
            .WithName("GetOrders")
            .WithTags("Orders")
            .WithSummary("Retrieve all orders")
            .WithDescription("Returns a list of all orders in the system with their details including customer information, items, and status.");

        return app;
    }
}
