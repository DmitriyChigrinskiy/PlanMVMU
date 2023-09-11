using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PlanMVMU.DataBase;

namespace PlanMVMU
{
    public partial class CreateOptionalPlan : Page
    {
        Entities Entities = new Entities();

        private FileInfo _fileInfo;

        Teacher _teacher;
        Student _student;

        List<Student> _listStudents = new List<Student>();
        List<Student> _listStudentsMonday = new List<Student>();
        List<Student> _listStudentsTuesday = new List<Student>();
        List<Student> _listStudentsWednesday = new List<Student>();
        List<Student> _listStudentsThursday = new List<Student>();
        List<Student> _listStudentsFriday = new List<Student>();
        List<Student> _listStudentsSaturday = new List<Student>();
        List<Student> _listStudentsSunday = new List<Student>();


        List<int> Stages = new List<int>(30) { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30};

        Composer _composer1;
        Composer _composer2;
        Composer _composer3;

        Category _category1;
        Category _category2;
        Category _category3;

        private int _stage1;
        private int _stage2;
        private int _stage3;

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
        

        public CreateOptionalPlan(bool Edit, DateTime _DateStart, DateTime _DateStop, Teacher prepodavatel, string path )
        {
            InitializeComponent();
            DateSession = _DateStart;
            DateStop = _DateStop;
            _teacher = prepodavatel;
            _Path= path;
            StudentsInDaysLists(prepodavatel);
            EnabledContent(Edit);
            //bool _check = false;
            for (int i = 0; Properties.Settings.Default.Stop == false; i++)
            {
                if (_CheckDayofWeek(DateSession))
                {
                    LoadStud(_student);
                    //_check = true;
                }
            }
        }

