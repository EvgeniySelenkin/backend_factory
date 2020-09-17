using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi
{
    public class BackgroundService : IHostedService
    {
        private readonly Logger _logger;
        private readonly IServiceProvider services;
        private Timer _timer;

        public BackgroundService(IServiceProvider services)
        {
            _logger = LogManager.GetCurrentClassLogger();
            this.services = services;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Info("Timed Hosted Service running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using(var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<TankRepository>();
                var tanks = await repo.GetAll();
                Random rnd = new Random();
                foreach (var tank in tanks)
                {
                    tank.Volume = tank.Volume * rnd.Next(9, 12) / 10;
                    if (tank.Volume > tank.MaxVolume)
                    {
                        _logger.Info($"Превышение объема резервуара {tank.Name}.");
                        tank.Volume = tank.MaxVolume;
                    }
                    else
                    {
                        _logger.Info($"Объем резервуара {tank.Name} изменился.");
                        await repo.Update(tank);
                    }
                }
            }
            

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Info("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}