using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using WpfApp.Views;

namespace WpfApp.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindow MainWindow { get; set; }

        public HomeView HomeView { get; set; }
        public HomeViewModel HomeViewModel { get; set; }

        public void InitMainWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeViews();
            MainWindow.Show();
            MainWindow.FrameBody.NavigationService.Navigate(HomeView);
        }

        private void InitializeViews()
        {
            HomeViewModel = new HomeViewModel();
            HomeView = new HomeView {DataContext = HomeViewModel};
        }
    }
}