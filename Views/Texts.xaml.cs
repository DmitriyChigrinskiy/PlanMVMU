using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Globalization;
using System.Threading;
using PlanMVMU.DataBase;

namespace PlanMVMU
{
    /// <summary>
    /// Логика взаимодействия для Texts.xaml
    /// </summary>
    public partial class Texts : Window
    {
        PlanEntities Entities = new PlanEntities();
        private int SelectedText = 0;
        private int SelectedKategory = 0;
        private int SelectedStage = 0;
        private int idPrepod;
        List<int> stages = new List<int>(20) { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

        public Texts(int _idPrepod)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-Ru");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-Ru");
            InitializeComponent();
            CBKategory.ItemsSource = Entities.Kategorii.ToList();
            CBStage.ItemsSource = stages.ToList();
            idPrepod = _idPrepod;
        }

        private void BtnSaveText_Click(object sender, RoutedEventArgs e)
        {
            if (TBText.Text != "" && (Kategorii)CBKategory.SelectedItem != null && CBStage.Text != "")
            {
                OriginalText originalText = new OriginalText()
                {
                    TextCompose = TBText.Text,
                    Stage = Convert.ToInt32(CBStage.Text),
                    id_Kategory = ((Kategorii)CBKategory.SelectedItem).ID_Kategoriya,
                    id_Prepodavatel = idPrepod
                };
                Entities.OriginalText.Add(originalText);
                Entities.SaveChanges();

                BtnDelText.Visibility = Visibility.Visible;
                BtnEditText.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Введите данные", "Предупреждение");
            }
        }

        private void CBKategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Convert.ToString(BtnEditText.Content) == "Изменить (категорию/этап/текст)")
            {
                SelectText(CBKategory, CBStage);
            }
        }

        private void CBStage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Convert.ToString(BtnEditText.Content) == "Изменить (категорию/этап/текст)")
            {
                SelectedStage = Convert.ToInt32(CBStage.Items.GetItemAt(CBStage.SelectedIndex).ToString());
                SelectText(CBKategory, CBStage);
            }
        }

        private void SelectText(ComboBox kateg, ComboBox stg)
        {
            if ((Kategorii)CBKategory.SelectedItem != null)
            {
                SelectedKategory = ((Kategorii)CBKategory.SelectedItem).ID_Kategoriya;
            }
            else
            {
                SelectedKategory = 0;
                TBText.Text = "";
                BtnDelText.Visibility = Visibility.Hidden;
                BtnEditText.Visibility = Visibility.Hidden;
                SelectedText = 0;
            }
            if (CBStage.SelectedIndex != -1)
            {

                OriginalText text = Entities.OriginalText.Where(t => t.id_Kategory == SelectedKategory && t.Stage == SelectedStage && t.id_Prepodavatel == idPrepod).FirstOrDefault();
                if (text != default)
                {
                    TBText.Text = text.TextCompose;
                    SelectedText = text.ID_OriginalTextCompose;
                    BtnDelText.Visibility = Visibility.Visible;
                    BtnEditText.Visibility = Visibility.Visible;
                }
                else
                {
                    TBText.Text = "";
                    BtnDelText.Visibility = Visibility.Hidden;
                    BtnEditText.Visibility = Visibility.Hidden;
                    SelectedText = 0;
                }
            }
            else
            {
                TBText.Text = "";
                BtnDelText.Visibility = Visibility.Hidden;
                BtnEditText.Visibility = Visibility.Hidden;
                SelectedText = 0;
            }
        }

        private void BtnEditText_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedText != 0 && Convert.ToString(BtnEditText.Content) == "Изменить (категорию/этап/текст)")
            {
                
                BtnEditText.Content = "Сохранить изменения";
            }
            else if (SelectedText != 0 && Convert.ToString(BtnEditText.Content) == "Сохранить изменения")
            {
                OriginalText originalText = Entities.OriginalText.FirstOrDefault(t => t.ID_OriginalTextCompose == SelectedText);
                originalText.id_Kategory = ((Kategorii)CBKategory.SelectedItem).ID_Kategoriya;
                originalText.Stage = Convert.ToInt32(CBStage.Items.GetItemAt(CBStage.SelectedIndex).ToString());
                originalText.TextCompose = TBText.Text;
                Entities.SaveChanges();
                BtnEditText.Content = "Изменить (категорию/этап/текст)";
            }
        }

        private void BtnDelText_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Действительно удалить данный этап с текстом?", "Подтверждение",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                OriginalText txt = Entities.OriginalText.Where(t => t.id_Kategory == SelectedKategory && t.Stage == SelectedStage).FirstOrDefault();
                if (txt != default)
                {
                    Entities.OriginalText.Remove(txt);
                    Entities.SaveChanges();
                    BtnDelText.Visibility = Visibility.Hidden;
                    BtnEditText.Visibility = Visibility.Hidden;
                    TBText.Text = "";
                    MessageBox.Show("Данный этап удалён");
                }
            }
        }
    }
}
