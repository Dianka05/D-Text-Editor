using D_Text_Editor;

namespace D_Text_Editor
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}


//[STAThread]
//static void Main(string[] args)
//{
//    Application.EnableVisualStyles();
//    Application.SetCompatibleTextRenderingDefault(false);

//    // Создаем экземпляр Form1
//    Form1 mainForm = new Form1();

//    // Проверяем, были ли переданы аргументы командной строки
//    if (args.Length > 0)
//    {
//        // Если аргументы были переданы, открываем файл
//        MessageBox.Show("File opened via association: " + args[0]);
//        OpenFile(mainForm, args[0]);
//    }

//    // Передаем этот экземпляр в приложение
//    Application.Run(mainForm);
//}

//// Метод для открытия файла в уже существующем экземпляре Form1
//static void OpenFile(Form1 form, string filePath)
//{
//    // Вызываем метод OpenFile в форме, передавая ему путь к файлу
//    form.OpenFile(filePath);
//}