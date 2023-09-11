using System;
using System.Linq;
using System.Windows;

namespace PlanMVMU.DataBase.Operations
{
    class Class1
    {
    }

    public class AddCategory
    {
        Entities entities = new Entities();

        public AddCategory(string nameCategory, string windowName)
        {
            TryAddCategory(nameCategory, windowName);
        }

        private bool CheckAddedNameCategory(string nameCategory)
        {
            if (Checks.NotAnEmptyString(nameCategory))
            {
                return entities.Category.FirstOrDefault(t => t.NameCategory == nameCategory) == default;
            }
            else
            {
                return false;
            }
        }

        private bool TryAddCategory(string nameCategory, string windowName)
        {
            try
            {
                if (CheckAddedNameCategory(nameCategory))
                {
                    Category category = new Category
                    { NameCategory = nameCategory };
                    entities.Category.Add(category);
                    entities.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logs.CreateLogs.CreateLog(windowName, ex.Message, "Добавление категории.");
                return false;
            }
        }
    }

    public class DelCategory
    {
        Entities entities = new Entities();

        public DelCategory(int idCategory, string windowName)
        {
            TryDeleteCategory(idCategory, windowName);
        }

        private bool TryDeleteCategory(int idCategory, string windowName)
        {
            Category category = entities.Category.FirstOrDefault(t => t.ID_Category == idCategory);
            if (category != null)
            {
                try
                {
                    entities.Category.Remove(category);
                    entities.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Logs.CreateLogs.CreateLog(windowName, ex.Message, "Удаление категории.");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public partial class TeacherOperations
    {
        

        
    }
}
