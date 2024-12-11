using System;
using CinemaApp.Services.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CinemaApp.Web.Infrastructure.BackgroundTasks
{
	public class RemovePassedEventsAndAddRunnedDistance : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public RemovePassedEventsAndAddRunnedDistance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Run the first notification check immediately, then schedule it to repeat daily.
            _timer = new Timer(GenerateNotifications, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void GenerateNotifications(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var rankListService = scope.ServiceProvider.GetRequiredService<IRankListService>();
                await rankListService.DeletePassedEventsAndRunnedDistanceToTheParticipants();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

