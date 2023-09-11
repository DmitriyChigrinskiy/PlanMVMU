using System;
using System.Linq;
using System.Windows;

namespace PlanMVMU.DataBase.Operations
{
    partial class TeacherOperations
    {
        public static bool AddTeacher(string teacherName, Entities entities)
        {
            if (Checks.NotAnEmptyString(teacherName))
            {
                if (Checks.CheckNoDuplicateRecordTeacher(teacherName))
                {
                    try
                    {
                        Teacher teacher = new Teacher
                        {
                            TeacherName = teacherName
                        };
                        entities.Teacher.Add(teacher);
                        entities.SaveChanges();
                        MessageBox.Show($"Запись преподавателя {teacher.TeacherName} добавлена."); ;
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка. Данные занесены в лог событий.", "Ошибка");
                        Logs.CreateLogs.CreateLog("Background Teacher", ex.Message, "Добавление преподавателя");
                        return false;
                    }
                }
                return false;
            }
            return false;
        }

        public static bool EditTeacher(string newTeacherName, Teacher teacher, Entities entities)
        {
            if (Checks.NotAnEmptyString(newTeacherName))
            {
                if (Checks.CheckNoDuplicateRecordTeacher(newTeacherName))
                {
                    if (teacher.TeacherName != newTeacherName)
                    {
                        try
                        {
                            teacher.TeacherName = newTeacherName;
                            entities.SaveChanges();
                            MessageBox.Show("Данные преподавателя изменены.", "Выполнено");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Произошла ошибка. Данные занесены в лог событий.", "Ошибка");
                            Logs.CreateLogs.CreateLog("Background Teacher", ex.Message, "Добавление преподавателя");
                            return false;
                        }
                    }
                    else
                        MessageBox.Show("Введено то-же самое имя преподавателя. Изменения не требуются.", "Предупреждение", MessageBoxButton.OK);
                }
            }
            return false;
        }

        public static bool DeleteTeacher(Teacher teacher, Entities entities)
        {
            if (entities.Student.Where(t=>t.id_Teacher == teacher.ID_Teacher).Count() > 0)
            {
                if (MessageBox.Show("","",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
