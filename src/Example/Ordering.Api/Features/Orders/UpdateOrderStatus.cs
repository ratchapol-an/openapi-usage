namespace Ordering.Api.Features.Orders;

public static class UpdateOrderStatus
{
    public record Request(OrderStatus Status);

    public static IEndpointRouteBuilder MapUpdateOrderStatus(this IEndpointRouteBuilder app)
    {
        app.MapPut("/orders/{id}/status", (int id, Request request) =>
            {
                var order = OrderStorage.Orders.FirstOrDefault(o => o.Id == id);
                if (order is null) return Results.NotFound();

                var updatedOrder = order with { Status = request.Status };

                var index = OrderStorage.Orders.IndexOf(order);
                OrderStorage.Orders[index] = updatedOrder;

                return Results.Ok(updatedOrder);
            })
            .WithName("UpdateOrderStatus");

        return app;
    }
}
