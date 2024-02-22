using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FluentValidation;
using System.Linq;

namespace AzureFunctions
{
    public class HttpFunctionExecutor : IHttpFunctionExecutor
    {
        public async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> func)
        {
            try
            {
                return await func();
            }
            catch (ValidationException ex)
            {
                var result = new
                {
                    message = "Validation failed.",
                    errors = ex.Errors.Select(x => new
                    {
                        x.PropertyName,
                        x.ErrorMessage,
                        x.ErrorCode
                    })
                };

                return new BadRequestObjectResult(result);
            }
        }
    }
}

