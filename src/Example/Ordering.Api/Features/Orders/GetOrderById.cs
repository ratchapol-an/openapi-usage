using System.ComponentModel;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ordering.Api.Features.Orders;

public static class GetOrderById
{
    public static IEndpointRouteBuilder MapGetOrderById(this IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{id}", Results<Ok<Order>, NotFound> ([Description("Order ID")]int id) =>
            {
                var order = OrderStorage.Orders.FirstOrDefault(o => o.Id == id);
                return order is not null ? TypedResults.Ok(order) : TypedResults.NotFound();
            })
            .WithName("GetOrderById")
            .WithTags("Orders")
            .WithSummary("Retrieve a specific order by ID")
            .WithDescription("Returns the details of a single order identified by its unique ID. Returns 404 if the order does not exist.");

        return app;
    }
}
