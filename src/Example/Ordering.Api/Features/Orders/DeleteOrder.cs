namespace Ordering.Api.Features.Orders;

public static class DeleteOrder
{
    public static IEndpointRouteBuilder MapDeleteOrder(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id}", (int id) =>
            {
                var order = OrderStorage.Orders.FirstOrDefault(o => o.Id == id);
                if (order is null) return Results.NotFound();

                OrderStorage.Orders.Remove(order);
                return Results.NoContent();
            })
            .WithName("DeleteOrder");

        return app;
    }
}
