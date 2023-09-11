using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Set = PlanMVMU.Properties.Settings;


namespace PlanMVMU.Views
{
    /// <summary>
    /// Логика взаимодействия для Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        private int allSecondsWorkingProgram;
        private int allPlansCreated;
        private int timeCreateOnePlanManually = 200; //Примерное время изменения шаблона плана вручную
        private int allStartsProgram;

        public Statistics()
        {
            InitializeComponent();
            UpdateStats();
        }

        private void UpdateStats()
        {
            allSecondsWorkingProgram = Set.Default.AllTimeWorkingProgram;
            allPlansCreated = Set.Default.AllPlansCreated;
            allStartsProgram = Set.Default.AllStartsProgram;
            LblTimerProgram.Content = TimeToDHMS(allSecondsWorkingProgram);
            LblSavedTime.Content = TimeSaving();
            LblAllPlansCreated.Content = allPlansCreated.ToString();
            LblAllStartsProgramm.Content = allStartsProgram.ToString();
        }

        private void ClearStatistics_Click(object sender, RoutedEventArgs e)
        {
            Set.Default.AllPlansCreated = 0;
            Set.Default.AllStartsProgram = 0;
            Set.Default.AllTimeWorkingProgram = 0;
            Set.Default.Save();
            UpdateStats();
        }

        private string TimeSaving()
        {
            int timePlansManually = timeCreateOnePlanManually * allPlansCreated;

            return TimeToDHMS(timePlansManually - allSecondsWorkingProgram);
        }
        private string TimeToDHMS(int n)
        {
            int day;
            int hourse;
            int minutes;
            int seconds;

            if (n > 0)
            {
                day = n / (24 * 3600);
                n = n % (24 * 3600);
                hourse = n / 3600;
                n = n % 3600;
                minutes = n / 60;
                n = n % 60;
                seconds = n;
            }
            else
            {
                day = 0;
                hourse = 0;
                minutes = 0;
                seconds = 0;
            }
            return $"{day}д./{hourse}ч./{minutes}мин./{seconds}сек.";
        }
    }
}
