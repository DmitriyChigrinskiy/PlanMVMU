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

namespace PlanMVMU
{
    /// <summary>
    /// Логика взаимодействия для AddDelStudents.xaml
    /// </summary>
    public partial class StudAndKompos
    {
        public StudAndKompos(string DayWeek ,int IDStud, string NameStud, int CourseStud, string KomposStud, string Kategory)
        {
            this.DayWeek = DayWeek;
            this.IDStud = IDStud;
            this.NameStud = NameStud;
            this.CourseStud = CourseStud;
            this.KomposStud = KomposStud;
            this.Kategory = Kategory;
        }
        public string DayWeek { get; set; }
        public int IDStud { get; set; }
        public string NameStud { get; set; }
        public int CourseStud { get; set; }
        public string KomposStud { get; set; }
        public string Kategory { get; set; }
    } //DataGrid List

    public partial class AddDelStudents : Window
    {
        Prepodavateli SelectedPrepod = new Prepodavateli();
        PlanEntities Entities = new PlanEntities();
        List<StudAndKompos> result = new List<StudAndKompos>(6);
        Students SelectedStud;
        List<Kompositors> ListKompositorsSelStud = new List<Kompositors>(4);
        Kompositors SelectedKompositor;
        Kategorii SelectedKategory;
        private bool EditKategory = false;

