namespace Ordering.Api.Features.Orders;

public static class CreateOrder
{
    public record Request(
        string CustomerName,
        string CustomerEmail,
        List<OrderItemRequest> Items
    );

    public record OrderItemRequest(
        string ProductName,
        int Quantity,
        decimal UnitPrice
    );

    public static IEndpointRouteBuilder MapCreateOrder(this IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", (Request request) =>
            {
                var items = request.Items.Select(i => new OrderItem(
                    i.ProductName,
                    i.Quantity,
                    i.UnitPrice
                )).ToList();

                var totalAmount = items.Sum(i => i.Quantity * i.UnitPrice);

                var newOrder = new Order(
                    Id: OrderStorage.Orders.Count > 0 ? OrderStorage.Orders.Max(o => o.Id) + 1 : 1,
                    CustomerName: request.CustomerName,
                    CustomerEmail: request.CustomerEmail,
                    OrderDate: DateTime.UtcNow,
                    Status: OrderStatus.Pending,
                    Items: items,
                    TotalAmount: totalAmount
                );

                OrderStorage.Orders.Add(newOrder);
                return Results.Created($"/orders/{newOrder.Id}", newOrder);
            })
            .WithName("CreateOrder");

        return app;
    }
}
