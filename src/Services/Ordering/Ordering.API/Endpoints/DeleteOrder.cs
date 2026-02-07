
using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints
{
    // - Accepts the orderId as a parameter
    // - Contructs a DeleteOrderCommand
    // - User MEdiatr to send the command to the appropriate handler
    // - Returns a success or fail state 

    //public record DeleteOrderRequest(Guid Id);
    public record DeleteOrderResponse(bool IsSuccessful);

    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{id:guid}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteOrderCommand(id);

                var result = await sender.Send(command);

                var response = result.Adapt<DeleteOrderResponse>();

                return response.IsSuccessful
                    ? Results.Ok(new DeleteOrderResponse(true))
                    : Results.NotFound(new DeleteOrderResponse(false));
            })
                .WithName("DeleteOrder")
                .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)                
                .WithSummary("Deletes an order by its ID.")
                .WithDescription("Deletes an order by its ID. Returns a success response if the order was deleted, or a not found response if the order does not exist.");
        }
    }
}
