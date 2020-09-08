using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Data;
using WpfApp.Helpers;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
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
                OnPropertyChanged(nameof(Event));
            }
        }

        public ICommand ShowUnitsCommand => new RelayCommand(async ol => await ShowUnitsCommandExecuted());

        public ICommand BreakUiCommand => new RelayCommand(BreakUiExecuted);
        public ICommand AddEventsCommand => new RelayCommand(AddEvents);

        private void BreakUiExecuted(object obj)
        {
            Task.Delay(TimeSpan.FromSeconds(3)).Wait();
        }

        private async Task ShowUnitsCommandExecuted()
        {
            var unitsDb = await repository.GetUnits();
            Units = new ObservableCollection<Unit>(unitsDb);
        }

        private void AddEvents(object obj)
        {
            MessageBox.Show(obj.ToString());
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