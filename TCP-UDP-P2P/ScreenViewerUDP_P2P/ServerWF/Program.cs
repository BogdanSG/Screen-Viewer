using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ServerWF {
    static class Program {

        private static Form1 ServerForm;

        public const string SV_Directory = "ScreenServerFiles";

        static Color green = Color.FromArgb(30, 223, 56);

        public static Color Green { get => green; }

        static Color yellow = Color.FromArgb(229, 236, 37);

        public static Color Yellow { get => yellow; }

        static Color magenta = Color.FromArgb(226, 45, 246);

        public static Color Magenta { get => magenta; }

        static Color red = Color.FromArgb(238, 24, 24);

        public static Color Red { get => red; }

        static Color orange = Color.FromArgb(255, 213, 0);

        public static Color Orange { get => orange; }


        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main() {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ServerForm = new Form1();
            Application.Run(ServerForm);

        }//Main

        public static void SetTitle(string Title) {

            ServerForm.Title = Title;

        }//SetTitle

        public static string GetTitle() {

            return ServerForm.Title;

        }//GetTitle

        public static string GetFirstTitle() {

            return ServerForm.oldTitle;

        }//GetFirstTitle

        public static void ConsoleWrite(string Text) {

            ServerForm.Console.Write(Text);

            var CP = ServerForm.Console.GetCursorPosition();

            ServerForm.Console.SetCursorPosition(CP.Row, 0);

        }//ConsoleWrite

        public static void ConsoleWriteLine(string Text) {

            ConsoleWrite(Text + "\n");

        }//ConsoleWrite

        public static void ConsoleWrite(string Text, Color fgColor, Color bgColor) {

            ServerForm.Console.Write(Text, fgColor, bgColor);

            var CP = ServerForm.Console.GetCursorPosition();

            ServerForm.Console.SetCursorPosition(CP.Row, 0);

        }//ConsoleWrite

        public static void ConsoleWriteLine(string Text, Color fgColor, Color bgColor) {

            ConsoleWrite(Text + "\n", fgColor, bgColor);

        }//ConsoleWrite

    }//Program

}//ServerWF
