using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlanMVMU.DataBase;
using PlanMVMU.Views;
using System.Timers;
using Set = PlanMVMU.Properties.Settings;
using System.Threading.Tasks;

namespace PlanMVMU
{
    public partial class MainWindow : Window
    {
        Timer timer;
        Entities Entities = new Entities();

        int timerSetInterval = 10;
        private bool Burger = true;
        private int _idTeacher = 0;

        private void Timer(object sender, ElapsedEventArgs e)
        {
            try
            {
                Set.Default.AllTimeWorkingProgram += timerSetInterval;
            }
            catch (Exception ex)
            {
                Logs.CreateLogs.CreateLog("Главное окно", ex.Message, "Таймер");
            }
        }

        private void startTimer()
        {
            timer = new Timer(timerSetInterval * 1000);
            timer.Elapsed += Timer;
            timer.Start();
        }

        public MainWindow()
        {
            InitializeComponent();
            startTimer();
            Set.Default.AllStartsProgram += 1;
            int teacher = Set.Default.Prepodavatel;
            CBTeacher.ItemsSource = Entities.Teacher.ToList();
            if (Entities.Teacher.FirstOrDefault(t => t.ID_Teacher == teacher) != default)
            {
                CBTeacher.Text = Entities.Teacher.FirstOrDefault(t => t.ID_Teacher == teacher).TeacherName;
                _idTeacher = teacher;
            }
            else
                Set.Default.Prepodavatel = 0;
            if (Set.Default.SaveUrl == "" || !Directory.Exists(Set.Default.SaveUrl))
            {
                IconSelectURL.Foreground = Brushes.Red;
                LblSelectURL.Foreground = Brushes.Red;
            }
            else
                IconSelectURL.ToolTip = "Путь сохранения: " + Set.Default.SaveUrl;
            if (Set.Default.StartDate != Convert.ToDateTime("01.01.0001"))
            {
                Date1.SelectedDate = Set.Default.StartDate;
            }
            if (Set.Default.StopDate != Convert.ToDateTime("01.01.0001"))
            {
                Date2.SelectedDate = Set.Default.StopDate;
            }
            mainframe.frame = frame;
        }

        private void SaveSelectedURL(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "";
            dlg.ValidateNames = false;
            dlg.CheckFileExists = false;
            dlg.CheckPathExists = true;
            dlg.FileName = "Folder Selection.";

            if (dlg.ShowDialog() == true)
            {
                Set.Default.SaveUrl = Path.GetDirectoryName(dlg.FileName);
                IconSelectURL.ToolTip = Set.Default.SaveUrl;
                LblSelectURL.Foreground = Brushes.Black;
                IconSelectURL.Foreground = Brushes.Black;
            }
        }

        private bool Check()
        {
            bool check = true;
            if (Set.Default.SaveUrl == "")
            {
                LblSelectURL.Foreground = Brushes.Red;
                IconSelectURL.Foreground = Brushes.Red;
                check = false;
            }
            if (Date1.SelectedDate == null)
            {
                Date1.Foreground = Brushes.Red;
                IconDateStart.Foreground = Brushes.Red;
                check = false;
            }
            if (Date2.SelectedDate == null)
            {
                Date2.Foreground = Brushes.Red;
                IconDateFinal.Foreground = Brushes.Red;
                check = false;
            }
            if (CBTeacher.SelectedItem == null)
            {
                CBTeacher.Foreground = Brushes.Red;
                IconTeacher.Foreground = Brushes.Red;
                check = false;
            }
            return check;
        }

