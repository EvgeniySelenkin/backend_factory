using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.IO;
using System.Text.Json;
using WebApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text;
using WebApi.Odt;
using System.Diagnostics.Eventing.Reader;
using Microsoft.EntityFrameworkCore;

namespace WebApi.BackgroundServices
{
    public class EventBackgroundService : IHostedService
    {
        private Timer _timer;
        private readonly IServiceProvider services;
        private IConfiguration configuration;
        static readonly HttpClient client = new HttpClient();

        public EventBackgroundService(IServiceProvider services, IConfiguration configuration)
        {
            this.services = services;
            this.configuration = configuration;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DBContext>();
                var unitsId = new List<int> {1,2,3}; //configuration.GetValue<List<int>>("UnitId");
                foreach (var id in unitsId)
                {
                    int skip = 0;
                    int take = 50;
                    var EventsId = new List<int>();
                    var Events = new List<Event>();
                    do
                    {
                        var url = "http://localhost:5000/api/events/keys" + "?" + $"unitId={id}&take={take}&skip={skip}";
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        EventsId.AddRange(JsonSerializer.Deserialize<List<int>>(responseBody));

                        string Id = JsonSerializer.Serialize(EventsId);
                        var content = new StringContent(Id, Encoding.UTF8, "application/json") /*new FormUrlEncodedContent(new[] { new KeyValuePair < string, string > ("", Id) })*/;
                        url = "http://localhost:5000/api/events";
                        var result = await client.PostAsync(url, content);
                        var jsonString = await result.Content.ReadAsStringAsync();

                        Events.AddRange(JsonSerializer.Deserialize<List<Event>>(jsonString));


                        skip += 50;
                    } while (take == EventsId.Count);

                    foreach (var elem in Events)
                    {
                        var elemOdt = new EventOdt()
                        {
                            IsActive = elem.IsActive,
                            StorageValue = elem.StorageValue,
                            Name = elem.Name,
                            UnitId = elem.UnitId,
                            Description = elem.Description,
                            Latitude = elem.Latitude,
                            Longituted = elem.Longituted,
                            Tags = JsonSerializer.Serialize(elem.Tags),
                            ResponsibleOperators = JsonSerializer.Serialize(elem.ResponsibleOperators)

                        };
                        
                        var findEvent = db.Event.FirstOrDefault(x => x.Name == elemOdt.Name);
                        if (findEvent == null)
                        {
                            db.Event.Add(elemOdt);
                            db.SaveChanges();
                        }
                        else if (elem.IsActive)
                        {
                            db.Event.Update(elemOdt);
                            db.SaveChanges();
                        }
                
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
