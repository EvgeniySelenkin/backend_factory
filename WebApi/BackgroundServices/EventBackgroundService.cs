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
using NLog;
using AutoMapper;

namespace WebApi.BackgroundServices
{
    public class EventBackgroundService : IHostedService
    {
        private Timer _timer;
        private readonly IServiceProvider services;
        private IConfiguration configuration;
        static readonly HttpClient client = new HttpClient();
        private bool syncFinish = true;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private IMapper mapper;

        public EventBackgroundService(IServiceProvider services, IConfiguration configuration, IMapper mapper)
        {
            this.services = services;
            this.configuration = configuration;
            this.mapper = mapper;
            logger.Info("Создан EventBackgroundService");
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.Info("Процесс начат");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            if(!syncFinish)
            {
                logger.Info("Синхронизация прервана");
                return;
            }
            logger.Info("Синхронизация началась.");
            syncFinish = false;
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DBContext>();
                var unitsId = JsonSerializer.Deserialize<List<int>>(configuration["UnitsId"]);
                var eventIdsInDb = db.Event.Select(u => u.EventId).ToList();
                foreach (var id in unitsId)
                {
                    int skip = 0;
                    int take = 1000;
                    int count = 0;
                    do
                    {
                        var EventsId = new List<int>();
                        var Events = new List<Event>();
                        var url = "http://localhost:5000/api/events/keys" + "?" + $"unitId={id}&take={take}&skip={skip}";
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        EventsId.AddRange(JsonSerializer.Deserialize<List<int>>(responseBody));

                        string Id = JsonSerializer.Serialize(EventsId);
                        var content = new StringContent(Id, Encoding.UTF8, "application/json");
                        url = "http://localhost:5000/api/events";
                        var result = await client.PostAsync(url, content);
                        var jsonString = await result.Content.ReadAsStringAsync();

                        Events.AddRange(JsonSerializer.Deserialize<List<Event>>(jsonString));
                        
                        //Выделяем элементы на добавление и обновление
                        var eventToAdd = Events.FindAll(u => !eventIdsInDb.Exists(y => y == u.Id ));
                        var eventToUpdate = Events.FindAll(u => eventIdsInDb.Exists(y => y == u.Id) && u.IsActive==true);
                        //Преобразуем в вид для БД
                        var eventToAddOdt = mapper.Map<List<EventOdt>>(eventToAdd);
                        var eventToUpdateOdt = mapper.Map<List<EventOdt>>(eventToUpdate);

                        db.Event.AddRange(eventToAddOdt);
                        db.SaveChanges();
                        
                        if(eventToUpdateOdt.Count != 0)
                        {
                            eventToUpdateOdt.ForEach(u => u.Id = db.Event.AsNoTracking().FirstOrDefault(e => e.EventId == u.EventId).Id);
                            db.Event.UpdateRange(eventToUpdateOdt);
                            db.SaveChanges();
                        }

                        /*foreach (var elem in Events)
                        {
                            var elemOdt = new EventOdt()
                            {
                                EventId = elem.Id,
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

                            var findEvent = db.Event.FirstOrDefault(x => x.EventId == elemOdt.EventId);
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

                        }*/

                        count += EventsId.Count;
                        skip += 1000;
                    } while (skip == count);
                }
            }
            logger.Info("Синхронизация закончена");
            syncFinish = true;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            logger.Info("Синхронизация остановлена Stop");
            return Task.CompletedTask;
        }
    }
}
