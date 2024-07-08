namespace SteamNexus_Server.Middlewares
{
    public static class CustomMiddlewareExtensions
    {
        public static void UseCustom(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomMiddleware>();
        }
    }

    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        // "Scoped" SERVICE SHOULDN'T DO CONSTRUCTOR DI!!
        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // 設定回應內容類型 UTF-8
            context.Response.ContentType = "text/plain; charset=utf-8";
            // 宣告變數 紀錄 Request 開始時間
            var startTime = DateTime.Now;
            await context.Response.WriteAsync($"Request started at: {startTime}\r\n");

            await _next(context);

            var endTime = DateTime.Now;
            await context.Response.WriteAsync($"Request ended at: {endTime}\r\n");
            var duration = endTime - startTime;
            await context.Response.WriteAsync($"Request duration: {duration.TotalMilliseconds} ms\r\n");
        }
    }
}
