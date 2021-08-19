using Inside.TestTask.MC1.Model;

namespace Inside.TestTask.MC1.Services
{
    public interface ITimerService
    {
        public Task StartAsync();
        public Task<Statistics> StopAsync();
    }
}