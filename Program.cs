using Assignment1_Middleware.CustomMiddleware;

namespace Assignment1_Middleware
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.UseLoginMiddleware();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("No response");
            });

            app.Run();
        }
    }
}
