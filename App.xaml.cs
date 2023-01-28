using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace PlanMVMU
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            PlanMVMU.Properties.Settings.Default.Save();
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var frFr = new CultureInfo("fr-FR");
            CultureInfo.DefaultThreadCurrentCulture = frFr;
            CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
            CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.FullDateTimePattern = "dd.MM.yyyy";
            CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            
        }
    }
}
