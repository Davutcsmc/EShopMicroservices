
using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints
{
    // - Accepts a UpdateOrderRequest request
    // - Maps the request to a UpdateOrderCommand
    // - User MEdiatr to send the command to the appropriate handler
    // - Returns the result of the command execution

    public record UpdateOrderRequest(OrderDto Order);
    public record UpdateOrderResponse(bool IsSuccessful);

    public class UpdateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateOrderCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateOrderResponse>();

                return Results.Ok(response);
            })
                .WithName("UpdateOrder")
                .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Updates an existing order")
                .WithDescription("Updates an existing order in the system.");
        }
    }
}
