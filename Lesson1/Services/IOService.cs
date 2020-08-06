using System;

namespace Lesson1
{
    public class IOService
    {
        public IOService()
        {

        }

        public void Output(string s)
        {
            Console.WriteLine(s);
        }

        public string Input()
        {
            return Console.ReadLine();
        }
        public void Exeption(System.Exception e)
        {
            Console.WriteLine(e);
        }

        public void SkipLine()
        {
            Console.WriteLine();
        }

        public delegate void CheckInput(string message, IOService ioService);
        public event CheckInput Notify;
        public string InputForFindSth(string Sth, IOService ioService)
        {
            Console.WriteLine($"Введите название {Sth} и нажмите enter");
            var SthName = Console.ReadLine();
            DateTime date = DateTime.Now;
            Notify?.Invoke($"Пользователь ввел название {SthName} в {date}", ioService);
            return SthName;
        }
        public void ProgramMenu()
        {
            Console.WriteLine("Чтобы осуществить поиск по имени завода нажмите 1");
            Console.WriteLine("Чтобы осуществить поиск по имени установки нажмите 2");
            Console.WriteLine("Чтобы осуществить поиск по имени резервуара нажмите 3");
            Console.WriteLine("Чтобы создать файл .json и серелиазовать данные заводов нажмите 4");
            Console.WriteLine("Чтобы создать файл .json и производить с ним изменения нажмите 5");
            Console.WriteLine("Чтобы асинхронно считать данные из файла Async.json нажмите 6");
            Console.WriteLine("Чтобы осуществить поиск по имени резервуара с помощью LINQ (Query syntax) нажмите 7");
            Console.WriteLine("Чтобы осуществить поиск по имени резервуара с помощью LINQ (Method-based syntax) нажмите 8");
            Console.WriteLine("Для выхода нажмите любую другую клавишу");
            Console.WriteLine();
        }

        public void JsonMenu()
        {
            Console.WriteLine("Чтобы добавить элемент в json файл нажмите 1");
            Console.WriteLine("Чтобы изменить элемент в json файл нажмите 2");
            Console.WriteLine("Чтобы удалить элемент в json файл нажмите 3");
            Console.WriteLine("Для выхода нажмите любую другую клавишу");
            Console.WriteLine();
        }
    }
}