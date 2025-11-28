using System.ComponentModel;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

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
            .WithDescription("Returns the details of a single order identified by its unique ID. Returns 404 if the order does not exist.")
            .AddOpenApiOperationTransformer((operation, context, _) =>
            {
                // Get configured JsonOptions from DI
                var jsonOptions = context.ApplicationServices.GetRequiredService<IOptions<JsonOptions>>();

                // Add response examples
                var successExample = new Order
                {
                    Id = 123,
                    CustomerName = "John Doe",
                    CustomerEmail = "john@example.com",
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Shipped,
                    Items = [new() { ProductName = "Laptop", Quantity = 1, UnitPrice = 999.99m }],
                    TotalAmount = 999.99m
                };

                var pendingExample = new Order
                {
                    Id = 456,
                    CustomerName = "Jane Smith",
                    CustomerEmail = "jane@example.com",
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    Items = [new() { ProductName = "Mouse", Quantity = 2, UnitPrice = 25.00m }],
                    TotalAmount = 50.00m
                };

                if (operation.Responses != null &&
                    operation.Responses.TryGetValue("200", out var response) &&
                    response.Content != null &&
                    response.Content.TryGetValue("application/json", out var mediaType))
                {
                    mediaType.Examples = new Dictionary<string, IOpenApiExample>
                    {
                        ["success"] = new OpenApiExample
                        {
                            Summary = "Successful order retrieval",
                            Description = "Example of a shipped order with a single laptop item",
                            Value = JsonSerializer.SerializeToNode(successExample, jsonOptions.Value.SerializerOptions)
                        },
                        ["pending"] = new OpenApiExample
                        {
                            Summary = "Pending order",
                            Description = "Example of a pending order awaiting confirmation",
                            Value = JsonSerializer.SerializeToNode(pendingExample, jsonOptions.Value.SerializerOptions)
                        }
                    };
                }

                return Task.CompletedTask;
            });

        return app;
    }
}
