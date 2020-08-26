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
                        /*HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        EventsId.AddRange(JsonSerializer.Deserialize<List<int>>(responseBody));*/

                        var url = "http://localhost:5001/api/events/keys" + "?" + $"unitId={id}&take={take}&skip={skip}";
                        WebRequest request = WebRequest.Create(url);
                        request.AuthenticationLevel = 0;
                        WebResponse response = await request.GetResponseAsync();

                        using (var dataStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(dataStream);
                            var jsonString = reader.ReadToEnd();
                            EventsId.AddRange(JsonSerializer.Deserialize<List<int>>(jsonString));
                        }

                        WebRequest requestPost = WebRequest.Create(@"http://localhost:5001/api/events}");
                        WebResponse responsePost = await requestPost.GetResponseAsync();
                        requestPost.Method = "POST";
                        string Id = JsonSerializer.Serialize(EventsId);
                        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(Id);
                        requestPost.ContentType = "application/json";
                        requestPost.ContentLength = byteArray.Length;
                        using (Stream dataStream1 = requestPost.GetRequestStream())
                        {
                            dataStream1.Write(byteArray, 0, byteArray.Length);

                        }
                        using (Stream stream = responsePost.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                var jsonString = reader.ReadToEnd();
                                Events.AddRange(JsonSerializer.Deserialize<List<Event>>(jsonString));
                            }
                        }
                        skip += 50;
                    } while (take == EventsId.Count);

                    foreach (var elem in Events)
                    {
                        var findEvent = db.Event.FirstOrDefault(x => x.Name == elem.Name);
                        if (findEvent == null)
                        {
                            db.Event.Add(elem);
                            db.SaveChangesAsync();
                        }
                        else if (elem.IsActive)
                        {
                            db.Event.Update(elem);
                            db.SaveChangesAsync();
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
