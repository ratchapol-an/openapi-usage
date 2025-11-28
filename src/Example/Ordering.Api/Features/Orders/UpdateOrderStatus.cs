using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ordering.Api.Features.Orders;

public static class UpdateOrderStatus
{
    public record UpdateOrderStatusRequest(
        [property: Description("New status for the order")]
        [property: Required]
        OrderStatus Status
    );

    public static IEndpointRouteBuilder MapUpdateOrderStatus(this IEndpointRouteBuilder app)
    {
        app.MapPut("/orders/{id}/status", Results<Ok<Order>, NotFound> (int id, UpdateOrderStatusRequest request) =>
            {
                var order = OrderStorage.Orders.FirstOrDefault(o => o.Id == id);
                if (order is null) return TypedResults.NotFound();

                var updatedOrder = order with { Status = request.Status };

                var index = OrderStorage.Orders.IndexOf(order);
                OrderStorage.Orders[index] = updatedOrder;

                return TypedResults.Ok(updatedOrder);
            })
            .WithName("UpdateOrderStatus")
            .WithTags("Orders")
            .WithSummary("Update order status")
            .WithDescription("Updates the status of an existing order. Valid statuses include: Pending, Confirmed, Processing, Shipped, Delivered, Cancelled, and Refunded. Returns 404 if the order does not exist.");

        return app;
    }
}
