using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Ordering.Api.Features.Orders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure JSON serialization for enums
builder.Services.Configure<JsonOptions>(options =>
{
    // Convert all enums to strings in JSON
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // default NumberHandling is JsonNumberHandling.AllowReadingFromString but swagger-ui has an issue with it.
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Map endpoints using vertical slice approach
app.MapGetOrders();
app.MapGetOrderById();
app.MapCreateOrder();
app.MapUpdateOrderStatus();
app.MapDeleteOrder();

app.Run();