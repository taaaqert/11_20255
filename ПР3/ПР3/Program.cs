using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ПР3
{
    class Program
    {
        static void Main(string[] args)
        {
            // List<string> для хранения записей журнала (как требует задание)
            List<string> logEntries = new List<string>();

            // Формат строки даты/времени для записи (строго по требованию)
            const string DateTimeFormat = "yyyy-MM-dd HH:mm";

            // Главный цикл меню: продолжаем работать, пока пользователь не выберет "Выход"
            while (true)
            {
                // Выводим пункты меню пользователю
                Console.WriteLine();
                Console.WriteLine("Меню журнала событий безопасности:");
                Console.WriteLine("1. Зарегистрировать успешный вход пользователя");
                Console.WriteLine("2. Зарегистрировать неудачную попытку входа");
                Console.WriteLine("3. Вывести все записи журнала");
                Console.WriteLine("4. Выход");
                Console.Write("Выберите пункт (1-4): ");

                // Считываем ввод и пытаемся преобразовать его в целое число
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    // Если ввод не число — сообщаем и возвращаемся в начало меню
                    Console.WriteLine("Некорректный ввод. Введите число от 1 до 4.");
                    continue;
                }

                // Обработка выбора меню
                switch (choice)
                {
                    case 1:
                        // Пункт 1: регистрация успешного входа
                        RegisterEvent(logEntries, DateTimeFormat, success: true);
                        break;

                    case 2:
                        // Пункт 2: регистрация неудачной попытки входа
                        RegisterEvent(logEntries, DateTimeFormat, success: false);
                        break;

                    case 3:
                        // Пункт 3: вывод всех записей журнала
                        PrintAllEntries(logEntries);
                        break;

                    case 4:
                        // Пункт 4: выход — завершаем программу
                        Console.WriteLine("Выход. До свидания!");
                        return;

                    default:
                        // Если введено число вне диапазона 1-4
                        Console.WriteLine("Пожалуйста, выберите пункт от 1 до 4.");
                        break;
                }
            }
        }

        /// <summary>
        /// Универсальный метод записи события (успех/неудача) в журнал.
        /// </summary>
        /// <param name="logEntries">Ссылка на список, куда добавляются строки журнала.</param>
        /// <param name="dateTimeFormat">Формат для DateTime.Now.ToString(...).</param>
        /// <param name="success">true — успешный вход, false — неудача.</param>
        private static void RegisterEvent(List<string> logEntries, string dateTimeFormat, bool success)
        {
            // Запрашиваем имя пользователя
            Console.Write("Введите имя пользователя: ");
            string? rawName = Console.ReadLine();

            // Если пользователь ввёл пустую строку или только пробелы, используем "unknown"
            string user = string.IsNullOrWhiteSpace(rawName) ? "unknown" : rawName!.Trim();

            // Получаем текущую дату и время в требуемом формате
            // Используем DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            string timestamp = DateTime.Now.ToString(dateTimeFormat);

            // Формируем текст записи в требуемом формате:
            // [yyyy-MM-dd HH:mm] Попытка входа: пользователь {имя} — успех/неудача
            string status = success ? "успех" : "неудача";
            string entry = $"[{timestamp}] Попытка входа: пользователь {user} — {status}";

            // Добавляем запись в список (List<string>)
            logEntries.Add(entry);

            // Информируем пользователя, что запись добавлена
            Console.WriteLine("Запись добавлена в журнал.");
        }

        /// <summary>
        /// Выводит все записи журнала на консоль. Если записей нет — сообщает, что журнал пуст.
        /// </summary>
        /// <param name="logEntries">Список записей журнала.</param>
        private static void PrintAllEntries(List<string> logEntries)
        {
            Console.WriteLine(); // пустая строка для читаемости
            if (logEntries.Count == 0)
            {
                // Если список пуст — информируем пользователя
                Console.WriteLine("Журнал пуст.");
                return;
            }

            // Иначе последовательно выводим все записи в порядке добавления
            Console.WriteLine("Записи журнала:");
            foreach (string entry in logEntries)
            {
                Console.WriteLine(entry);
            }

            // Дополнительно выводим количество записей
            Console.WriteLine($"\nВсего записей: {logEntries.Count}");
        }
    }
}
