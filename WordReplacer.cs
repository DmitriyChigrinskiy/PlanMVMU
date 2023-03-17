using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;
using PlanMVMU.DataBase;

namespace PlanMVMU
{
    class WordReplacer
    {


        public static void WholeChunks(string str, List<string> Text)
        {
            int stInd = 0;
            int chunk = 255;
            bool _else = true;
            for (int i = 0; i < 3; i += 1)
            {
                if (chunk < str.Length)
                {
                    Text.Add(str.Substring(stInd, chunk));
                    stInd = chunk;
                    chunk += 255;
                }
                else if (_else)
                {
                    Text.Add(str.Substring(stInd, str.Length - stInd));
                    _else = false;
                }
                else
                {
                    Text.Add("");
                }
            }
        }

        internal static bool ReplaceWord(Dictionary<string, string> items, string PathSave, DateTime DateSession, Students students, FileInfo _fileInfo)
        {
            Word.Application app = null;
            try
            {
                app = new Word.Application();
                Object file = _fileInfo.FullName;
                Object missing = Type.Missing;

                app.Documents.Open(file);

                foreach (var item in items)
                {
                    Word.Find find = app.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;
                    Object wrap = Word.WdFindWrap.wdFindContinue;
                    Object replace = Word.WdReplace.wdReplaceAll;
                    FindReplaceWord(wrap, missing, replace, find);
                }

                PathSave = System.IO.Path.Combine(PathSave, DateSession.ToString("dd.MM.yyyy") + " " + students.NameFile + " " + students.StudGroup + ".docx");
                app.ActiveDocument.SaveAs2(PathSave);


                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                if (app != null)
                {
                    app.ActiveDocument.Close();
                    app.Quit();
                }
            }
        }

        public static void FindReplaceWord(Object wrap, Object missing, Object replace, Word.Find find)
        {
            find.Execute(FindText: Type.Missing,
                        MatchCase: true,
                        MatchWholeWord: true,
                        MatchWildcards: false,
                        MatchSoundsLike: false,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: missing,
                        Replace: replace);
        }

    }
}
