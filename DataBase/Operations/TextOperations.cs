using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlanMVMU.DataBase.Operations
{
    class TextOperations
    {
        public static bool AddText(string _text, int _stage, int _idCategory, int _idTeacher, Entities entities)
        {
            if (Checks.NotAnEmptyString(_text) && Checks.CheckNoDuplicateRecordText(_idTeacher,_text, _idCategory))
            {
                try
                {
                    OriginalText originalText = new OriginalText()
                    {
                        Text = _text,
                        Stage = _stage,
                        id_Category = _idCategory,
                        id_Teacher = _idTeacher
                    };
                    entities.OriginalText.Add(originalText);
                    entities.SaveChanges();
                    MessageBox.Show("Текст добавлен.", "Выполнено", MessageBoxButton.OK);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка. Запись добавлена в лог событий.", "Ошибка");
                    Logs.CreateLogs.CreateLog("BackEnd TextOperations", ex.Message, "Добавление текста");
                    return false;
                }
            }
            return false;
        }

        public static bool EditText(int idOriginalText, string _text, int _stage, int _idCategory, int _idTeacher, Entities entities)
        {
            if (Checks.NotAnEmptyString(_text) && Checks.CheckNoDuplicateRecordText(_idTeacher, _text, _idCategory))
            {
                try
                {
                    OriginalText originalText = entities.OriginalText.FirstOrDefault(t => t.ID_OriginalText == idOriginalText);
                    if (originalText != default)
                    {
                        originalText.id_Category = _idCategory;
                        originalText.Stage = _stage;
                        originalText.Text = _text;
                        entities.SaveChanges();
                        MessageBox.Show("Текст успешно изменен.", "Выполнено", MessageBoxButton.OK);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Текст для редактирования не найден.", "Предупреждение");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при выполнении операции. Создана запись в логе событий.", "Ошибка", MessageBoxButton.OK);
                    Logs.CreateLogs.CreateLog("BackEnd TextOperations", ex.Message, "Изменение текста");
                    return false;
                }
            }
            return false;
        }

        public static bool DeleteText(int _idText, Entities entities)
        {
            if (MessageBox.Show("Действительно удалить данный текст?", "Подтверждение операции", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    OriginalText originalText = entities.OriginalText.FirstOrDefault(t => t.ID_OriginalText == _idText);
                    if (originalText != default)
                    {
                        entities.OriginalText.Remove(originalText);
                        entities.SaveChanges();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Текст для удаления не найден.", "Предупреждение", MessageBoxButton.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при выполнении операции. Создана запись в лог событий.", "Ошибка", MessageBoxButton.OK);
                    Logs.CreateLogs.CreateLog("BackEnd TextOperations", ex.Message, "Удаление текста");
                    return false;
                }
            }
            return false;
        }
    }
}
