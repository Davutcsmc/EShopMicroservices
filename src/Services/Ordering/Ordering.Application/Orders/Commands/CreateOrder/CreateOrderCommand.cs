
using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid id);


public class CreateOrderCommandValidatior : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidatior()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("CustomerId is required");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty");
    }
}