        private void CreatePlans_Click(object sender, RoutedEventArgs e)
        {
            if (Check())
            {
                if (Entities.Student.Where(t=>t.id_Teacher == _idTeacher).Count() <= 0)
                {
                    MessageBox.Show("Нет добавленных студентов для данного преподавателя", "Предупреждение");
                    return;
                }
                DateTime dt1 = (Date1.SelectedDate).Value;
                DateTime dt2 = (Date2.SelectedDate).Value;
                string pathString = Path.Combine(Set.Default.SaveUrl, ((Teacher)CBTeacher.SelectedItem).TeacherName);
                Directory.CreateDirectory(pathString);
                Set.Default.Stop = false;
                frame.Content = new CreateOptionalPlan(true, dt1, dt2, (Teacher)CBTeacher.SelectedItem, pathString);
            }
            else
            {
                MessageBox.Show("Для создания планов добавьте/выберите необходимые данные","Предупреждение");
            }
        }

        private void Date1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Date1.SelectedDate > Date2.SelectedDate)
            {
                Date2.SelectedDate = Date1.SelectedDate;
            }
            Date1.Foreground = Brushes.Black;
            IconDateStart.Foreground = Brushes.Black;
            IconDateStart.ToolTip = (object)"Дата начала планов - " + Date1.SelectedDate.Value.ToString("d");
        }

        private void Date2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Date2.SelectedDate < Date1.SelectedDate)
            {
                Date1.SelectedDate = Date2.SelectedDate;
            }
            Date2.Foreground = Brushes.Black;
            IconDateFinal.Foreground = Brushes.Black;
            IconDateFinal.ToolTip = (object)"Дата окончания планов - " + Date2.SelectedDate.Value.ToString("d");
        }

        private void CBTeacher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Teacher)CBTeacher.SelectedItem != null)
            {
                CBTeacher.Foreground = Brushes.Black;
                IconTeacher.Foreground = Brushes.Black;
                _idTeacher = ((Teacher)CBTeacher.SelectedItem).ID_Teacher;
                Set.Default.Prepodavatel = _idTeacher;
                IconTeacher.ToolTip = "Выбран преподаватель: " + ((Teacher)CBTeacher.SelectedItem).TeacherName;
            }
            else
            {
                _idTeacher = 0;
                IconTeacher.ToolTip = "Выберите преподавателя";
            }
        }

        private void BtnAddTeacher(object sender, RoutedEventArgs e)
        {
            AddDelTeacher addDelPrepod = new AddDelTeacher(Entities);
            addDelPrepod.ShowDialog();
            CBTeacher.Items.Refresh();
            CBTeacher.SelectedIndex = -1;
        }

        private void BtnAddStudents(object sender, RoutedEventArgs e)
        {
            if (_idTeacher != 0)
            {
                AddDelStudents addDelStudents = new AddDelStudents((Teacher)CBTeacher.SelectedItem);
                addDelStudents.ShowDialog();
            }
            else
            {
                IconTeacher.Foreground = Brushes.Red;
                CBTeacher.Foreground = Brushes.Red;
                MessageBox.Show("Для добавления студентов выберите преподавателя, из выпадающего списка ниже.", "Предупреждение");
            }
        }

        private void BtnAddTexts(object sender, RoutedEventArgs e)
        {
            if (_idTeacher != 0)
            {
                Texts texts = new Texts(_idTeacher, Entities);
                texts.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите преподавателя для добавления текста", "Предупреждение");
                CBTeacher.Foreground = Brushes.Red;
                IconTeacher.Foreground = Brushes.Red;
            }
        }

        private void AddInfoSh_Click(object sender, RoutedEventArgs e)
        {
            AddInfoShablon addInfoShablon = new AddInfoShablon();
            addInfoShablon.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            timer.Stop();
            Set.Default.Save();
        }

        private void PackIcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!Burger)
            {
                MenuBurger.Width = new GridLength(220);
                Burger = true;
            }
            else
            {
                MenuBurger.Width = new GridLength(40);
                Burger = false;
            }
        }

        private void Label_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Программное обеспечение МВМУ Планы v.1.0 2022-2023.\n" +
                "Разработчик - Чигринский Д.В.\n" +
                "Контакты для связи:\n" +
                "e-mail: dimkoo.arm@gmail.com\n" +
                "моб. телефон: +7(977)548-28-70","МВМУ Планы");
        }

        private void ShowWindowStatistics_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.ShowDialog();
        }
    }
}
