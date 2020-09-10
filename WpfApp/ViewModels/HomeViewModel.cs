using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Data;
using WpfApp.Helpers;
using WpfApp.Models;
using System.Text.Json;
using System.Linq;
using Newtonsoft.Json;

namespace WpfApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        static readonly HttpClient client = new HttpClient();
        public Unit SelectedUnit { get; set; }
        private readonly Repository repository;

        public HomeViewModel()
        {
            repository = new Repository();

        }

        private ObservableCollection<Unit> units = new ObservableCollection<Unit>();

        public ObservableCollection<Unit> Units
        {
            get => units;
            set
            {
                units = value;
                OnPropertyChanged(nameof(Units));
            }
        }

        private ObservableCollection<Event> events = new ObservableCollection<Event>();
        public ObservableCollection<Event> Events
        {
            get => events;
            set
            {
                events = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        public ICommand ShowUnitsCommand => new RelayCommand(async ol => await ShowUnitsCommandExecuted());

        public ICommand BreakUiCommand => new RelayCommand(BreakUiExecuted);
        public ICommand AddEventsCommand => new RelayCommand(async ol => await AddEvents());

        private void BreakUiExecuted(object obj)
        {
            Task.Delay(TimeSpan.FromSeconds(3)).Wait();
        }

        private async Task ShowUnitsCommandExecuted()
        {
            var unitsDb = await repository.GetUnits();
            Units = new ObservableCollection<Unit>(unitsDb);
        }

        private async Task AddEvents()
        {
            if(SelectedUnit!=null)
            {
               try
               {
                    var url = $"http://localhost:5001/api/unit/events/{SelectedUnit.Id}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var eventsDb = JsonConvert.DeserializeObject<List<Event>>(responseBody); //JsonSerializer.Deserialize<IReadOnlyCollection<Event>>(responseBody);
                    Events = new ObservableCollection<Event>(eventsDb);
                    
               }
               catch(Exception e)
               {
                   MessageBox.Show(e.ToString());
               }
               finally
               {
                   MessageBox.Show(Events.Count.ToString());
               }
                
            }
        }
        

        #region OnPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}