using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PlanMVMU.DataBase;
using PlanMVMU.DataBase.Operations;

namespace PlanMVMU
{
    /// <summary>
    /// Логика взаимодействия для AddDelPrepod.xaml
    /// </summary>
    public partial class AddDelTeacher : Window
    {
        private Entities Entities;
        private Teacher Teacher;

        public AddDelTeacher(Entities entities)
        {
            Entities = entities;
            InitializeComponent();
            UpdateDg();
        }

        private void UpdateDg()
        {
            DgTeachers.ItemsSource = Entities.Teacher.ToList();
            DgTeachers.Items.Refresh();
        }

        private void BtnDeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Teacher != null)
                {
                    if (MessageBox.Show("Вы действительно хотите удалить данного преподавателя?", "Подтверждение операции", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Entities.Teacher.Remove(Teacher);
                        Entities.SaveChanges();
                        TbTeacherName.Text = "";
                        BtnDeleteTeacher.Visibility = Visibility.Hidden;
                        UpdateDg();
                    }
                }
                else
                {
                    MessageBox.Show("Преподаватель не выбран или запись больше не существует", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка. Запись добавлена в лог событий.","Ошибка");
                Logs.CreateLogs.CreateLog("Редактирование преподавателей", ex.Message, "Удаление преподавателя");
            }
        }

        private void BtnAddTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (TeacherOperations.AddTeacher(TbTeacherName.Text, Entities))
            {
                UpdateDg();
                TbTeacherName.Text = "";
            }
        }

        private void DgTeachersSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Teacher = (Teacher)DgTeachers.SelectedItem;
            if (Teacher != null)
            {
                TbTeacherName.Text = Teacher.TeacherName;
                BtnDeleteTeacher.Visibility = Visibility.Visible;
            }
        }

        private void BtnEditTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (DataBase.Operations.TeacherOperations.EditTeacher(TbTeacherName.Text, (Teacher)DgTeachers.SelectedItem, Entities))
            {
                UpdateDg();
                TbTeacherName.Text = "";
            }
        }

        private void TbTeacherName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }
    }
}
