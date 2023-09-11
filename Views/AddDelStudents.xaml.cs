using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PlanMVMU.DataBase;

namespace PlanMVMU
{
    /// <summary>
    /// Логика взаимодействия для AddDelStudents.xaml
    /// </summary>
    public partial class StudentAndСomposer
    {
        public StudentAndСomposer(string DayWeek ,int IDStudent, string NameStudent, int CourseStudent, string ComposerStudent, string Category)
        {
            this.DayWeek = DayWeek;
            this.IDStudent = IDStudent;
            this.NameStudent = NameStudent;
            this.CourseStudent = CourseStudent;
            this.ComposerStudent = ComposerStudent;
            this.Category = Category;
        }
        public string DayWeek { get; set; }
        public int IDStudent { get; set; }
        public string NameStudent { get; set; }
        public int CourseStudent { get; set; }
        public string ComposerStudent { get; set; }
        public string Category { get; set; }
    } //DataGrid List

    public partial class AddDelStudents : Window
    {
        Teacher SelectedTeacher = new Teacher();
        Entities Entities = new Entities();
        List<StudentAndСomposer> result = new List<StudentAndСomposer>();
        Student SelectedStudent;
        List<Composer> ListKompositorsSelStud = new List<Composer>(4);
        Composer SelectedComposer;
        Category SelectedCategory;
        private bool EditCategory = false;

        public AddDelStudents(Teacher _teacher)
        {
            InitializeComponent();
            SelectedTeacher = _teacher;
            CBKategory.ItemsSource = Entities.Category.ToList();
            UpdateDG();
        }

        private bool Check()
        {
            bool res = true;

            if (TBFIOFile.Text == "")
            {
                res = false;
                TBFIOFile.Background = Brushes.LightCoral;
            }
            if (TBFIOText.Text == "")
            {
                res = false;
                TBFIOText.Background = Brushes.LightCoral;
            }
            if (TBCourse.Text == "")
            {
                res = false;
                TBCourse.Background = Brushes.LightCoral;
            }
            else
            {
                try
                {
                    Convert.ToInt32(TBCourse.Text);
                }
                catch (Exception)
                {
                    res = false;
                    TBCourse.Background = Brushes.LightCoral;
                    MessageBox.Show("Курс студента вводится числами", "Предупреждение");
                }
            }
            if (TBGroup.Text == "")
            {
                res = false;
                TBGroup.Background = Brushes.LightCoral;
            }
            if (ChkBxFri.IsChecked == false && ChkBxMon.IsChecked == false && ChkBxSat.IsChecked == false && ChkBxThu.IsChecked == false && ChkBxTue.IsChecked == false && ChkBxWed.IsChecked == false )
            {
                res = false;
                MessageBox.Show("Выберите не менее 1 дня занятия студента","Предупреждение");
            }
            if (res == false)
            {
                MessageBox.Show("Введите данные для добавления студента", "Предупреждение");
            }

            return res;
        }

        private void UpdateDG()
        {
            result.Clear();
            Entities Entities = new Entities();
            foreach (var item in Entities.Student.Where(t => t.id_Teacher == SelectedTeacher.ID_Teacher))
            {
                List<Composer> Composer = Entities.Composer.Where(t => t.id_Student == item.ID_Student).ToList();
                string _compose = "";
                string _category = "";
                if (Composer.Count != 0)
                {
                    for (int i = 0; i < Composer.Count; i++)
                    {
                        _compose += Composer[i].ComposerAndComposition + "\n";
                        _category += Composer[i].Category.NameCategory + "\n";
                    }
                    _compose = _compose.Remove(_compose.Length - 1);
                    _category = _category.Remove(_category.Length - 1);
                    result.Add(new StudentAndСomposer(GetMonthName.GetNameDays(Entities.Student.Where(t => t.ID_Student == item.ID_Student).FirstOrDefault()), item.ID_Student, item.NameFile, item.StudentCourse, _compose, _category));
                }
                else
                {
                    result.Add(new StudentAndСomposer(GetMonthName.GetNameDays(Entities.Student.Where(t => t.ID_Student == item.ID_Student).FirstOrDefault()), item.ID_Student, item.NameFile, item.StudentCourse, "-", "-"));
                }
            }
            result.Sort((x, y) => String.Compare(x.DayWeek, y.DayWeek));
            DGStudents.ItemsSource = result;
            DGStudents.Items.Refresh();
            string _cb = CBKategory.Text;
            CBKategory.Items.Refresh();
            CBKategory.Text = _cb;
        }

