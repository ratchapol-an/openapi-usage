using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ordering.Api.Features.Orders;

public static class CreateOrder
{
    public record CreateOrderRequest(
        [property: Description("Full name of the customer")]
        [property: Required]
        [property: StringLength(100, MinimumLength = 2)]
        string CustomerName,

        [property: Description("Customer's email address")]
        [property: Required]
        [property: EmailAddress]
        string CustomerEmail,

        [property: Description("List of items to order")]
        [property: Required]
        [property: MinLength(1)]
        List<OrderItemRequest> Items
    );
    
    public record OrderItemRequest(
        [property: Description("Name of the product")]
        [property: Required]
        [property: StringLength(200, MinimumLength = 1)]
        string ProductName,

        [property: Description("Quantity to order")]
        [property: Required]
        [property: Range(1, 1000)]
        int Quantity,

        [property: Description("Price per unit in USD")]
        [property: Required]
        [property: Range(0.01, 999999.99)]
        decimal UnitPrice
    );

    public static IEndpointRouteBuilder MapCreateOrder(this IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", Created<Order> (CreateOrderRequest request) =>
            {
                var items = request.Items.Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList();

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
                return TypedResults.Created($"/orders/{newOrder.Id}", newOrder);
            })
            .WithName("CreateOrder")
            .WithTags("Orders")
            .WithSummary("Create a new order")
            .WithDescription("Creates a new order with customer information and order items. The total amount is automatically calculated based on item quantities and prices. The order is created with a Pending status.");

        return app;
    }
}
