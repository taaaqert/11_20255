using System;
using System.Text;

namespace Пр2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Проверка надёжности пароля ---");
            Console.Write("Введите пароль: ");

            // Считываем введенный пользователем пароль
            string password = Console.ReadLine() ?? string.Empty;

            // Вызываем функцию проверки и получаем результат
            bool isStrong = CheckPasswordStrength(password);

            // Выводим результат пользователю
            if (isStrong)
            {
                Console.WriteLine("\nПароль надёжный");
            }
            else
            {
                Console.WriteLine("\nПароль слабый");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        /// <summary>
        /// Проверяет, соответствует ли пароль заданным критериям надёжности.
        /// </summary>
        /// <param name="password">Пароль для проверки.</param>
        /// <returns>True, если пароль надёжный, иначе False.</returns>
        static bool CheckPasswordStrength(string password)
        {
            // 1. Проверка ТРЕБОВАНИЯ 1: Длина не менее 8 символов
            if (password.Length < 8)
            {
                Console.WriteLine("    [Ошибка] Длина пароля менее 8 символов.");
                return false; // Пароль сразу слабый
            }

            // Инициализируем флаги для отслеживания наличия цифр и заглавных букв
            bool hasDigit = false;
            bool hasUpperLatin = false;

            // 2. Проверка ТРЕБОВАНИЙ 2 и 3: Цифры и заглавные латинские буквы
            // Проходим по каждому символу в пароле
            foreach (char c in password)
            {
                // Проверяем, является ли символ цифрой (используем char.IsDigit(), как требуется)
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }

                // Проверяем, является ли символ заглавной буквой (используем char.IsUpper(), как требуется).
                // Дополнительная проверка c >= 'A' && c <= 'Z' гарантирует, что это именно латинская буква,
                // а не заглавная буква из другого алфавита (например, кириллицы).
                if (char.IsUpper(c) && c >= 'A' && c <= 'Z')
                {
                    hasUpperLatin = true;
                }

                // Оптимизация: если оба требования уже выполнены, нет смысла продолжать цикл.
                if (hasDigit && hasUpperLatin)
                {
                    break;
                }
            }

            // Проверяем результат наличия цифр
            if (!hasDigit)
            {
                Console.WriteLine("    [Ошибка] Пароль не содержит ни одной цифры.");
                return false;
            }

            // Проверяем результат наличия заглавных латинских букв
            if (!hasUpperLatin)
            {
                Console.WriteLine("    [Ошибка] Пароль не содержит ни одной заглавной латинской буквы.");
                return false;
            }

            // Если все проверки пройдены (длина >= 8, есть цифра, есть заглавная буква),
            // то пароль считается надёжным.
            return true;
        }
    }
}