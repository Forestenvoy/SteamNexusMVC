using System.Timers;

namespace SteamNexus_Server.Services
{
    public class ScheduledTaskService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private System.Timers.Timer _priceDailyTimer;
        private System.Timers.Timer _peopleHalfHourTimer;
        private System.Timers.Timer _NumberOfCommentsDailyTimer;

        public ScheduledTaskService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _priceDailyTimer = new System.Timers.Timer();
            _peopleHalfHourTimer = new System.Timers.Timer();
            _NumberOfCommentsDailyTimer = new System.Timers.Timer();
        }

        public void StartPriceDailyTimer()
        {
            setPriceDailyTimer();
        }
        public void StartPeopleTimer()
        {
            setPeopleHalfHourTimer();
        }
        public void StartNumberOfCommentsTimer()
        {
            setNumberOfCommentsDailyTimer();
        }


        public void StopPriceDailyTimer()
        {
            _priceDailyTimer.Stop();
        }
        public void StoppeopleHalfHourTimer()
        {
            _peopleHalfHourTimer.Stop();
        }
        public void StoptNumberOfCommentsTimer()
        {
            _NumberOfCommentsDailyTimer.Stop();
        }


        private void setPriceDailyTimer()
        {
            DateTime now = DateTime.Now;
            DateTime nextRun = DateTime.Today.AddHours(2).AddMinutes(1); // 今天的下午6:28
            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1); // 如果已经过了今天的时间，就设置为明天的6:28
            }

            double firstInterval = (nextRun - now).TotalMilliseconds;
            _priceDailyTimer = new System.Timers.Timer(firstInterval);
            _priceDailyTimer.Elapsed += async (sender, e) => await OnDailyTimedEvent(sender, e);
            _priceDailyTimer.AutoReset = true; //是否重複執行
            _priceDailyTimer.Start();
        }
        private async Task OnDailyTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var gamePriceToDB = scope.ServiceProvider.GetRequiredService<GameTimer>();
                    await gamePriceToDB.GetGamePriceDataToDB();
                }

                _priceDailyTimer.Interval = TimeSpan.FromHours(24).TotalMilliseconds; //設定下一次的時間
                _priceDailyTimer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        private void setPeopleHalfHourTimer()
        {
            double halfHourInterval = TimeSpan.FromMinutes(30).TotalMilliseconds;
            _peopleHalfHourTimer = new System.Timers.Timer(halfHourInterval);
            _peopleHalfHourTimer.Elapsed += async (sender, e) => await OnHalfHourTimedEvent(sender, e);
            _peopleHalfHourTimer.AutoReset = true; // 設置為 true，以便每半小時自動重置
            _peopleHalfHourTimer.Start();

            // 手動調用一次，立即執行第一次
            Task.Run(async () => await OnHalfHourTimedEvent(null, null));
        }
        private async Task OnHalfHourTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var gamePriceToDB = scope.ServiceProvider.GetRequiredService<GameTimer>();
                    await gamePriceToDB.GetOnlineUsersDataToDB();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        private void setNumberOfCommentsDailyTimer()
        {
            DateTime now = DateTime.Now;
            DateTime nextRun = DateTime.Today.AddHours(2).AddMinutes(1); // 今天的下午6:28
            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1); // 如果已经过了今天的时间，就设置为明天的6:28
            }

            double firstInterval = (nextRun - now).TotalMilliseconds;
            _NumberOfCommentsDailyTimer = new System.Timers.Timer(firstInterval);
            _NumberOfCommentsDailyTimer.Elapsed += async (sender, e) => await NumberOfCommentsDailyTimerTimedEvent(sender, e);
            _NumberOfCommentsDailyTimer.AutoReset = true; // 設置為 true，以便每半小時自動重置
            _NumberOfCommentsDailyTimer.Start();

            
        }
        private async Task NumberOfCommentsDailyTimerTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var gamePriceToDB = scope.ServiceProvider.GetRequiredService<GameTimer>();
                    await gamePriceToDB.GetNumberOfCommentsDataToDB();

                    _priceDailyTimer.Interval = TimeSpan.FromHours(24).TotalMilliseconds; //設定下一次的時間
                    _priceDailyTimer.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }


    }
}
