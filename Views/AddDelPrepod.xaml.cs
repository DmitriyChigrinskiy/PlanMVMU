using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PlanMVMU.DataBase;

namespace PlanMVMU
{
    /// <summary>
    /// Логика взаимодействия для AddDelPrepod.xaml
    /// </summary>
    public partial class AddDelPrepod : Window
    {
        PlanEntities Entities = new PlanEntities();
        Prepodavateli prepodavateli;

        public AddDelPrepod()
        {
            InitializeComponent();
            UpdateDG();
        }

        private void UpdateDG()
        {
            DGPrepods.ItemsSource = Entities.Prepodavateli.ToList();
        }

        private void DeletePrepod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (prepodavateli != null)
                {
                    Entities.Prepodavateli.Remove(prepodavateli);
                    Entities.SaveChanges();
                    TBSelectedPrepod.Text = "";
                    DeletePrepod.Visibility = Visibility.Hidden;
                    UpdateDG();
                }
                else
                {
                    MessageBox.Show("Выбранный преподаватель не найден", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddPrepod_Click(object sender, RoutedEventArgs e)
        {
            if (TBPrepod.Text != "")
            {
                Prepodavateli prepodavatel = new Prepodavateli();
                prepodavatel.Name = TBPrepod.Text;
                Entities.Prepodavateli.Add(prepodavatel);
                Entities.SaveChanges();
                MessageBox.Show("Преподаватель " + TBPrepod.Text + " добавлен.", "Выполнено");
                UpdateDG();
                TBPrepod.Text = "";
            }
            else
            {
                MessageBox.Show("Введите данные!", "Предупреждение");
            }
        }

        private void DGPrepodsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            prepodavateli = (Prepodavateli)DGPrepods.SelectedItem;
            if (prepodavateli != null)
            {
                TBSelectedPrepod.Text = prepodavateli.Name;
                DeletePrepod.Visibility = Visibility.Visible;
            }
        }
    }
}
