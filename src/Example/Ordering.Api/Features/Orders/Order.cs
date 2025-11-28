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

public class Order
{
    /// <summary>
    /// Unique order identifier
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Full name of the customer
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public required string CustomerName { get; init; }

    /// <summary>
    /// Customer's email address
    /// </summary>
    [Required]
    [EmailAddress]
    public required string CustomerEmail { get; init; }

    /// <summary>
    /// Date and time when the order was created
    /// </summary>
    public DateTime OrderDate { get; init; }

    /// <summary>
    /// Current status of the order
    /// </summary>
    public OrderStatus Status { get; init; }

    /// <summary>
    /// List of items in the order
    /// </summary>
    [Required]
    [MinLength(1)]
    public required List<OrderItem> Items { get; init; }

    /// <summary>
    /// Total order amount in USD
    /// </summary>
    [Required]
    [Range(0.01, 999999.99)]
    public decimal TotalAmount { get; init; }
}

public static class OrderStorage
{
    public static List<Order> Orders { get; } = new()
    {
        new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            CustomerEmail = "john.doe@example.com",
            OrderDate = DateTime.UtcNow.AddDays(-2),
            Status = OrderStatus.Processing,
            Items = new List<OrderItem>
            {
                new() { ProductName = "Laptop", Quantity = 1, UnitPrice = 999.99m },
                new() { ProductName = "Mouse", Quantity = 2, UnitPrice = 25.00m }
            },
            TotalAmount = 1049.99m
        },
        new Order
        {
            Id = 2,
            CustomerName = "Jane Smith",
            CustomerEmail = "jane.smith@example.com",
            OrderDate = DateTime.UtcNow.AddDays(-1),
            Status = OrderStatus.Shipped,
            Items = new List<OrderItem>
            {
                new() { ProductName = "Wireless Keyboard", Quantity = 1, UnitPrice = 79.99m },
                new() { ProductName = "USB-C Cable", Quantity = 3, UnitPrice = 12.99m }
            },
            TotalAmount = 118.96m
        }
    };
}
