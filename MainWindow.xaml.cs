using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlanMVMU
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PlanEntities Entities = new PlanEntities();
        int IdPrep = 0;

        public MainWindow()
        {
            InitializeComponent();
            int prepod = Properties.Settings.Default.Prepodavatel;
            CBPrepodavatel.ItemsSource = Entities.Prepodavateli.ToList();
            if (Entities.Prepodavateli.FirstOrDefault(t=>t.ID_Prepodavatel == prepod) != default)
            {
                CBPrepodavatel.Text = Entities.Prepodavateli.FirstOrDefault(t=>t.ID_Prepodavatel == prepod).Name;
            }
            if (Properties.Settings.Default.SaveUrl != "" && Directory.Exists(Properties.Settings.Default.SaveUrl))
            {
                BtnUrlSave.Background = Brushes.Transparent;
            }
            if (Properties.Settings.Default.StartDate != Convert.ToDateTime("01.01.0001"))
            {
                Date1.SelectedDate = Properties.Settings.Default.StartDate;
            }
            if (Properties.Settings.Default.StopDate != Convert.ToDateTime("01.01.0001"))
            {
                Date2.SelectedDate = Properties.Settings.Default.StopDate;
            }
            mainframe.frame = frame;
        }

        private void Select_URLSave(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "";
            dlg.ValidateNames = false;
            dlg.CheckFileExists = false;
            dlg.CheckPathExists = true;
            dlg.FileName = "Folder Selection.";

            if (dlg.ShowDialog() == true)
            {
                Properties.Settings.Default.SaveUrl = System.IO.Path.GetDirectoryName(dlg.FileName);
                BtnUrlSave.Background = Brushes.Transparent;
            }
        }

        private bool Check()
        {
            int chk = 0;
            if (Properties.Settings.Default.SaveUrl == "")
            {
                BtnUrlSave.Background = Brushes.LightCoral;
                chk += 1;
            }
            if (Date1.SelectedDate == null)
            {
                Date1.Background = Brushes.LightCoral;
                chk += 1;
            }
            if (Date2.SelectedDate == null)
            {
                Date2.Background = Brushes.LightCoral;
                chk += 1;
            }
            if (CBPrepodavatel.SelectedItem == null)
            {
                CBPrepodavatel.Background = Brushes.LightCoral;
                chk += 1;
            }
            return chk == 0 ? true: false;
        }

        private void CreatePlans_Click(object sender, RoutedEventArgs e)
        {
            if (Check())
            {
                if (Entities.Students.Where(t=>t.id_Prepod == IdPrep).Count() <= 0)
                {
                    MessageBox.Show("Нет добавленных студентов для данного преподавателя", "Предупреждение");
                    return;
                }
                DateTime dt1 = Convert.ToDateTime(Date1.SelectedDate);
                DateTime dt2 = Convert.ToDateTime(Date2.SelectedDate);

                string pathString = System.IO.Path.Combine(Properties.Settings.Default.SaveUrl, ((Prepodavateli)CBPrepodavatel.SelectedItem).Name);
                Directory.CreateDirectory(pathString);
                Properties.Settings.Default.Stop = false;

                frame.Content = new CreateOptionalPlan(true, dt1, dt2, (Prepodavateli)CBPrepodavatel.SelectedItem, pathString);
            }
            else
            {
                MessageBox.Show("Выберите путь сохранения файлов и даты","Предупреждение");
            }
        }

        private void Date1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Date1.Background = Brushes.Transparent;
        }

        private void Date2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Date2 .Background = Brushes.Transparent;
        }

        private void CBPrepodavatel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBPrepodavatel.Background = Brushes.Transparent;
            IdPrep = ((Prepodavateli)CBPrepodavatel.SelectedItem).ID_Prepodavatel;
            Properties.Settings.Default.Prepodavatel = IdPrep;
        }

        private void BtnAddPrepod(object sender, RoutedEventArgs e)
        {
            AddDelPrepod addDelPrepod = new AddDelPrepod();
            addDelPrepod.ShowDialog();
            CBPrepodavatel.ItemsSource = Entities.Prepodavateli.ToList();
            CBPrepodavatel.Items.Refresh();
        }

        private void BtnAddStudents(object sender, RoutedEventArgs e)
        {
            if (CBPrepodavatel.SelectedIndex != -1)
            {
                AddDelStudents addDelStudents = new AddDelStudents((Prepodavateli)CBPrepodavatel.SelectedItem);
                addDelStudents.ShowDialog();
            }
            else
            {
                MessageBox.Show("Для добавления студентов выберите преподавателя, из выпадающего списка, в верхнем правом углу", "Предупреждение");
            }
        }

        private void BtnAddTexts(object sender, RoutedEventArgs e)
        {
            Texts texts = new Texts(IdPrep);
            texts.ShowDialog();
        }

        private void AddInfoSh_Click(object sender, RoutedEventArgs e)
        {
            AddInfoShablon addInfoShablon = new AddInfoShablon();
            addInfoShablon.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
