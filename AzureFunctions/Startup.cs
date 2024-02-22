using FluentValidation;
using AzureFunctions.Domain;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using AzureFunctions.Domain.Behaviours;
using System.Reflection;

[assembly: FunctionsStartup(typeof(AzureFunctions.Startup))]
namespace AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddMediatR(typeof(GetFileQueryHandler));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddSingleton<IValidator<GetFileQuery>, GetFileQueryValidator>();
            builder.Services.AddSingleton<IHttpFunctionExecutor, HttpFunctionExecutor>();
        }
    }
}
