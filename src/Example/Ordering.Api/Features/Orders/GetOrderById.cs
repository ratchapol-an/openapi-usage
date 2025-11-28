namespace Ordering.Api.Features.Orders;

public static class GetOrderById
{
    public static IEndpointRouteBuilder MapGetOrderById(this IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{id}", (int id) =>
            {
                var order = OrderStorage.Orders.FirstOrDefault(o => o.Id == id);
                return order is not null ? Results.Ok(order) : Results.NotFound();
            })
            .WithName("GetOrderById");

        return app;
    }
}
