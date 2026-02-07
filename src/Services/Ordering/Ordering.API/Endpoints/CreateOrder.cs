
namespace Ordering.API.Endpoints
{
    // - Accepts a CreateOrderRequest object
    // - Maps the request to a CreateOrderCommand
    // - User MEdiatr to send the command to the appropriate handler
    // - Returns the result of the command execution

    public record CreateOrderRequest(OrderDto Order);

    public record CreateOrderResponse(Guid Id);

    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOrderCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateOrderResponse>();

                return Results.Created($"/orders/{response.Id}", response);
            })
                .WithName("CreateOrder")
                .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Creates a new order")
                .WithDescription("Creates a new order with the provided details.");
        }
    }
}
