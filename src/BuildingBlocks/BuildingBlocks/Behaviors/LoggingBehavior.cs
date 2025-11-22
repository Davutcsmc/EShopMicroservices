using BuildingBlocks.CQRS;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handling request={@Request} - Response={Response} - RequestData={RequestData}",
                typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = System.Diagnostics.Stopwatch.StartNew();

            var response = await next();

            var elapsedMs = timer.ElapsedMilliseconds;

            if (elapsedMs > 3000) 
            {
                logger.LogWarning("[PERFORMANCE] Handled request={@Request} with Response={Response} and took ElapsedMs={ElapsedMs}ms",
                    typeof(TRequest).Name, typeof(TResponse).Name, elapsedMs);
            }
            else
            {
                logger.LogInformation("[END] Handled request={@Request} with Response={Response} - ElapsedMs={ElapsedMs}ms",
                    typeof(TRequest).Name, typeof(TResponse).Name, elapsedMs);
            }

            return response;
        }
    }
}