        private void DGStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGStudents.SelectedItem != null)
            {
                StudentAndСomposer SelectedRow = (StudentAndСomposer)DGStudents.SelectedItem;
                SelectedStudent = Entities.Student.Where(t => t.ID_Student == SelectedRow.IDStudent).FirstOrDefault();
                TBKompos.Text = "";
                CBKategory.Text = "";
                if (SelectedRow.ComposerStudent != "-")
                {
                    Composer SelectedKompos = Entities.Composer.Where(t => t.id_Student == SelectedRow.IDStudent).FirstOrDefault();
                    TBKompos.Text = SelectedKompos.ComposerAndComposition;
                    CBKategory.Text = SelectedKompos.Category.NameCategory;
                    SelectedCategory = SelectedKompos.Category;
                    BtnEditKategory.Visibility = Visibility.Visible;
                    if (Entities.Composer.Where(t => t.id_Student == SelectedStudent.ID_Student).Count() > 1)
                    {
                        BtnKomposBack.Visibility = Visibility.Visible;
                        BtnKomposNext.Visibility = Visibility.Visible;
                    }
                    else if (Entities.Composer.Where(t => t.id_Student == SelectedStudent.ID_Student).Count() == 1)
                    {
                        BtnKomposBack.Visibility = Visibility.Hidden;
                        BtnKomposNext.Visibility = Visibility.Hidden;
                    }
                    BtnNewKompos.Visibility = Visibility.Visible;
                    BtnDelKompos.Visibility = Visibility.Visible;
                    SelectedComposer = SelectedKompos;
                    if (ListKompositorsSelStud != null)
                    {
                        ListKompositorsSelStud.Clear();
                    }
                    foreach (var item in Entities.Composer.Where(t => t.id_Student == SelectedStudent.ID_Student))
                    {
                        ListKompositorsSelStud.Add(item);
                    }
                }
                else
                {
                    BtnKomposBack.Visibility = Visibility.Hidden;
                    BtnKomposNext.Visibility = Visibility.Hidden;
                    BtnNewKompos.Visibility = Visibility.Hidden;
                    BtnDelKompos.Visibility = Visibility.Hidden;
                }

                TBFIOFile.Text = SelectedStudent.NameFile;
                TBFIOText.Text = SelectedStudent.NameText;
                TBCourse.Text = Convert.ToString(SelectedStudent.StudentCourse);
                TBGroup.Text = SelectedStudent.StudentGroup;
                ChkBxMon.IsChecked = SelectedStudent.Monday;
                ChkBxTue.IsChecked = SelectedStudent.Tuesday;
                ChkBxWed.IsChecked = SelectedStudent.Wednesday;
                ChkBxThu.IsChecked = SelectedStudent.Thursday;
                ChkBxFri.IsChecked = SelectedStudent.Friday;
                ChkBxSat.IsChecked = SelectedStudent.Saturday;
                TBKompos.Visibility = Visibility.Visible;
                CBKategory.Visibility = Visibility.Visible;
                BtnClearBoxes.Visibility = Visibility.Visible;
                BtnDelStud.Visibility = Visibility.Visible;
                BtnAddEditStud.Content = "Изменить студента";
            }
        }

        private void AddStud()
        {
            if (Check())
            {
                Student student = new Student()
                {
                    NameFile = TBFIOFile.Text,
                    NameText = TBFIOText.Text,
                    StudentGroup = TBGroup.Text,
                    StudentCourse = Convert.ToInt32(TBCourse.Text),
                    id_Teacher = SelectedTeacher.ID_Teacher,
                    Monday = (bool)ChkBxMon.IsChecked,
                    Tuesday = (bool)ChkBxTue.IsChecked,
                    Wednesday = (bool)ChkBxWed.IsChecked,
                    Thursday = (bool)ChkBxThu.IsChecked,
                    Friday = (bool)ChkBxFri.IsChecked,
                    Saturday = (bool)ChkBxSat.IsChecked
                };
                Entities.Student.Add(student);
                Entities.SaveChanges();
                SelectedStudent = student;
                CBKategory.Text = "Произведение";
                TBKompos.Visibility = Visibility.Visible;
                CBKategory.Visibility = Visibility.Visible;
                BtnNewKompos.Visibility = Visibility.Visible;
                BtnDelStud.Visibility = Visibility.Visible;
                BtnClearBoxes.Visibility = Visibility.Visible;
                BtnAddEditStud.Content = "Изменить студента";
            }
        }

        private void EditStud()
        {
            if (Check())
            {
                if (TBFIOFile.Text != SelectedStudent.NameFile || TBFIOText.Text != SelectedStudent.NameText || Convert.ToInt32(TBCourse.Text) != SelectedStudent.StudentCourse ||
                    TBGroup.Text != SelectedStudent.StudentGroup || (bool)ChkBxFri.IsChecked != SelectedStudent.Friday || (bool)ChkBxMon.IsChecked != SelectedStudent.Monday ||
                    (bool)ChkBxSat.IsChecked != SelectedStudent.Saturday || (bool)ChkBxThu.IsChecked != SelectedStudent.Thursday ||
                    (bool)ChkBxTue.IsChecked != SelectedStudent.Tuesday || (bool)ChkBxWed.IsChecked != SelectedStudent.Wednesday)
                {
                    SelectedStudent.NameFile = TBFIOFile.Text;
                    SelectedStudent.NameText = TBFIOText.Text;
                    SelectedStudent.StudentCourse = Convert.ToInt32(TBCourse.Text);
                    SelectedStudent.StudentGroup = TBGroup.Text;
                    SelectedStudent.Monday = (bool)ChkBxMon.IsChecked;
                    SelectedStudent.Tuesday = (bool)ChkBxTue.IsChecked;
                    SelectedStudent.Wednesday = (bool)ChkBxWed.IsChecked;
                    SelectedStudent.Thursday = (bool)ChkBxThu.IsChecked;
                    SelectedStudent.Friday = (bool)ChkBxFri.IsChecked;
                    SelectedStudent.Saturday = (bool)ChkBxSat.IsChecked;
                    Entities.SaveChanges();
                }
                if (TBKompos.Text != "" && CBKategory.Text != "")
                {
                    if (CBKategory.SelectedItem == null && CBKategory.Text != "")
                    {
                        Category category = new Category
                        {
                            NameCategory = CBKategory.Text
                        };
                        Entities.Category.Add(category);
                        Entities.SaveChanges();
                        SelectedCategory = category;
                        CBKategory.ItemsSource = Entities.Category.ToList();
                        CBKategory.Items.Refresh();
                        CBKategory.Text = category.NameCategory;
                    }
                    else if (CBKategory.SelectedItem != null)
                    {
                        SelectedCategory = (Category)CBKategory.SelectedItem;
                    }
                    if (Entities.Composer.Where(t=>t.id_Student == SelectedStudent.ID_Student && t.ComposerAndComposition == TBKompos.Text).FirstOrDefault() == default && CBKategory.Text != "")
                    {
                        try
                        {
                            Composer kompositors = new Composer()
                            {
                                id_Student = SelectedStudent.ID_Student,
                                ComposerAndComposition = TBKompos.Text,
                                id_Category = SelectedCategory.ID_Category
                            };
                            Entities.Composer.Add(kompositors);
                            Entities.SaveChanges();
                            SelectedComposer = kompositors;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else if (Entities.Composer.Where(t => t.id_Student == SelectedStudent.ID_Student && t.ComposerAndComposition == TBKompos.Text).FirstOrDefault() != default)
                    {
                        SelectedComposer.ComposerAndComposition = TBKompos.Text;
                        SelectedComposer.id_Category = SelectedCategory.ID_Category;
                        Entities.SaveChanges();
                    }
                }
                UpdateDG();
                DGStudents.ScrollIntoView(DGStudents.Items[DGStudents.Items.IndexOf(result.Where(t => t.IDStudent == SelectedStudent.ID_Student).FirstOrDefault())]);
                DGStudents.SelectedItem = DGStudents.Items[DGStudents.Items.IndexOf(result.Where(t => t.IDStudent == SelectedStudent.ID_Student).FirstOrDefault())];
                
            }
        }

        private void BtnAddEditStud_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(BtnAddEditStud.Content) == "Добавить студента")
            {
                AddStud();
                UpdateDG();
            }
            else
            {
                EditStud();
            }
        }

        private void ClearAllBoxes()
        {
            TBFIOFile.Text = "";
            TBFIOText.Text = "";
            TBGroup.Text = "";
            TBCourse.Text = "";
            TBKompos.Text = "";
            ChkBxMon.IsChecked = false;
            ChkBxSat.IsChecked = false;
            ChkBxThu.IsChecked = false;
            ChkBxTue.IsChecked = false;
            ChkBxWed.IsChecked = false;
            ChkBxFri.IsChecked = false;
            BtnDelStud.Visibility = Visibility.Hidden;
            BtnClearBoxes.Visibility = Visibility.Hidden;
            TBKompos.Visibility = Visibility.Hidden;
            CBKategory.Visibility = Visibility.Hidden;
            BtnDelKompos.Visibility = Visibility.Hidden;
            BtnNewKompos.Visibility = Visibility.Hidden;
            BtnEditKategory.Visibility = Visibility.Hidden;
            BtnKomposBack.Visibility = Visibility.Hidden;
            BtnKomposNext.Visibility = Visibility.Hidden;
            BtnAddEditStud.Content = "Добавить студента";
        }

        private void BtnClearBoxes_Click(object sender, RoutedEventArgs e)
        {
            ClearAllBoxes();
        }

        private void TBFIOFile_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TBFIOFile.Background = Brushes.Transparent;
        }

        private void TBFIOText_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TBFIOText.Background = Brushes.Transparent;
        }

        private void TBCourse_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TBCourse.Background = Brushes.Transparent;
        }

        private void TBGroup_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TBGroup.Background = Brushes.Transparent;
        }

        private void BtnDellAllKompos_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы хотите удалить список всех композиторов с произведениями. Подтвердите действие.","Требуется подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var item in Entities.Composer)
                {
                    Entities.Composer.Remove(item);
                    Entities.SaveChanges();
                }
                foreach(var item in Entities.Student)
                {
                    item.LastComposer1 = null;
                    item.LastComposer2 = null;
                    item.LastComposer3 = null;
                    Entities.SaveChanges();
                }
                UpdateDG();
                ClearAllBoxes();
                MessageBox.Show("Все композиторы удалены.", "Выполнено");
            }
        }

        private void BtnNewKompos_Click(object sender, RoutedEventArgs e)
        {
            TBKompos.Text = "";
            CBKategory.SelectedIndex = -1;
            CBKategory.Text = "Произведение";
            TBKompos.Focus();
            CBKategory.IsEditable = true;
            BtnDelKompos.Visibility = Visibility.Hidden;
            BtnKomposBack.Visibility = Visibility.Hidden;
            BtnKomposNext.Visibility = Visibility.Hidden;
            BtnClearBoxes.Visibility = Visibility.Hidden;
            BtnNewKompos.Visibility = Visibility.Hidden;
        }

        private void BtnDelKompos_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы хотите удалить данного композитора с произведением. Подтвердите действие.", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    if (SelectedStudent.LastComposer1 == SelectedComposer.ID_Composer)
                    {
                        SelectedStudent.LastComposer1 = null;
                        Entities.SaveChanges();
                    }
                    if (SelectedStudent.LastComposer2 == SelectedComposer.ID_Composer)
                    {
                        SelectedStudent.LastComposer2 = null;
                        Entities.SaveChanges();
                    }
                    if (SelectedStudent.LastComposer3 == SelectedComposer.ID_Composer)
                    {
                        SelectedStudent.LastComposer3 = null;
                        Entities.SaveChanges();
                    }
                    if (SelectedStudent.LastComposer1 == SelectedComposer.ID_Composer)
                    {
                        SelectedStudent.LastComposer1 = null;
                        Entities.SaveChanges();
                    }
                    Entities.Composer.Remove(SelectedComposer);
                    Entities.SaveChanges();
                    UpdateDG();
                    DGStudents.ScrollIntoView(DGStudents.Items[DGStudents.Items.IndexOf(result.Where(t => t.IDStudent == SelectedStudent.ID_Student).FirstOrDefault())]);
                    DGStudents.SelectedItem = DGStudents.Items[DGStudents.Items.IndexOf(result.Where(t => t.IDStudent == SelectedStudent.ID_Student).FirstOrDefault())];
                    MessageBox.Show("Композитор и произведение удалёны.");
                }
                catch (Exception)
                {
                }
            }
        }

        private void CBKategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBKategory.SelectedItem != null)
            {
                BtnEditKategory.Visibility = Visibility.Visible;
            }
            else
            {
                if (EditCategory == false)
                {
                    BtnEditKategory.Visibility = Visibility.Hidden;
                }
            }
        }

        private void BtnEditKategory_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(BtnEditKategory.Content) == "Изм.")
            {
                SelectedCategory = (Category)CBKategory.SelectedItem;
                BtnEditKategory.Content = "Сохр.";
                CBKategory.IsEditable = true;
                EditCategory = true;
            }
            else
            {
                SelectedCategory.NameCategory = CBKategory.Text;
                Entities.SaveChanges();
                CBKategory.Items.Refresh();
                CBKategory.Text = SelectedCategory.NameCategory;
                BtnEditKategory.Content = "Изм.";
                CBKategory.IsEditable = false;
                EditCategory = false;
                UpdateDG();
            }
        }

        private void ListingKompos(int listing)
        {
            if (listing == -1)
            {
                int lst = ListKompositorsSelStud.FindIndex(t => t.ID_Composer == SelectedComposer.ID_Composer);
                if (lst != 0)
                {
                    TBKompos.Text = ListKompositorsSelStud[lst - 1].ComposerAndComposition;
                    CBKategory.Text = ListKompositorsSelStud[lst - 1].Category.NameCategory;
                    SelectedCategory = ListKompositorsSelStud[lst - 1].Category;
                    SelectedComposer = ListKompositorsSelStud[lst - 1];
                }
            }
            else
            {
                int lst = ListKompositorsSelStud.FindIndex(t => t.ID_Composer == SelectedComposer.ID_Composer);
                if (lst < ListKompositorsSelStud.Count - 1)
                {
                    TBKompos.Text = ListKompositorsSelStud[lst + 1].ComposerAndComposition;
                    CBKategory.Text = ListKompositorsSelStud[lst + 1].Category.NameCategory;
                    SelectedCategory = ListKompositorsSelStud[lst + 1].Category;
                    SelectedComposer = ListKompositorsSelStud[lst + 1];
                }
            }
        }

        private void BtnKomposNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListingKompos(1);
        }

        private void BtnKomposBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListingKompos(-1);
        }

        private void BtnDelStud_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Действительно удалить данного студента? Требуется подтверждение.","Подтвердите действие", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                List<Composer> kompositors = Entities.Composer.Where(t => t.id_Student == SelectedStudent.ID_Student).ToList();
                if (kompositors != null)
                {
                    foreach (var item in kompositors)
                    {
                        Entities.Composer.Remove(item);
                        Entities.SaveChanges();
                    }
                }
                Entities.Student.Remove(SelectedStudent);
                Entities.SaveChanges();
                ClearAllBoxes();
                UpdateDG();
            }
        }
    }
}
