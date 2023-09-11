using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanMVMU.DataBase;

namespace PlanMVMU
{
    public class GetMonthName
    {
        public static string GetName(int Month)
        {
            string MonthName = "";
            switch (Month)
            {
                case 1:
                    MonthName = "января";
                    break;
                case 2:
                    MonthName = "февраля";
                    break;
                case 3:
                    MonthName = "марта";
                    break;
                case 4:
                    MonthName = "апреля";
                    break;
                case 5:
                    MonthName = "мая";
                    break;
                case 6:
                    MonthName = "июня";
                    break;
                case 7:
                    MonthName = "июля";
                    break;
                case 8:
                    MonthName = "августа";
                    break;
                case 9:
                    MonthName = "сентября";
                    break;
                case 10:
                    MonthName = "октября";
                    break;
                case 11:
                    MonthName = "ноября";
                    break;
                case 12:
                    MonthName = "декабря";
                    break;
                default:
                    ;
                    break;
            }
            return MonthName;
        }

        public static string GetNameMonth(int Month)
        {
            string MonthName = "";
            switch (Month)
            {
                case 1:
                    MonthName = "Январь";
                    break;
                case 2:
                    MonthName = "Февраль";
                    break;
                case 3:
                    MonthName = "Март";
                    break;
                case 4:
                    MonthName = "Апрель";
                    break;
                case 5:
                    MonthName = "Май";
                    break;
                case 6:
                    MonthName = "Июнь";
                    break;
                case 7:
                    MonthName = "Июль";
                    break;
                case 8:
                    MonthName = "Август";
                    break;
                case 9:
                    MonthName = "Сентябрь";
                    break;
                case 10:
                    MonthName = "Октябрь";
                    break;
                case 11:
                    MonthName = "Ноябрь";
                    break;
                case 12:
                    MonthName = "Декабрь";
                    break;
                default:
                    ;
                    break;
            }
            return MonthName;
        }

        public static string GetDayOfWeek(string Day)
        {
            switch (Day)
            {
                case "Monday":
                    return "Понедельник";
                case "Tuesday":
                    return "Вторник";
                case "Wednesday":
                    return "Среда";
                case "Thursday":
                    return "Четверг";
                case "Friday":
                    return "Пятница";
                case "Saturday":
                    return "Суббота";
                default:
                    return "";
            }
        }

        public static string GetNameDays(Student stud)
        {
            string Days = "";
            for (int i = 0; i <= 5; i++)
            {
                if (i == 0 && stud.Monday == true)
                {
                    Days += "Понедельник\n";
                }
                if (i == 1 && stud.Tuesday == true)
                {
                    Days += "Вторник\n";
                }
                if (i == 2 && stud.Wednesday == true)
                {
                    Days += "Среда\n";
                }
                if (i == 3 && stud.Thursday == true)
                {
                    Days += "Четверг\n";
                }
                if (i == 4 && stud.Friday == true)
                {
                    Days += "Пятница\n";
                }
                if (i == 5 && stud.Saturday == true)
                {
                    Days += "Суббота\n";
                }
            }
            Days = Days.Remove(Days.Length-1);
            return Days;
        }
    }
}
