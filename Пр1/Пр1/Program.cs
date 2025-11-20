using System;

namespace Пр1
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1) Заданные правильные значения логина и пароля (хранятся в коде)
            const string correctLogin = "nikita";
            const string correctPassword = "1111";

            // 2) Ограничение по числу попыток (не более 3)
            const int maxAttempts = 3;
            int attemptsLeft = maxAttempts; // переменная, отслеживающая оставшиеся попытки

            // Сообщение о начале работы программы
            Console.WriteLine("Проверка логина и пароля. У вас есть 3 попытки.\n");

            // 3) Цикл while: будет повторяться, пока есть попытки
            //    Можно использовать for, но здесь применён while для читаемости.
            while (attemptsLeft > 0)
            {
                // Запрашиваем логин
                Console.Write("Логин: ");
                string inputLogin = Console.ReadLine() ?? string.Empty;

                // Запрашиваем пароль.
                // Обычно в консоли пароль видно; здесь для простоты используем ReadLine.
                // Если хотите скрыть ввод пароля — ниже есть пример метода ReadPassword().
                Console.Write("Пароль: ");
                string inputPassword = Console.ReadLine() ?? string.Empty;

                // Сравниваем введённые данные с корректными
                if (inputLogin == correctLogin && inputPassword == correctPassword)
                {
                    // Если совпадают — доступ разрешён, выводим сообщение и выходим из программы
                    Console.WriteLine("\nДоступ разрешён.");
                    return; // завершаем программу успешно
                }
                else
                {
                    // Если данные неверные — уменьшаем количество оставшихся попыток
                    attemptsLeft--;

                    // Если попытки ещё остались — информируем пользователя о неверных данных и сколько осталось попыток
                    if (attemptsLeft > 0)
                    {
                        Console.WriteLine($"\nНеверные данные. Осталось попыток: {attemptsLeft}\n");
                    }
                    else
                    {
                        // Если попытки закончились (attemptsLeft == 0) — выводим сообщение и завершаем
                        Console.WriteLine("\nНеверные данные. Доступ запрещён.");
                        // При желании можно вернуть код ошибки (например, Environment.ExitCode = 1;)
                        return;
                    }
                }
            }
        }
    }
}