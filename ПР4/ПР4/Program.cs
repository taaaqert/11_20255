using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ПР4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Главный цикл программы: повторять, пока пользователь не выберет выход
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Шифр Цезаря (только латинские буквы A-Z, a-z)");
                Console.WriteLine("1 — Зашифровать");
                Console.WriteLine("2 — Расшифровать");
                Console.WriteLine("3 — Выход");
                Console.Write("Выберите режим (1/2/3): ");

                string choice = Console.ReadLine()?.Trim();

                if (choice == "3")
                {
                    // Выход из программы
                    Console.WriteLine("Выход. До свидания!");
                    return;
                }

                if (choice != "1" && choice != "2")
                {
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    continue;
                }

                // Запрос текста у пользователя
                Console.Write("Введите текст: ");
                string inputText = Console.ReadLine() ?? string.Empty;

                // Запрос сдвига и валидация
                Console.Write("Сдвиг (целое число, например 3): ");
                string shiftStr = Console.ReadLine()?.Trim() ?? "";
                if (!int.TryParse(shiftStr, out int shift))
                {
                    Console.WriteLine("Некорректное значение сдвига. Операция отменена.");
                    continue;
                }

                // Если выбран режим расшифровки, инвертируем сдвиг
                if (choice == "2")
                {
                    shift = -shift;
                }

                // Преобразование текста
                string result = TransformCaesar(inputText, shift);

                Console.WriteLine();
                Console.WriteLine("Результат:");
                Console.WriteLine(result);
            }
        }

        // Функция, выполняющая шифрование/дешифрование методом Цезаря только для латинских букв
        // shift может быть положительным или отрицательным; большой по модулю сдвиг корректно обрабатывается
        static string TransformCaesar(string text, int shift)
        {
            // Нормализуем сдвиг в диапазон [0, 25] для удобства
            // Чтобы корректно работать с отрицательными сдвигами, используем ((shift % 26) + 26) % 26
            int normalizedShift = ((shift % 26) + 26) % 26;

            char[] resultChars = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (c >= 'A' && c <= 'Z')
                {
                    // Для заглавных букв: приводим букву к диапазону 0..25, сдвигаем, возвращаем в диапазон A..Z
                    int pos = c - 'A'; // позиция буквы в алфавите (0..25)
                    int newPos = (pos + normalizedShift) % 26; // применяем сдвиг по модулю 26
                    resultChars[i] = (char)('A' + newPos);
                }
                else if (c >= 'a' && c <= 'z')
                {
                    // Для строчных букв аналогично, но с базой 'a'
                    int pos = c - 'a';
                    int newPos = (pos + normalizedShift) % 26;
                    resultChars[i] = (char)('a' + newPos);
                }
                else
                {
                    // Для всех остальных символов (пробелы, знаки препинания, цифры и т.д.)
                    // оставляем символ без изменений
                    resultChars[i] = c;
                }
            }

            return new string(resultChars);
        }
    }
}