using System.Timers;

namespace SteamNexus_Server.Services
{
    public class ScheduledTaskService
    {
        private readonly IServiceScopeFactory _scopeFactory; 
        private System.Timers.Timer _timer;

        public ScheduledTaskService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _timer = new System.Timers.Timer();
        }

        public void StartTimer()
        {
            SetDailyTimer();
        }

        private void SetDailyTimer()
        {
            DateTime now = DateTime.Now;
            DateTime nextRun = DateTime.Today.AddHours(2).AddMinutes(1); // 今天的下午6:28
            if (now > nextRun)
            {
                nextRun = nextRun.AddMinutes(1); // 如果已经过了今天的时间，就设置为明天的6:28
            }

            double firstInterval = (nextRun - now).TotalMilliseconds;
            _timer = new System.Timers.Timer(firstInterval);
            _timer.Elapsed += async (sender, e) => await OnTimedEvent(sender, e);
            _timer.AutoReset = true; //是否重複執行
            _timer.Start();
        }

        private async Task OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var gamePriceToDB = scope.ServiceProvider.GetRequiredService<GamePriceToDB>();
                    await gamePriceToDB.GetGamePriceDataToDB();
                }

                _timer.Interval = TimeSpan.FromHours(24).TotalMilliseconds; //設定下一次的時間
                _timer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
