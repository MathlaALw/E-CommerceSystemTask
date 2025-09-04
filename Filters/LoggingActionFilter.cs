using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_CommerceSystem.Filters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly IAppLogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(IAppLogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Executing action {ActionName} on controller {ControllerName}",
                context.ActionDescriptor.DisplayName,
                context.Controller.GetType().Name);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                _logger.LogError(context.Exception, "Error in action {ActionName} on controller {ControllerName}",
                    context.ActionDescriptor.DisplayName,
                    context.Controller.GetType().Name);
            }
            else
            {
                _logger.LogInformation("Successfully executed action {ActionName} on controller {ControllerName}",
                    context.ActionDescriptor.DisplayName,
                    context.Controller.GetType().Name);
            }
        }
    }
}
