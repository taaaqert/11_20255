using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; 

namespace prform1
{
    public partial class Form1 : Form
    {
        // Имя файла, в котором будут храниться задачи
        private const string DataFileName = "tasks.txt";

        // Полный путь к файлу 
        private string DataFilePath => Path.Combine(Application.StartupPath, DataFileName);

        // Список для хранения данных
        private List<string> taskList = new List<string>();

        public Form1()
        {
            InitializeComponent();

            // 1. Загрузка данных при старте приложения
            LoadData();

            // Привязка обработчика закрытия формы для сохранения
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        /// Сохраняет текущий список задач в файл.
        private void SaveData()
        {
            try
            {
                // Записываем все строки из taskList в файл
                File.WriteAllLines(DataFilePath, taskList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// Загружает список задач из файла и заполняет ListView.
   
        private void LoadData()
        {
            if (File.Exists(DataFilePath))
            {
                try
                {
                    // Читаем все строки из файла
                    string[] loadedTasks = File.ReadAllLines(DataFilePath);

                    // Очищаем текущий список и добавляем загруженные элементы
                    taskList.Clear();
                    taskList.AddRange(loadedTasks);

                    // Заполняем ListView
                    PopulateListView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    
        /// Обновляет содержимое ListView на основе taskList.
    
        private void PopulateListView()
        {
            listViewData.Items.Clear();
            foreach (string task in taskList)
            {
                listViewData.Items.Add(new ListViewItem(task));
            }
        }

        // Обработчик события закрытия формы 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }

        // Кнопка Добавить
        private void addButton_Click(object sender, EventArgs e)
        {
            string text = textInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Поле ввода задачи пусто!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Добавляем в список
            taskList.Add(text);

            // Добавляем в ListView
            ListViewItem listItem = new ListViewItem(text);
            listViewData.Items.Add(listItem);

            // Очищаем поле ввода и устанавливаем фокус
            textInput.Clear();
            textInput.Focus();
        }

        // Удаление элемента по двойному клику
        private void listViewData_MouseDoubleClick(object sender, EventArgs e)
        {
            if (listViewData.SelectedItems.Count == 0)
            {
                return;
            }

            ListViewItem selectedItem = listViewData.SelectedItems[0];
            int selectedIndex = listViewData.SelectedIndices[0];

            string confirmationMessage = $"Вы действительно хотите удалить задачу: \"{selectedItem.Text}\"?";

            if (MessageBox.Show(confirmationMessage, "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Удаляем из внутреннего списка taskList
                if (selectedIndex >= 0 && selectedIndex < taskList.Count)
                {
                    taskList.RemoveAt(selectedIndex);
                }

                // Удаляем из ListView 
                listViewData.Items.Remove(selectedItem);
            }
        }

        // Кнопка Выход
        private void exitButton_Click(object sender, EventArgs e)
        {
            // Вызов this.Close() автоматически вызовет обработчик Form1_FormClosing,
            // который сохранит данные.
            this.Close();
        }
    }
}
