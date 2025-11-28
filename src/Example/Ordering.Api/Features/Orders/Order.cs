using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Features.Orders;

public enum OrderStatus
{
    [Description("Order is awaiting confirmation")]
    Pending = 0,
    [Description("Order has been confirmed by the customer")]
    Confirmed = 1,
    [Description("Order is being prepared for shipment")]
    Processing = 2,
    [Description("Order has been shipped to the customer")]
    Shipped = 3,
    [Description("Order has been successfully delivered")]
    Delivered = 4,
    [Description("Order was cancelled")]
    Cancelled = 5,
    [Description("Order payment has been refunded")]
    Refunded = 6
}

public class OrderItem
{
    /// <summary>
    /// Name of the product
    /// </summary>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public required string ProductName { get; init; }

    /// <summary>
    /// Quantity of items ordered
    /// </summary>
    [Required]
    [Range(1, 1000)]
    public required int Quantity { get; init; }

    /// <summary>
    /// Price per unit in USD
    /// </summary>
    [Required]
    [Range(0.01, 999999.99)]
    public required decimal UnitPrice { get; init; }
}

public record Order(
    [property: Description("Unique order identifier")]
    int Id,

    [property: Description("Full name of the customer")]
    [property: Required]
    [property: StringLength(100, MinimumLength = 2)]
    string CustomerName,

    [property: Description("Customer's email address")]
    [property: Required]
    [property: EmailAddress]
    string CustomerEmail,

    [property: Description("Date and time when the order was created")]
    DateTime OrderDate,

    [property: Description("Current status of the order")]
    OrderStatus Status,

    [property: Description("List of items in the order")]
    [property: Required]
    [property: MinLength(1)]
    List<OrderItem> Items,

    [property: Description("Total order amount in USD")]
    [property: Required]
    [property: Range(0.01, 999999.99)]
    decimal TotalAmount
);

public static class OrderStorage
{
    public static List<Order> Orders { get; } = new()
    {
        new Order(
            Id: 1,
            CustomerName: "John Doe",
            CustomerEmail: "john.doe@example.com",
            OrderDate: DateTime.UtcNow.AddDays(-2),
            Status: OrderStatus.Processing,
            Items: new List<OrderItem>
            {
                new() { ProductName = "Laptop", Quantity = 1, UnitPrice = 999.99m },
                new() { ProductName = "Mouse", Quantity = 2, UnitPrice = 25.00m }
            },
            TotalAmount: 1049.99m
        ),
        new Order(
            Id: 2,
            CustomerName: "Jane Smith",
            CustomerEmail: "jane.smith@example.com",
            OrderDate: DateTime.UtcNow.AddDays(-1),
            Status: OrderStatus.Shipped,
            Items: new List<OrderItem>
            {
                new() { ProductName = "Wireless Keyboard", Quantity = 1, UnitPrice = 79.99m },
                new() { ProductName = "USB-C Cable", Quantity = 3, UnitPrice = 12.99m }
            },
            TotalAmount: 118.96m
        )
    };
}
