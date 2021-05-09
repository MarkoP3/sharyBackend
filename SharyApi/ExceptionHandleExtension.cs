using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SharyApi
{
    public static class ExceptionHandleExtension
    {
        public static void ConfigureErrorHandling(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseExceptionHandler(
               options =>
                {
                    options.Run(async context =>
                     {
                         context.Response.StatusCode = 500;
                         var exception = context.Features.Get<IExceptionHandlerFeature>();
                         if (exception != null)
                         {
                             await context.Response.WriteAsync(exception.Error.Message);
                         }

                     });
                }
                );
        }
    }
}