        public AddDelStudents(Prepodavateli Prepodavatel)
        {
            InitializeComponent();
            SelectedPrepod = Prepodavatel;
            CBKategory.ItemsSource = Entities.Kategorii.ToList();
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
            PlanEntities Entities = new PlanEntities();
            foreach (var item in Entities.Students.Where(t => t.id_Prepod == SelectedPrepod.ID_Prepodavatel))
            {
                List<Kompositors> kompos = Entities.Kompositors.Where(t => t.id_Student == item.ID_Student).ToList();
                string _kompos = "";
                string _kategory = "";
                if (kompos.Count != 0)
                {
                    for (int i = 0; i < kompos.Count; i++)
                    {
                        _kompos += kompos[i].KomposAndName + "\n";
                        _kategory += kompos[i].Kategorii.NameKategory + "\n";
                    }
                    _kompos = _kompos.Remove(_kompos.Length - 1);
                    _kategory = _kategory.Remove(_kategory.Length - 1);
                    result.Add(new StudAndKompos(GetMonthName.GetNameDays(Entities.Students.Where(t => t.ID_Student == item.ID_Student).FirstOrDefault()), item.ID_Student, item.NameFile, item.StudCourse, _kompos, _kategory));
                }
                else
                {
                    result.Add(new StudAndKompos(GetMonthName.GetNameDays(Entities.Students.Where(t => t.ID_Student == item.ID_Student).FirstOrDefault()), item.ID_Student, item.NameFile, item.StudCourse, "-", "-"));
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
                StudAndKompos SelectedRow = (StudAndKompos)DGStudents.SelectedItem;
                SelectedStud = Entities.Students.Where(t => t.ID_Student == SelectedRow.IDStud).FirstOrDefault();
                TBKompos.Text = "";
                CBKategory.Text = "";
                if (SelectedRow.KomposStud != "-")
                {
                    Kompositors SelectedKompos = Entities.Kompositors.Where(t => t.id_Student == SelectedRow.IDStud).FirstOrDefault();
                    TBKompos.Text = SelectedKompos.KomposAndName;
                    CBKategory.Text = SelectedKompos.Kategorii.NameKategory;
                    SelectedKategory = SelectedKompos.Kategorii;
                    BtnEditKategory.Visibility = Visibility.Visible;
                    if (Entities.Kompositors.Where(t => t.id_Student == SelectedStud.ID_Student).Count() > 1)
                    {
                        BtnKomposBack.Visibility = Visibility.Visible;
                        BtnKomposNext.Visibility = Visibility.Visible;
                    }
                    else if (Entities.Kompositors.Where(t => t.id_Student == SelectedStud.ID_Student).Count() == 1)
                    {
                        BtnKomposBack.Visibility = Visibility.Hidden;
                        BtnKomposNext.Visibility = Visibility.Hidden;
                    }
                    BtnNewKompos.Visibility = Visibility.Visible;
                    BtnDelKompos.Visibility = Visibility.Visible;
                    SelectedKompositor = SelectedKompos;
                    if (ListKompositorsSelStud != null)
                    {
                        ListKompositorsSelStud.Clear();
                    }
                    foreach (var item in Entities.Kompositors.Where(t => t.id_Student == SelectedStud.ID_Student))
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

                TBFIOFile.Text = SelectedStud.NameFile;
                TBFIOText.Text = SelectedStud.NameText;
                TBCourse.Text = Convert.ToString(SelectedStud.StudCourse);
                TBGroup.Text = SelectedStud.StudGroup;
                ChkBxMon.IsChecked = SelectedStud.Monday;
                ChkBxTue.IsChecked = SelectedStud.Tuesday;
                ChkBxWed.IsChecked = SelectedStud.Wednesday;
                ChkBxThu.IsChecked = SelectedStud.Thursday;
                ChkBxFri.IsChecked = SelectedStud.Friday;
                ChkBxSat.IsChecked = SelectedStud.Saturday;
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
                Students student = new Students()
                {
                    NameFile = TBFIOFile.Text,
                    NameText = TBFIOText.Text,
                    StudGroup = TBGroup.Text,
                    StudCourse = Convert.ToInt32(TBCourse.Text),
                    id_Prepod = SelectedPrepod.ID_Prepodavatel,
                    Monday = (bool)ChkBxMon.IsChecked,
                    Tuesday = (bool)ChkBxTue.IsChecked,
                    Wednesday = (bool)ChkBxWed.IsChecked,
                    Thursday = (bool)ChkBxThu.IsChecked,
                    Friday = (bool)ChkBxFri.IsChecked,
                    Saturday = (bool)ChkBxSat.IsChecked
                };
                Entities.Students.Add(student);
                Entities.SaveChanges();
                SelectedStud = student;
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
                if (TBFIOFile.Text != SelectedStud.NameFile || TBFIOText.Text != SelectedStud.NameText || Convert.ToInt32(TBCourse.Text) != SelectedStud.StudCourse ||
                    TBGroup.Text != SelectedStud.StudGroup || (bool)ChkBxFri.IsChecked != SelectedStud.Friday || (bool)ChkBxMon.IsChecked != SelectedStud.Monday ||
                    (bool)ChkBxSat.IsChecked != SelectedStud.Saturday || (bool)ChkBxThu.IsChecked != SelectedStud.Thursday ||
                    (bool)ChkBxTue.IsChecked != SelectedStud.Tuesday || (bool)ChkBxWed.IsChecked != SelectedStud.Wednesday)
                {
                    SelectedStud.NameFile = TBFIOFile.Text;
                    SelectedStud.NameText = TBFIOText.Text;
                    SelectedStud.StudCourse = Convert.ToInt32(TBCourse.Text);
                    SelectedStud.StudGroup = TBGroup.Text;
                    SelectedStud.Monday = (bool)ChkBxMon.IsChecked;
                    SelectedStud.Tuesday = (bool)ChkBxTue.IsChecked;
                    SelectedStud.Wednesday = (bool)ChkBxWed.IsChecked;
                    SelectedStud.Thursday = (bool)ChkBxThu.IsChecked;
                    SelectedStud.Friday = (bool)ChkBxFri.IsChecked;
                    SelectedStud.Saturday = (bool)ChkBxSat.IsChecked;
                    Entities.SaveChanges();
                }
                if (TBKompos.Text != "" && CBKategory.Text != "")
                {
                    if (CBKategory.SelectedItem == null && CBKategory.Text != "")
                    {
                        Kategorii kategorii = new Kategorii
                        {
                            NameKategory = CBKategory.Text
                        };
                        Entities.Kategorii.Add(kategorii);
                        Entities.SaveChanges();
                        SelectedKategory = kategorii;
                        CBKategory.ItemsSource = Entities.Kategorii.ToList();
                        CBKategory.Items.Refresh();
                        CBKategory.Text = kategorii.NameKategory;
                    }
                    else if (CBKategory.SelectedItem != null)
                    {
                        SelectedKategory = (Kategorii)CBKategory.SelectedItem;
                    }
                    if (Entities.Kompositors.Where(t=>t.id_Student == SelectedStud.ID_Student && t.KomposAndName == TBKompos.Text).FirstOrDefault() == default && CBKategory.Text != "")
                    {
                        try
                        {
                            Kompositors kompositors = new Kompositors()
                            {
                                id_Student = SelectedStud.ID_Student,
                                KomposAndName = TBKompos.Text,
                                id_Kat = SelectedKategory.ID_Kategoriya
                            };
                            Entities.Kompositors.Add(kompositors);
                            Entities.SaveChanges();
                            SelectedKompositor = kompositors;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else if (Entities.Kompositors.Where(t => t.id_Student == SelectedStud.ID_Student && t.KomposAndName == TBKompos.Text).FirstOrDefault() != default)
                    {
                        SelectedKompositor.KomposAndName = TBKompos.Text;
                        SelectedKompositor.id_Kat = SelectedKategory.ID_Kategoriya;
                        Entities.SaveChanges();
                    }
                }
                UpdateDG();
                DGStudents.ScrollIntoView(DGStudents.Items[DGStudents.Items.IndexOf(result.Where(t => t.IDStud == SelectedStud.ID_Student).FirstOrDefault())]);
                DGStudents.SelectedItem = DGStudents.Items[DGStudents.Items.IndexOf(result.Where(t => t.IDStud == SelectedStud.ID_Student).FirstOrDefault())];
                
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
                foreach (var item in Entities.Kompositors)
                {
                    Entities.Kompositors.Remove(item);
                    Entities.SaveChanges();
                }
                foreach(var item in Entities.Students)
                {
                    item.LastKompos1 = null;
                    item.LastKompos2 = null;
                    item.LastKompos3 = null;
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
                    if (SelectedStud.LastKompos1 == SelectedKompositor.ID_Kompos)
                    {
                        SelectedStud.LastKompos1 = null;
                        Entities.SaveChanges();
                    }
                    if (SelectedStud.LastKompos2 == SelectedKompositor.ID_Kompos)
                    {
                        SelectedStud.LastKompos2 = null;
                        Entities.SaveChanges();
                    }
                    if (SelectedStud.LastKompos3 == SelectedKompositor.ID_Kompos)
                    {
                        SelectedStud.LastKompos3 = null;
                        Entities.SaveChanges();
                    }
                    if (SelectedStud.LastKompos1 == SelectedKompositor.ID_Kompos)
                    {
                        SelectedStud.LastKompos1 = null;
                        Entities.SaveChanges();
                    }
                    Entities.Kompositors.Remove(SelectedKompositor);
                    Entities.SaveChanges();
                    UpdateDG();
                    DGStudents.ScrollIntoView(DGStudents.Items[DGStudents.Items.IndexOf(result.Where(t => t.IDStud == SelectedStud.ID_Student).FirstOrDefault())]);
                    DGStudents.SelectedItem = DGStudents.Items[DGStudents.Items.IndexOf(result.Where(t => t.IDStud == SelectedStud.ID_Student).FirstOrDefault())];
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
                if (EditKategory == false)
                {
                    BtnEditKategory.Visibility = Visibility.Hidden;
                }
            }
        }

        private void BtnEditKategory_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(BtnEditKategory.Content) == "Изм.")
            {
                SelectedKategory = (Kategorii)CBKategory.SelectedItem;
                BtnEditKategory.Content = "Сохр.";
                CBKategory.IsEditable = true;
                EditKategory = true;
            }
            else
            {
                SelectedKategory.NameKategory = CBKategory.Text;
                Entities.SaveChanges();
                CBKategory.Items.Refresh();
                CBKategory.Text = SelectedKategory.NameKategory;
                BtnEditKategory.Content = "Изм.";
                CBKategory.IsEditable = false;
                EditKategory = false;
                UpdateDG();
            }
        }

        private void ListingKompos(int listing)
        {
            if (listing == -1)
            {
                int lst = ListKompositorsSelStud.FindIndex(t => t.ID_Kompos == SelectedKompositor.ID_Kompos);
                if (lst != 0)
                {
                    TBKompos.Text = ListKompositorsSelStud[lst - 1].KomposAndName;
                    CBKategory.Text = ListKompositorsSelStud[lst - 1].Kategorii.NameKategory;
                    SelectedKategory = ListKompositorsSelStud[lst - 1].Kategorii;
                    SelectedKompositor = ListKompositorsSelStud[lst - 1];
                }
            }
            else
            {
                int lst = ListKompositorsSelStud.FindIndex(t => t.ID_Kompos == SelectedKompositor.ID_Kompos);
                if (lst < ListKompositorsSelStud.Count - 1)
                {
                    TBKompos.Text = ListKompositorsSelStud[lst + 1].KomposAndName;
                    CBKategory.Text = ListKompositorsSelStud[lst + 1].Kategorii.NameKategory;
                    SelectedKategory = ListKompositorsSelStud[lst + 1].Kategorii;
                    SelectedKompositor = ListKompositorsSelStud[lst + 1];
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
                List<Kompositors> kompositors = Entities.Kompositors.Where(t => t.id_Student == SelectedStud.ID_Student).ToList();
                if (kompositors != null)
                {
                    foreach (var item in kompositors)
                    {
                        Entities.Kompositors.Remove(item);
                        Entities.SaveChanges();
                    }
                }
                Entities.Students.Remove(SelectedStud);
                Entities.SaveChanges();
                ClearAllBoxes();
                UpdateDG();
            }
        }
    }
}
