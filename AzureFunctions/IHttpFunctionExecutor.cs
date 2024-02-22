using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AzureFunctions
{
    public interface IHttpFunctionExecutor
    {
        Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> func);
    }
    
}

