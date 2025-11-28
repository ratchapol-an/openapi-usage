using Microsoft.AspNetCore.Http.HttpResults;

namespace Ordering.Api.Features.Orders;

public static class DeleteOrder
{
    public static IEndpointRouteBuilder MapDeleteOrder(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id}", Results<NoContent, NotFound> (int id) =>
            {
                var order = OrderStorage.Orders.FirstOrDefault(o => o.Id == id);
                if (order is null) return TypedResults.NotFound();

                OrderStorage.Orders.Remove(order);
                return TypedResults.NoContent();
            })
            .WithName("DeleteOrder")
            .WithTags("Orders")
            .WithSummary("Delete an order")
            .WithDescription("Permanently deletes an order from the system. Returns 404 if the order does not exist.");

        return app;
    }
}
