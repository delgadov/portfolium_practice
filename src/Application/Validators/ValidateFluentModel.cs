using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace portfolium.Application.Validators;

public class ValidateFluentModel : IAsyncActionFilter{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
        foreach (var arg in context.ActionArguments.Values) {
            if (arg == null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(arg.GetType());
            var validator = context.HttpContext.RequestServices.GetService(validatorType);

            if (validator is not IValidator fluentValidator) continue;

            var result = await fluentValidator.ValidateAsync(new ValidationContext<object>(arg));
            if (result.IsValid) continue;

            context.Result = new BadRequestObjectResult(result.Errors.Select(e => e.ErrorMessage));

            return;
        }

        await next();
    }

}