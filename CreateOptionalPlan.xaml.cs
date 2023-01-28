using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для CreateOptionalPlan.xaml
    /// </summary>
    public partial class CreateOptionalPlan : Page
    {
        PlanEntities Entities = new PlanEntities();

        private FileInfo _fileInfo;

        Prepodavateli _Prepod;
        List<Students> _LstStud = new List<Students>();
        List<Students> _LstStudMonday = new List<Students>();
        List<Students> _LstStudTuesday = new List<Students>();
        List<Students> _LstStudWednesday = new List<Students>();
        List<Students> _LstStudThursday = new List<Students>();
        List<Students> _LstStudFriday = new List<Students>();
        List<Students> _LstStudSaturday = new List<Students>();
        List<Students> _LstStudSunday = new List<Students>();

        Students students;
        List<int> Stages = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

        Kompositors _kompos1;
        Kompositors _kompos2;
        Kompositors _kompos3;

        Kategorii _kateg1;
        Kategorii _kateg2;
        Kategorii _kateg3;

        int _stage1;
        int _stage2;
        int _stage3;

        List<string> Text1 = new List<string>(3);
        List<string> Text2 = new List<string>(3);
        List<string> Text3 = new List<string>(3);

        int _indexList = 0;
        int _month = 0;

        string _Path = "";
        string PathSave = "";
        string PathMonth = "";

        DateTime DateSession;
        DateTime DateSertified;
        DateTime DateStop;
        

        public CreateOptionalPlan(bool Edit, DateTime _DateStart, DateTime _DateStop, Prepodavateli prepodavatel, string path )
        {
            InitializeComponent();
            DateSession = _DateStart;
            DateStop = _DateStop;
            _Prepod = prepodavatel;
            _Path= path;
            StudentsInDaysLists(prepodavatel);
            EnabledContent(Edit);
            //bool _check = false;
            for (int i = 0; Properties.Settings.Default.Stop != true; i++)
            {
                if (_CheckDayofWeek(DateSession))
                {
                    LoadStud(students);
                    //_check = true;

                }
            }
        }

        private void StudentsInDaysLists(Prepodavateli prepodavatel)
        {
            _LstStud = Entities.Students.Where(t => t.id_Prepod == prepodavatel.ID_Prepodavatel).ToList();
            foreach (var item in _LstStud)
            {
                if (item.Monday == true)
                {
                    _LstStudMonday.Add(item);
                }
                if (item.Tuesday == true)
                {
                    _LstStudTuesday.Add(item);
                }
                if (item.Wednesday == true)
                {
                    _LstStudWednesday.Add(item);
                }
                if (item.Thursday == true)
                {
                    _LstStudThursday.Add(item);
                }
                if (item.Friday == true)
                {
                    _LstStudFriday.Add(item);
                }
                if (item.Saturday == true)
                {
                    _LstStudSaturday.Add(item);
                }
            }
        }

        private bool _CheckDayofWeek(DateTime date)
        {
            if (date.Date > DateStop.Date)
            {
                MessageBox.Show("Все отчеты в диапазоне выбраных дат, созданы!", "Выполнено");
                
                Properties.Settings.Default.Stop = true;
                Properties.Settings.Default.StartDate = DateSession;
                Properties.Settings.Default.StopDate = DateSession.AddMonths(1);
                BtnNextPlan.IsEnabled = false;
                BtnStopCreatePlan.IsEnabled = false;
                return false;
            }
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return _CheckStud(_LstStudMonday);
                case DayOfWeek.Tuesday:
                    return _CheckStud(_LstStudTuesday);
                case DayOfWeek.Wednesday:
                    return _CheckStud(_LstStudWednesday);
                case DayOfWeek.Thursday:
                    return _CheckStud(_LstStudThursday);
                case DayOfWeek.Friday:
                    return _CheckStud(_LstStudFriday);
                case DayOfWeek.Saturday:
                    return _CheckStud(_LstStudSaturday);
                case DayOfWeek.Sunday:
                    return _CheckStud(_LstStudSunday);
                default:
                    return false;
            }
        }

        private bool _CheckStud(List<Students> lstStud)
        {
            if (_indexList < lstStud.Count())
            {
                students = lstStud[_indexList];
                if (_month != DateSession.Month)
                {
                    _month = DateSession.Month;
                    PathMonth = System.IO.Path.Combine(_Path, GetMonthName.GetNameMonth(_month));
                }
                PathSave = System.IO.Path.Combine(PathMonth, DateSession.ToString("dd.MM.yyyy"));
                System.IO.Directory.CreateDirectory(PathSave);
                return true;
            }
            else
            {
                _indexList = 0;
                DateSession = DateSession.AddDays(1);
                return false;
            }
        }

        private void LoadStud(Students students)
        {
            _kompos1 = null;
            _kompos2 = null;
            _kompos3 = null;
            LblStudName.Content = students.NameFile;
            LblStudGroup.Content = students.StudGroup;
            LblStudCourse.Content = students.StudCourse;
            DateSertified = DateSession.AddDays(-7);

            if (DateSertified < Convert.ToDateTime("01.09." + DateTime.Now.Year.ToString()))
            {
                DateSertified = Convert.ToDateTime("01.09." + DateTime.Now.Year.ToString());
            }

            LblDateSertified.Content = "«" + DateSertified.Day + "» " + GetMonthName.GetName(DateSertified.Month) + " " + DateSertified.Year;
            LblDateSession.Content = DateSession.Day + "." + DateSession.Month + "." + DateSession.Year;
            CBKompos1.ItemsSource = Entities.Kompositors.Where(t => t.id_Student == students.ID_Student).ToList();
            CBKompos1.DisplayMemberPath = "KomposAndName";
            if (students.LastKompos1 != null && Entities.Kompositors.FirstOrDefault(t=>t.ID_Kompos == students.LastKompos1) != default)
            {
                CBKompos1.Text = Entities.Kompositors.FirstOrDefault(t => t.ID_Kompos == students.LastKompos1).KomposAndName;
            }
            CBKompos2.ItemsSource = Entities.Kompositors.Where(t => t.id_Student == students.ID_Student).ToList();
            CBKompos2.DisplayMemberPath = "KomposAndName";
            if (students.LastKompos2 != null && Entities.Kompositors.FirstOrDefault(t => t.ID_Kompos == students.LastKompos2) != default)
            {
                CBKompos2.Text = Entities.Kompositors.FirstOrDefault(t => t.ID_Kompos == students.LastKompos2).KomposAndName;
            }
            CBKompos3.ItemsSource = Entities.Kompositors.Where(t => t.id_Student == students.ID_Student).ToList();
            CBKompos3.DisplayMemberPath = "KomposAndName";
            if (students.LastKompos3 != null && Entities.Kompositors.FirstOrDefault(t => t.ID_Kompos == students.LastKompos3) != default)
            {
                CBKompos3.Text = Entities.Kompositors.FirstOrDefault(t => t.ID_Kompos == students.LastKompos3).KomposAndName;
            }
        }

        public void EnabledContent(bool Edit)
        {
            if (Edit == true)
            {
                BtnNextPlan.IsEnabled = true;
                BtnStopCreatePlan.IsEnabled = true;
                CBKategory1.ItemsSource = Entities.Kategorii.ToList();
                CBKategory1.DisplayMemberPath = "NameKategory";
                CBKategory2.ItemsSource = Entities.Kategorii.ToList();
                CBKategory2.DisplayMemberPath = "NameKategory";
                CBKategory3.ItemsSource = Entities.Kategorii.ToList();
                CBKategory3.DisplayMemberPath = "NameKategory";
                CBStage1.ItemsSource = Stages;
                CBStage2.ItemsSource = Stages;
                CBStage3.ItemsSource = Stages;
            }
        }

        private void CBKompos1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Kompositors)CBKompos1.SelectedItem != null)
            {
                _kompos1 = (Kompositors)CBKompos1.SelectedItem;
                _kateg1 = ((Kompositors)CBKompos1.SelectedItem).Kategorii;
                CBKategory1.Text = _kateg1.NameKategory;
                if (((Kompositors)CBKompos1.SelectedItem).LastStage != null)
                {
                    int Stage = (int)((Kompositors)CBKompos1.SelectedItem).LastStage;
                    CBStage1.Text = Stage.ToString();
                }
                else
                {
                    CBStage1.SelectedIndex = -1;
                    _stage1 = 0;
                }
            }
            else
            {
                CBKategory1.SelectedIndex = -1;
                CBStage1.SelectedIndex = -1;
                TBText1.Text = "";
                _kompos1 = null;
                _kateg1 = null;
            }
        }

        private void CBKompos2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Kompositors)CBKompos2.SelectedItem != null)
            {
                _kompos2 = (Kompositors)CBKompos2.SelectedItem;
                _kateg2 = ((Kompositors)CBKompos2.SelectedItem).Kategorii;
                CBKategory2.Text = _kateg2.NameKategory;
                if (((Kompositors)CBKompos2.SelectedItem).LastStage != null)
                {
                    int Stage = (int)((Kompositors)CBKompos2.SelectedItem).LastStage;
                    CBStage2.Text = Stage.ToString();
                    TBText2.Text = Entities.OriginalText.FirstOrDefault(t => t.Stage == Stage && t.id_Kategory == ((Kategorii)CBKategory2.SelectedItem).ID_Kategoriya).TextCompose;
                }
                else
                {
                    CBStage2.SelectedIndex = -1;
                }
            }
            else
            {
                CBKategory2.SelectedIndex = -1;
                CBStage2.SelectedIndex = -1;
                TBText2.Text = "";
                _kompos2 = null;
                _kateg2 = null;
            }
        }

        private void CBKompos3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Kompositors)CBKompos3.SelectedItem != null)
            {
                _kompos3 = (Kompositors)CBKompos3.SelectedItem;
                _kateg3 = ((Kompositors)CBKompos3.SelectedItem).Kategorii;
                CBKategory3.Text = _kateg3.NameKategory;
                if (((Kompositors)CBKompos3.SelectedItem).LastStage != null)
                {
                    int Stage = (int)((Kompositors)CBKompos3.SelectedItem).LastStage;
                    CBStage3.Text = Stage.ToString();
                    TBText3.Text = Entities.OriginalText.FirstOrDefault(t => t.Stage == Stage && t.id_Kategory == ((Kategorii)CBKategory3.SelectedItem).ID_Kategoriya).TextCompose;
                }
                else
                {
                    CBStage3.SelectedIndex = -1;
                }
            }
            else
            {
                CBKategory3.SelectedIndex = -1;
                CBStage3.SelectedIndex = -1;
                TBText3.Text = "";
                _kompos3 = null;
                _kateg3 = null;
            }
        }

        private void CBKategory1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBKategory1.SelectedIndex != -1)
            {
                CBStage1.IsEnabled = true;
            }
            else
            {
                CBStage1.IsEnabled = false;
            }
        }

        private void CBKategory2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBKategory2.SelectedIndex != -1)
            {
                CBStage2.IsEnabled = true;
            }
            else
            {
                CBStage2.IsEnabled = false;
            }
        }

        private void CBKategory3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBKategory3.SelectedIndex != -1)
            {
                CBStage3.IsEnabled = true;
            }
            else
            {
                CBStage3.IsEnabled = false;
            }
        }

        private void BtnCloseKompos1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CBKompos1.SelectedIndex = -1;
        }

        private void BtnCloseKompos2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CBKompos2.SelectedIndex = -1;
        }

        private void BtnCloseKompos3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CBKompos3.SelectedIndex = -1;
        }

        private void CBStage1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBStage1.SelectedIndex != -1)
            {
                _stage1 = (int)CBStage1.Items.GetItemAt(CBStage1.SelectedIndex);
                OriginalText originalText = Entities.OriginalText.FirstOrDefault(t => t.id_Kategory == _kateg1.ID_Kategoriya && t.Stage == _stage1 && t.id_Prepodavatel == _Prepod.ID_Prepodavatel);
                if (originalText != default)
                {
                    TBText1.Text = originalText.TextCompose;
                }
                else
                {
                    TBText1.Text = "";
                }
            }
            else
            {
                TBText1.Text = "";
            }
        }

        private void CBStage2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBStage2.SelectedIndex != -1)
            {
                _stage2 = (int)CBStage2.Items.GetItemAt(CBStage2.SelectedIndex);
                OriginalText originalText = Entities.OriginalText.FirstOrDefault(t => t.id_Kategory == _kateg2.ID_Kategoriya && t.Stage == _stage2 && t.id_Prepodavatel == _Prepod.ID_Prepodavatel);
                if (originalText != default)
                {
                    TBText2.Text = originalText.TextCompose;
                }
                else
                {
                    TBText2.Text = "";
                }
            }
            else
            {
                TBText2.Text = "";
            }
        }

        private void CBStage3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBStage3.SelectedIndex != -1)
            {
                _stage3 = (int)CBStage3.Items.GetItemAt(CBStage3.SelectedIndex);
                OriginalText originalText = Entities.OriginalText.FirstOrDefault(t => t.id_Kategory == _kateg3.ID_Kategoriya && t.Stage == _stage3 && t.id_Prepodavatel == _Prepod.ID_Prepodavatel);
                if (originalText != default)
                {
                    TBText3.Text = originalText.TextCompose;
                }
                else
                {
                    TBText3.Text = "";
                }
            }
            else
            {
                TBText3.Text = "";
            }
        }

        private void BtnNextPlan_Click(object sender, RoutedEventArgs e)
        {
            //внесение изменений в шаблон сохранение и переход к следующему студенту
            BtnNextPlan.IsEnabled = false;
            BtnStopCreatePlan.IsEnabled = false;
            var items = new Dictionary<string, string>
            {
                {"<SertifDt>", DateSertified.ToString("«d» ") + GetMonthName.GetName(DateSertified.Month) + " " + DateSertified.ToString("yyyy") + "г." },
                {"<Name>", students.NameText},
                {"dd", DateSession.ToString("dd")},
                {"mm", DateSession.ToString("MM")},
                {"yyyy", DateSession.ToString("yyyy")},
                {"<Course>", students.StudCourse.ToString()},
                {"<Group>", students.StudGroup.ToString()},
                {"<Theme>", Properties.Settings.Default.ThemeLesson},
                {"<TargLearn>", Properties.Settings.Default.TargetLearning},
                {"<TargDev>", Properties.Settings.Default.TargetDevelopment},
                {"<TargEduc>", Properties.Settings.Default.TargetEducation},
                {"<FormLess>", Properties.Settings.Default.FormLesson},
                {"<MethLearn>", Properties.Settings.Default.MethodLearning},
                {"<LearnTools>", Properties.Settings.Default.LearningTools},
                {"<FrstPt>", Properties.Settings.Default.FirstPart},
                {"<TwPt>", Properties.Settings.Default.TwoPart},
                {"<Prepodavatel>", _Prepod.Name},
                {"<FinishSertDt>", DateSertified.ToString("dd.MM.yyyy") + "г."}
            };
            if (_kompos1 != null && _kompos2 != null && _kompos3 != null)
            {
                _fileInfo = new FileInfo(System.IO.Directory.GetCurrentDirectory() + "\\Shablon\\ShablonPlana3.docx");
                WordReplacer.WholeChunks(TBText1.Text, Text1);
                WordReplacer.WholeChunks(TBText2.Text, Text2);
                WordReplacer.WholeChunks(TBText3.Text, Text3);
                items.Add("<Compose1>", _kompos1.KomposAndName);
                items.Add("<Compose2>", _kompos2.KomposAndName);
                items.Add("<Compose3>", _kompos3.KomposAndName);
                items.Add("<Text1>", Text1[0]);
                items.Add("<Text11>", Text1[1]);
                items.Add("<Text111>", Text1[2]);
                items.Add("<Text2>", Text2[0]);
                items.Add("<Text22>", Text2[1]);
                items.Add("<Text222>", Text2[2]);
                items.Add("<Text3>", Text3[0]);
                items.Add("<Text33>", Text3[1]);
                items.Add("<Text333>", Text3[2]);
                _kompos1.LastStage = _stage1;
                _kompos2.LastStage = _stage2;
                _kompos3.LastStage = _stage3;
                students.LastKompos1 = _kompos1.ID_Kompos;
                students.LastKompos2 = _kompos2.ID_Kompos;
                students.LastKompos3 = _kompos3.ID_Kompos;
                Entities.SaveChanges();
            }
            else if (_kompos1 != null && _kompos2 != null && _kompos3 == null)
            {
                _fileInfo = new FileInfo(System.IO.Directory.GetCurrentDirectory() + "\\Shablon\\ShablonPlana2.docx");
                WordReplacer.WholeChunks(TBText1.Text, Text1);
                WordReplacer.WholeChunks(TBText2.Text, Text2);
                items.Add("<Compose1>", _kompos1.KomposAndName);
                items.Add("<Compose2>", _kompos2.KomposAndName);
                items.Add("<Text1>", Text1[0]);
                items.Add("<Text11>", Text1[1]);
                items.Add("<Text111>", Text1[2]);
                items.Add("<Text2>", Text2[0]);
                items.Add("<Text22>", Text2[1]);
                items.Add("<Text222>", Text2[2]);
                _kompos1.LastStage = _stage1;
                _kompos2.LastStage = _stage2;
                students.LastKompos1 = _kompos1.ID_Kompos;
                students.LastKompos2 = _kompos2.ID_Kompos;
                students.LastKompos3 = null;
                Entities.SaveChanges();
            }
            else if (_kompos1 != null && _kompos2 == null && _kompos3 == null)
            {
                _fileInfo = new FileInfo(System.IO.Directory.GetCurrentDirectory() + "\\Shablon\\ShablonPlana1.docx");
                WordReplacer.WholeChunks(TBText1.Text, Text1);
                items.Add("<Compose1>", _kompos1.KomposAndName);
                items.Add("<Text1>", Text1[0]);
                items.Add("<Text11>", Text1[1]);
                items.Add("<Text111>", Text1[2]);
                _kompos1.LastStage = _stage1;
                students.LastKompos1 = _kompos1.ID_Kompos;
                students.LastKompos2 = null;
                students.LastKompos3 = null;
                Entities.SaveChanges();
            }
            else
            {
                MessageBox.Show("Композитор1 не может быть пустым", "Пердупреждение");
                return;
            }

            if (WordReplacer.ReplaceWord(items, PathSave, DateSession, students, _fileInfo))
            {
                _indexList += 1;
                //bool _check = false;
                for (int i = 0; Properties.Settings.Default.Stop != true; i++)
                {
                    if (_CheckDayofWeek(DateSession))
                    {
                        LoadStud(students);
                        //_check = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка в программе");
            }
            Text1 = new List<string>(3);
            Text2 = new List<string>(3);
            Text3 = new List<string>(3);

            BtnNextPlan.IsEnabled = true;
            BtnStopCreatePlan.IsEnabled = true;
        }

        private void BtnStopCreatePlan_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Stop = true;
            Properties.Settings.Default.StartDate = DateSession;
            Properties.Settings.Default.StopDate = DateSession.AddMonths(1);
            BtnNextPlan.IsEnabled = false;
        }
    }
}
