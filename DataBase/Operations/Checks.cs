using System;
using System.Linq;
using System.Windows;


namespace PlanMVMU.DataBase.Operations
{
    public class Checks
    {
        public static bool NotAnEmptyString(string text)
        {
            if (text != "")
            {
                if (text.Replace(" ", "").Length != 0)
                {
                    return true;
                }
            }
            MessageBox.Show("Пустая строка", "Предупреждение");
            return false;
        }

        public static bool CheckDuplicateRecordKompositors(string nameCompositor, int idStudent, int idCategory)
        {
            Entities entities = new Entities();
            if (entities.Composer.FirstOrDefault(t => t.ComposerAndComposition.ToUpper() == nameCompositor.ToUpper() &&
                                                        t.id_Category == idCategory && t.id_Student == idStudent) != default)
            {
                MessageBox.Show($"Данный композитор и произведение, категории '{entities.Category.Find(idCategory).NameCategory}',  уже имеется у данного студента.") ;
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool CheckNoDuplicateRecordText(int _idTeacher, string _text, int _idCategory)
        {
            Entities entities = new Entities();
            if (entities.OriginalText.FirstOrDefault(t => t.id_Teacher == _idTeacher && t.Text == _text && t.id_Category == _idCategory) == default)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Данный текст уже существует. Проверьте имеющиеся данные.", "Предупреждение");
                return false;
            }
        }

        public static bool CheckNoDuplicateRecordTeacher(string teacherName)
        {
            Entities entities = new Entities();
            try
            {
                if (entities.Teacher.FirstOrDefault(t => t.TeacherName == teacherName) != default)
                {
                    MessageBox.Show($"Преподаватель, {teacherName},  уже есть в базе данных.", "Предупреждение");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logs.CreateLogs.CreateLog("BackEnd Checks", ex.Message, "Проверка дублирования записи преподавателя");
                MessageBox.Show("Произошла ошибка. Данные отправлены в лог событий.", "Ошибка");
                return false;
            }
        }
    }
}
