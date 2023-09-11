using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PlanMVMU.DataBase;
using PlanMVMU.DataBase.Operations;

namespace PlanMVMU
{
    /// <summary>
    /// Логика взаимодействия для Texts.xaml
    /// </summary>
    public partial class Texts : Window
    {
        #region Variables
        private Entities Entities;
        private int _selectedText = 0;
        private int _selectedCategory = 0;
        private int _selectedStage = 0;
        private int idTeacher;
        private int stagesLength = 20;
        private List<int> stages = new List<int>();
        #endregion

        public Texts(int _idTeacher, Entities entities)
        {
            InitializeComponent();
            Entities = entities;
            CBCategory.ItemsSource = Entities.Category.ToList();
            idTeacher = _idTeacher;
            for (int i = 1; i <= stagesLength; i++)
                stages.Add(i);
            CBStage.ItemsSource = stages.ToList();
        }

        private bool checkData()
        {
            bool _cbStageSelected = _selectedStage != 0;
            bool _cbCategorySelected = _selectedCategory != 0;
            bool _tbTextWrite = TBText.Text != "";
            return _cbStageSelected && _cbCategorySelected && _tbTextWrite;
        }

        private void CBCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnEditText.Visibility = Visibility.Hidden;
            if ((Category)CBCategory.SelectedItem != null)
            {
                _selectedCategory = ((Category)CBCategory.SelectedItem).ID_Category;
                if (CBStage.SelectedIndex > -1)
                    SelectText();
            }
        }

        private void CBStage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnEditText.Visibility = Visibility.Hidden;
            if (CBStage.SelectedIndex > -1)
            {
                _selectedStage = int.Parse(CBStage.SelectedItem.ToString());
                if ((Category)CBCategory.SelectedItem != null)
                    SelectText();
            }
        }

        private void SelectText()
        {
            OriginalText text = Entities.OriginalText.FirstOrDefault(t => t.id_Category == _selectedCategory && t.Stage == _selectedStage && t.id_Teacher == idTeacher);
            if (text != default)
            {
                BtnSaveText.Visibility = Visibility.Hidden;
                BtnEditText.Visibility = Visibility.Hidden;
                BtnDelText.Visibility = Visibility.Visible;
                TBText.Text = text.Text;
                _selectedText = text.ID_OriginalText;
            }
            else
            {
                TBText.Text = "";
                _selectedText = 0;
                BtnDelText.Visibility = Visibility.Hidden;
                BtnEditText.Visibility = Visibility.Hidden;
            }
        }

        private void BtnSaveText_Click(object sender, RoutedEventArgs e)
        {
            if (checkData())
            {
                if (TextOperations.AddText(TBText.Text, _selectedStage, _selectedCategory, idTeacher, Entities))
                {
                    SelectText();
                }
            }
            else
                MessageBox.Show("Данные не введены.", "Предупреждение");
        }

        private void BtnEditText_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedText != 0 && checkData())
            {
                if (TextOperations.EditText(_selectedText, TBText.Text, _selectedStage, _selectedCategory, idTeacher, Entities))
                {
                    SelectText();
                }
            }
            else
                MessageBox.Show("Соблюдены не все условия.", "Предупреждение", MessageBoxButton.OK);
        }

        private void BtnDelText_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedText != 0 && checkData())
            {
                if (TextOperations.DeleteText(_selectedText, Entities))
                {
                    SelectText();
                }
            }
        }

        private void TBText_KeyUp(object sender, KeyEventArgs e)
        {
            if (BtnEditText.Visibility == Visibility.Hidden && _selectedText != 0)
            {
                BtnEditText.Visibility = Visibility.Visible;
            }
            else if (_selectedText == 0 && _selectedCategory != 0 && _selectedStage != 0)
            {
                BtnEditText.Visibility = Visibility.Hidden;
                BtnSaveText.Visibility = Visibility.Visible;
            }
            if (TBText.Text == "")
            {
                BtnSaveText.Visibility = Visibility.Hidden;
                BtnEditText.Visibility = Visibility.Hidden;
            }
        }
    }
}
