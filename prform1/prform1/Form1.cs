using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prform1
{
    public partial class Form1 : Form
    {
        // текстбокс для хранения данных
        private List<string> taskList = new List<string>(); // Изменено название текстбокса

        public Form1()
        {
            InitializeComponent();
        }

        // Кнопка Добавить
        private void addButton_Click(object sender, EventArgs e) // Изменено название метода
        {
            // Получаем и очищаем введенный текст
            string text = textInput.Text.Trim();

            // Проверка на пустое поле
            if (string.IsNullOrWhiteSpace(text)) 
            {
                MessageBox.Show("Поле ввода задачи пусто!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Добавляем в текстбокс 
            taskList.Add(text);

            // Добавляем в ListView
            // Создаем новый элемент ListViewItem, чтобы добавить его в контрол
            ListViewItem listItem = new ListViewItem(text);
            listViewData.Items.Add(listItem);

            // Очищаем поле ввода и устанавливаем фокус
            textInput.Clear();
            textInput.Focus();
        }

        // Удаление элемента по двойному клику
        private void listViewData_MouseDoubleClick(object sender, EventArgs e) // Изменено название метода
        {
            // Проверка, выбран ли хоть один элемент
            if (listViewData.SelectedItems.Count == 0)
            {
                return;
            }

            // Получаем выбранный элемент и его индекс
            ListViewItem selectedItem = listViewData.SelectedItems[0];
            int selectedIndex = listViewData.SelectedIndices[0]; // Получаем индекс для удаления из taskList

            // Подтверждение удаления
            string confirmationMessage = $"Вы действительно хотите удалить задачу: \"{selectedItem.Text}\"?";

            if (MessageBox.Show(confirmationMessage, "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Удаляем из текстбокса 
                // Это более надежно, чем удаление по строковому значению, если строки могут дублироваться
                if (selectedIndex >= 0 && selectedIndex < taskList.Count)
                {
                    taskList.RemoveAt(selectedIndex);
                }

                // Удаляем из ListView 
                listViewData.Items.Remove(selectedItem);
            }
        }

        // Кнопка Выход
        private void exitButton_Click(object sender, EventArgs e) // Изменено название метода
        {
            // Используем this.Close() для закрытия текущей формы, что также завершит приложение
            this.Close();
            // Application.Exit(); // Оригинальный вариант
        }
    }
}