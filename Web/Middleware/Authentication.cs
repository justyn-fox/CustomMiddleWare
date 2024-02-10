public class Authentication
{
    private readonly RequestDelegate _next;
    public Authentication(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        var username = context.Request.Query["username"];
        var password = context.Request.Query["password"];

        if (username == "user1" && password == "password1")
        {
            context.Request.HttpContext.Items.Add("userdetails", "CONGRATS!");
            await _next(context);
        }

        else
        {
            await context.Response.WriteAsync("Not authorized...");
        }
    }
}