namespace Ordering.Api.Features.Orders;

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5,
    Refunded = 6
}

public record OrderItem(
    string ProductName,
    int Quantity,
    decimal UnitPrice
);

public record Order(
    int Id,
    string CustomerName,
    string CustomerEmail,
    DateTime OrderDate,
    OrderStatus Status,
    List<OrderItem> Items,
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
                new("Laptop", 1, 999.99m),
                new("Mouse", 2, 25.00m)
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
                new("Wireless Keyboard", 1, 79.99m),
                new("USB-C Cable", 3, 12.99m)
            },
            TotalAmount: 118.96m
        )
    };
}
