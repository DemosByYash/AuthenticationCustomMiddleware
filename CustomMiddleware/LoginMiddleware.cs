using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Assignment1_Middleware.CustomMiddleware
{
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;
        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/" && context.Request.Method == "POST")
            {
                StreamReader reader = new StreamReader(context.Request.Body);
                string Body = await reader.ReadToEndAsync();
                Dictionary<string, StringValues> queryDict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(Body);
                string? Email = null;
                string? Password = null;

                if (queryDict.ContainsKey("Email"))
                {
                    Email = queryDict["Email"][0].ToString();

                }
                else
                {
                    context.Response.StatusCode = 400; // Bad Request
                    context.Response.WriteAsync("Email is required.");
                }
                if (queryDict.ContainsKey("Password"))
                {
                    Password = queryDict["Password"][0].ToString();
                }
                else
                {
                    context.Response.StatusCode = 400; // Bad Request
                    context.Response.WriteAsync("Password is required.");
                }
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("Email and Password are required.");
                    return;
                }

                if (Email == "admin@example.com" && Password == "admin1234")
                {
                    context.Response.StatusCode = 200; // OK
                    await context.Response.WriteAsync("Login successful.");
                }
                else
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Invalid credentials.");
                }
            }
            else
            {
                await _next(context);
            }
        }
    }

    public static class LoginMiddlewareExtensions 
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
