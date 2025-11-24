using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerMonitorApp
{
    public partial class Form1 : Form
    {

        private string[] servers = {
            "192.168.31.1",     // роутер
            "192.168.31.136",   // компьютер
            "localhost",        // Локальный хост
            "google.com"        // Внешний сервер для проверки интернета
        };

        private const int CHECK_INTERVAL = 10000; // 10 сек
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
        }

        /// Настройка таймера для периодической проверки
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = CHECK_INTERVAL;
            timer.Tick += Timer_Tick;
        }

        /// Обработчик таймера, запуск проверки серверов
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await MonitorServers();
        }

        /// Основной цикл мониторинга всех серверов
        private async Task MonitorServers()
        {
            resultsTextBox.Clear();
            resultsTextBox.AppendText($"=== Проверка {DateTime.Now:HH:mm:ss} ===\r\n\r\n");

            foreach (string server in servers)
            {
                string status = await CheckServerStatus(server);
                resultsTextBox.AppendText($"{server}: {status}\r\n");
            }
        }

        /// Проверка статуса одного сервера с обработкой ошибок
    
        private async Task<string> CheckServerStatus(string address)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    // Асинхронная ping-проверка с таймаутом  5 секунд
                    PingReply reply = await ping.SendPingAsync(address, 5000);

                    if (reply.Status == IPStatus.Success)
                        return "online server";
                    else
                        return "offline server";
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок 
                return "offline server";
            }
        }

        // Кнопка запуска мониторинга
        private void startButton_Click(object sender, EventArgs e)
        {
            timer.Start();
            startButton.Enabled = false;
            stopButton.Enabled = true;
            resultsTextBox.AppendText("Мониторинг запущен\r\n");
        }

        // Кнопка остановки мониторинга
        private void stopButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            startButton.Enabled = true;
            stopButton.Enabled = false;
            resultsTextBox.AppendText("Мониторинг остановлен\r\n");
        }

        // Кнопка принудительной проверки
        private async void checkNowButton_Click(object sender, EventArgs e)
        {
            await MonitorServers();
        }
    }
}