        private void StudentsInDaysLists(Teacher prepodavatel)
        {
            _listStudents = Entities.Student.Where(t => t.id_Teacher == prepodavatel.ID_Teacher).ToList();
            foreach (var item in _listStudents)
            {
                if (item.Monday == true)
                {
                    _listStudentsMonday.Add(item);
                }
                if (item.Tuesday == true)
                {
                    _listStudentsTuesday.Add(item);
                }
                if (item.Wednesday == true)
                {
                    _listStudentsWednesday.Add(item);
                }
                if (item.Thursday == true)
                {
                    _listStudentsThursday.Add(item);
                }
                if (item.Friday == true)
                {
                    _listStudentsFriday.Add(item);
                }
                if (item.Saturday == true)
                {
                    _listStudentsSaturday.Add(item);
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
                    return _CheckStud(_listStudentsMonday);
                case DayOfWeek.Tuesday:
                    return _CheckStud(_listStudentsTuesday);
                case DayOfWeek.Wednesday:
                    return _CheckStud(_listStudentsWednesday);
                case DayOfWeek.Thursday:
                    return _CheckStud(_listStudentsThursday);
                case DayOfWeek.Friday:
                    return _CheckStud(_listStudentsFriday);
                case DayOfWeek.Saturday:
                    return _CheckStud(_listStudentsSaturday);
                case DayOfWeek.Sunday:
                    return _CheckStud(_listStudentsSunday);
                default:
                    return false;
            }
        }

        private bool _CheckStud(List<Student> lstStud)
        {
            if (_indexList < lstStud.Count())
            {
                _student = lstStud[_indexList];
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

        private void LoadStud(Student students)
        {
            _composer1 = null;
            _composer2 = null;
            _composer3 = null;
            LblStudentName.Content = students.NameFile;
            LblStudentGroup.Content = students.StudentGroup;
            LblStudentCourse.Content = students.StudentCourse;
            DateSertified = DateSession.AddDays(-7);

            if (DateSertified < Convert.ToDateTime("01.09." + DateTime.Now.Year.ToString()) && DateSertified > Convert.ToDateTime("01.06." + DateTime.Now.Year.ToString()))
            {
                DateSertified = Convert.ToDateTime("01.09." + DateTime.Now.Year.ToString());
            }

            LblDateSertified.Content = "«" + DateSertified.Day + "» " + GetMonthName.GetName(DateSertified.Month) + " " + DateSertified.Year;
            LblDateSession.Content = DateSession.Day + "." + DateSession.Month + "." + DateSession.Year;
            CBComposer1.ItemsSource = Entities.Composer.Where(t => t.id_Student == students.ID_Student).ToList();
            CBComposer1.DisplayMemberPath = "ComposerAndComposition";
            if (students.LastComposer1 != null && Entities.Composer.FirstOrDefault(t=>t.ID_Composer == students.LastComposer1) != default)
            {
                CBComposer1.Text = Entities.Composer.FirstOrDefault(t => t.ID_Composer == students.LastComposer1).ComposerAndComposition;
            }
            CBComposer2.ItemsSource = Entities.Composer.Where(t => t.id_Student == students.ID_Student).ToList();
            CBComposer2.DisplayMemberPath = "ComposerAndComposition";
            if (students.LastComposer2 != null && Entities.Composer.FirstOrDefault(t => t.ID_Composer == students.LastComposer2) != default)
            {
                CBComposer2.Text = Entities.Composer.FirstOrDefault(t => t.ID_Composer == students.LastComposer2).ComposerAndComposition;
            }
            CBComposer3.ItemsSource = Entities.Composer.Where(t => t.id_Student == students.ID_Student).ToList();
            CBComposer3.DisplayMemberPath = "ComposerAndComposition";
            if (students.LastComposer3 != null && Entities.Composer.FirstOrDefault(t => t.ID_Composer == students.LastComposer3) != default)
            {
                CBComposer3.Text = Entities.Composer.FirstOrDefault(t => t.ID_Composer == students.LastComposer3).ComposerAndComposition;
            }
            Properties.Settings.Default.Stop = true;
        }

        public void EnabledContent(bool Edit)
        {
            if (Edit == true)
            {
                BtnNextPlan.IsEnabled = true;
                BtnStopCreatePlan.IsEnabled = true;
                CBCategory1.ItemsSource = Entities.Category.ToList();
                CBCategory1.DisplayMemberPath = "NameCategory";
                CBCategory2.ItemsSource = Entities.Category.ToList();
                CBCategory2.DisplayMemberPath = "NameCategory";
                CBCategory3.ItemsSource = Entities.Category.ToList();
                CBCategory3.DisplayMemberPath = "NameCategory";
                CBStage1.ItemsSource = Stages;
                CBStage2.ItemsSource = Stages;
                CBStage3.ItemsSource = Stages;
            }
        }

        private void CBKompos1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Composer)CBComposer1.SelectedItem != null)
            {
                _composer1 = (Composer)CBComposer1.SelectedItem;
                _category1 = ((Composer)CBComposer1.SelectedItem).Category;
                CBCategory1.Text = _category1.NameCategory;
                if (((Composer)CBComposer1.SelectedItem).LastStage != null)
                {
                    int Stage = (int)((Composer)CBComposer1.SelectedItem).LastStage;
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
                CBCategory1.SelectedIndex = -1;
                CBStage1.SelectedIndex = -1;
                TBText1.Text = "";
                _composer1 = null;
                _category1 = null;
            }
        }

        private void CBKompos2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Composer)CBComposer2.SelectedItem != null)
            {
                _composer2 = (Composer)CBComposer2.SelectedItem;
                _category2 = ((Composer)CBComposer2.SelectedItem).Category;
                CBCategory2.Text = _category2.NameCategory;
                if (((Composer)CBComposer2.SelectedItem).LastStage != null)
                {
                    int Stage = (int)((Composer)CBComposer2.SelectedItem).LastStage;
                    CBStage2.Text = Stage.ToString();
                    TBText2.Text = Entities.OriginalText.FirstOrDefault(t => t.Stage == Stage && t.id_Category == ((Category)CBCategory2.SelectedItem).ID_Category).Text;
                }
                else
                {
                    CBStage2.SelectedIndex = -1;
                }
            }
            else
            {
                CBCategory2.SelectedIndex = -1;
                CBStage2.SelectedIndex = -1;
                TBText2.Text = "";
                _composer2 = null;
                _category2 = null;
            }
        }

        private void CBKompos3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Composer)CBComposer3.SelectedItem != null)
            {
                _composer3 = (Composer)CBComposer3.SelectedItem;
                _category3 = ((Composer)CBComposer3.SelectedItem).Category;
                CBCategory3.Text = _category3.NameCategory;
                if (((Composer)CBComposer3.SelectedItem).LastStage != null)
                {
                    int Stage = (int)((Composer)CBComposer3.SelectedItem).LastStage;
                    CBStage3.Text = Stage.ToString();
                    TBText3.Text = Entities.OriginalText.FirstOrDefault(t => t.Stage == Stage && t.id_Category == ((Category)CBCategory3.SelectedItem).ID_Category).Text;
                }
                else
                {
                    CBStage3.SelectedIndex = -1;
                }
            }
            else
            {
                CBCategory3.SelectedIndex = -1;
                CBStage3.SelectedIndex = -1;
                TBText3.Text = "";
                _composer3 = null;
                _category3 = null;
            }
        }

        private void CBKategory1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBCategory1.SelectedIndex != -1)
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
            if (CBCategory2.SelectedIndex != -1)
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
            if (CBCategory3.SelectedIndex != -1)
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
            CBComposer1.SelectedIndex = -1;
        }

        private void BtnCloseKompos2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CBComposer2.SelectedIndex = -1;
        }

        private void BtnCloseKompos3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CBComposer3.SelectedIndex = -1;
        }

        private void CBStage1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBStage1.SelectedIndex != -1)
            {
                _stage1 = (int)CBStage1.Items.GetItemAt(CBStage1.SelectedIndex);
                OriginalText originalText = Entities.OriginalText.FirstOrDefault(t => t.id_Category == _category1.ID_Category && t.Stage == _stage1 && t.id_Teacher == _teacher.ID_Teacher);
                if (originalText != default)
                {
                    TBText1.Text = originalText.Text;
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
                OriginalText originalText = Entities.OriginalText.FirstOrDefault(t => t.id_Category == _category2.ID_Category && t.Stage == _stage2 && t.id_Teacher == _teacher.ID_Teacher);
                if (originalText != default)
                {
                    TBText2.Text = originalText.Text;
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
                OriginalText originalText = Entities.OriginalText.FirstOrDefault(t => t.id_Category == _category3.ID_Category && t.Stage == _stage3 && t.id_Teacher == _teacher.ID_Teacher);
                if (originalText != default)
                {
                    TBText3.Text = originalText.Text;
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
                {"<Name>", _student.NameText},
                {"dd", DateSession.ToString("dd")},
                {"mm", DateSession.ToString("MM")},
                {"yyyy", DateSession.ToString("yyyy")},
                {"<Course>", _student.StudentCourse.ToString()},
                {"<Group>", _student.StudentGroup.ToString()},
                {"<Theme>", Properties.Settings.Default.ThemeLesson},
                {"<TargLearn>", Properties.Settings.Default.TargetLearning},
                {"<TargDev>", Properties.Settings.Default.TargetDevelopment},
                {"<TargEduc>", Properties.Settings.Default.TargetEducation},
                {"<FormLess>", Properties.Settings.Default.FormLesson},
                {"<MethLearn>", Properties.Settings.Default.MethodLearning},
                {"<LearnTools>", Properties.Settings.Default.LearningTools},
                {"<FrstPt>", Properties.Settings.Default.FirstPart},
                {"<TwPt>", Properties.Settings.Default.TwoPart},
                {"<Prepodavatel>", _teacher.TeacherName},
                {"<FinishSertDt>", DateSertified.ToString("dd.MM.yyyy") + "г."}
            };
            if (_composer1 != null && _composer2 != null && _composer3 != null)
            {
                _fileInfo = new FileInfo(System.IO.Directory.GetCurrentDirectory() + "\\Views\\Resources\\PlanTemplates\\PlanTemplates3.docx");
                WordReplacer.WholeChunks(TBText1.Text, Text1);
                WordReplacer.WholeChunks(TBText2.Text, Text2);
                WordReplacer.WholeChunks(TBText3.Text, Text3);
                items.Add("<Compose1>", _composer1.ComposerAndComposition);
                items.Add("<Compose2>", _composer2.ComposerAndComposition);
                items.Add("<Compose3>", _composer3.ComposerAndComposition);
                items.Add("<Text1>", Text1[0]);
                items.Add("<Text11>", Text1[1]);
                items.Add("<Text111>", Text1[2]);
                items.Add("<Text2>", Text2[0]);
                items.Add("<Text22>", Text2[1]);
                items.Add("<Text222>", Text2[2]);
                items.Add("<Text3>", Text3[0]);
                items.Add("<Text33>", Text3[1]);
                items.Add("<Text333>", Text3[2]);
                _composer1.LastStage = _stage1;
                _composer2.LastStage = _stage2;
                _composer3.LastStage = _stage3;
                _student.LastComposer1 = _composer1.ID_Composer;
                _student.LastComposer2 = _composer2.ID_Composer;
                _student.LastComposer3 = _composer3.ID_Composer;
                Entities.SaveChanges();
            }
            else if (_composer1 != null && _composer2 != null && _composer3 == null)
            {
                _fileInfo = new FileInfo(System.IO.Directory.GetCurrentDirectory() + "\\Shablon\\ShablonPlana2.docx");
                WordReplacer.WholeChunks(TBText1.Text, Text1);
                WordReplacer.WholeChunks(TBText2.Text, Text2);
                items.Add("<Compose1>", _composer1.ComposerAndComposition);
                items.Add("<Compose2>", _composer2.ComposerAndComposition);
                items.Add("<Text1>", Text1[0]);
                items.Add("<Text11>", Text1[1]);
                items.Add("<Text111>", Text1[2]);
                items.Add("<Text2>", Text2[0]);
                items.Add("<Text22>", Text2[1]);
                items.Add("<Text222>", Text2[2]);
                _composer1.LastStage = _stage1;
                _composer2.LastStage = _stage2;
                _student.LastComposer1 = _composer1.ID_Composer;
                _student.LastComposer2 = _composer2.ID_Composer;
                _student.LastComposer3 = null;
                Entities.SaveChanges();
            }
            else if (_composer1 != null && _composer2 == null && _composer3 == null)
            {
                _fileInfo = new FileInfo(System.IO.Directory.GetCurrentDirectory() + "\\Shablon\\ShablonPlana1.docx");
                WordReplacer.WholeChunks(TBText1.Text, Text1);
                items.Add("<Compose1>", _composer1.ComposerAndComposition);
                items.Add("<Text1>", Text1[0]);
                items.Add("<Text11>", Text1[1]);
                items.Add("<Text111>", Text1[2]);
                _composer1.LastStage = _stage1;
                _student.LastComposer1 = _composer1.ID_Composer;
                _student.LastComposer2 = null;
                _student.LastComposer3 = null;
                Entities.SaveChanges();
            }
            else
            {
                MessageBox.Show("Композитор1 не может быть пустым", "Пердупреждение");
                return;
            }

            if (WordReplacer.ReplaceWord(items, PathSave, DateSession, _student, _fileInfo))
            {
                _indexList += 1;
                Properties.Settings.Default.Stop = false;
                for (int i = 0; Properties.Settings.Default.Stop != true; i++)
                {
                    if (_CheckDayofWeek(DateSession))
                    {
                        LoadStud(_student);
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
            mainframe.frame.IsEnabled = false;
            
        }
    }
}
