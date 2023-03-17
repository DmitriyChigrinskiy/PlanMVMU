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
    /// Логика взаимодействия для AddInfoShablon.xaml
    /// </summary>
    public partial class AddInfoShablon : Window
    {
        public AddInfoShablon()
        {
            InitializeComponent();
            Theme.Text = Properties.Settings.Default.ThemeLesson;
            TargetLearn.Text = Properties.Settings.Default.TargetLearning;
            TargetDev.Text = Properties.Settings.Default.TargetDevelopment;
            TargetEduc.Text = Properties.Settings.Default.TargetEducation;
            FormLess.Text = Properties.Settings.Default.FormLesson;
            MethLearn.Text = Properties.Settings.Default.MethodLearning;
            LearnTools.Text = Properties.Settings.Default.LearningTools;
            FirstPt.Text = Properties.Settings.Default.FirstPart;
            TwoPt.Text = Properties.Settings.Default.TwoPart;
        }

        private void SaveCh_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ThemeLesson = Theme.Text;
            Properties.Settings.Default.TargetLearning = TargetLearn.Text;
            Properties.Settings.Default.TargetDevelopment = TargetDev.Text;
            Properties.Settings.Default.TargetEducation = TargetEduc.Text;
            Properties.Settings.Default.FormLesson = FormLess.Text;
            Properties.Settings.Default.MethodLearning = MethLearn.Text;
            Properties.Settings.Default.LearningTools = LearnTools.Text;
            Properties.Settings.Default.FirstPart = FirstPt.Text;
            Properties.Settings.Default.TwoPart = TwoPt.Text;
            MessageBox.Show("Сохранено");
        }
    }
}
