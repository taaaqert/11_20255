using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChecklistApp
{
    public class ChecklistItem : INotifyPropertyChanged
    {
        private string _description;
        private bool _isCompleted;

        /// Текст описания элемента
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                // Уведомляем интерфейс об изменении свойства
                OnPropertyChanged(nameof(Description));
            }
        }

        ///галочка
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                // Уведомляем интерфейс об изменении свойства
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        /// Событие для уведомления об изменении свойств
        public event PropertyChangedEventHandler PropertyChanged;

        /// Метод для вызова события PropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// Класс, представляющий чек-лист
    public class Checklist
    {
        /// Название чек-листа
        public string Title { get; set; } = "Новый чек-лист";

        /// Описание чек-листа
        public string Description { get; set; } = "";

        /// Коллекция элементов чек-листа
        public ObservableCollection<ChecklistItem> Items { get; set; } = new ObservableCollection<ChecklistItem>();

        /// Прогресс выполнения чек-листа
       
        public string Progress
        {
            get
            {
                // Если нет элементов возвращаем 0%
                if (Items.Count == 0) return "0/0 (0%)";

                // Считаем количество выполненных элементов
                int completed = Items.Count(item => item.IsCompleted);

                // Вычисляем процент выполнения
                int percent = (completed * 100) / Items.Count;

                // Возвращаем строку прогресса
                return $"{completed}/{Items.Count} ({percent}%)";
            }
        }
    }

    /// Главное окно приложения
    public partial class MainWindow : Window
    {
        // Коллекция всех чек-листов
        private ObservableCollection<Checklist> checklists = new ObservableCollection<Checklist>();

        // Текущий выбранный чеклист
        private Checklist currentChecklist;

        /// Конструктор главного окна
        public MainWindow()
        {
            InitializeComponent();

            // Привязываем список чек-листов к ListBox
            lstChecklists.ItemsSource = checklists;

        }
        /// Обработчик нажатия кнопки "Новый" - создает новый чек-лист
  
        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новый чек-лист 
            var newChecklist = new Checklist
            {
                Title = $"Чек-лист {checklists.Count + 1}"
            };

            // Добавляем в коллекцию и выбираем
            checklists.Add(newChecklist);
            lstChecklists.SelectedItem = newChecklist;
        }

        /// удаляет текущий чеклист
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Если нет выбранного чек-листа, выходим
            if (currentChecklist == null) return;

            // Удаляем чеклист из коллекции
            checklists.Remove(currentChecklist);

            // Сбрасываем текущий чеклист
            currentChecklist = null;

            // Очищаем поля ввода
            ClearFields();
        }

        /// Добавить новый элемент в чеклист
        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            // Если нет выбранного чеклиста, выходим
            if (currentChecklist == null) return;

            // Создаем новый элемент с текстом по умолчанию
            var newItem = new ChecklistItem { Description = "Новый элемент" };

            // Добавляем элемент в текущий чеклист
            currentChecklist.Items.Add(newItem);

            // Выбираем новый элемент в списке
            lstItems.SelectedItem = newItem;

            // Обновляем отображение прогресса
            UpdateProgress();
        }

        /// Удалить выбранный элемент из чеклиста
        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            // Если нет выбранного чеклиста, выходим
            if (currentChecklist == null) return;

            // Получаем выбранный элемент
            var selectedItem = lstItems.SelectedItem as ChecklistItem;

            // Если элемент выбран, удаляем его
            if (selectedItem != null)
            {
                currentChecklist.Items.Remove(selectedItem);
                UpdateProgress();
            }
        }

        /// удаляет все выполненные элементы
        private void BtnClearCompleted_Click(object sender, RoutedEventArgs e)
        {
            // Если нет выбранного чек-листа, выходим
            if (currentChecklist == null) return;

            // Получаем список всех выполненных элементов
            var completedItems = currentChecklist.Items.Where(item => item.IsCompleted).ToList();

            // Если нет выполненных элементов, показываем сообщение
            if (completedItems.Count == 0)
            {
                MessageBox.Show("Нет выполненных элементов для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Удаляем все выполненные элементы
            foreach (var item in completedItems)
            {
                currentChecklist.Items.Remove(item);
            }

            // Обновляем прогресс
            UpdateProgress();
        }

        /// Обработчик изменения выбора в списке чек листов
        private void LstChecklists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранный чек-лист
            currentChecklist = lstChecklists.SelectedItem as Checklist;

            // Если чек-лист выбран
            if (currentChecklist != null)
            {
                // Заполняем поля данными выбранного чек-листа
                txtTitle.Text = currentChecklist.Title;
                txtDescription.Text = currentChecklist.Description;

                // Привязываем список элементов к ListBox
                lstItems.ItemsSource = currentChecklist.Items;

                // Обновляем отображение прогресса
                UpdateProgress();
            }
            else
            {
                // Если чек-лист не выбран, очищаем поля
                ClearFields();
            }
        }

        /// Обработчик изменения выбора в списке элементов
      
        private void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранный элемент
            var selectedItem = lstItems.SelectedItem as ChecklistItem;

            // Если элемент выбран, показываем его текст в поле редактирования
            if (selectedItem != null)
            {
                txtItemText.Text = selectedItem.Description;
            }
            else
            {
                // Если элемент не выбран, очищаем поле
                txtItemText.Text = "";
            }
        }

        /// Обработчик установки галочки
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Обновляем прогресс при установке галочки
            UpdateProgress();
        }

        /// Обработчик снятия галочки
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Обновляем прогресс при снятии галочки
            UpdateProgress();
        }

        /// Обработчик изменения текста в поле названия чек-листа

        private void TxtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Если есть текущий чек-лист, обновляем его название
            if (currentChecklist != null)
            {
                currentChecklist.Title = txtTitle.Text;
            }
        }

        /// Обработчик изменения текста в поле описания чек-листа
        private void TxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Если есть текущий чек-лист, обновляем его описание
            if (currentChecklist != null)
            {
                currentChecklist.Description = txtDescription.Text;
            }
        }

        /// Обработчик изменения текста в поле элемента чек-листа
        private void TxtItemText_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получаем выбранный элемент
            var selectedItem = lstItems.SelectedItem as ChecklistItem;

            // Если элемент выбран, обновляем его описание
            if (selectedItem != null)
            {
                selectedItem.Description = txtItemText.Text;
            }
        }

        /// Обновляет отображение прогресса выполнения текущего чек-листа
        private void UpdateProgress()
        {
            // показываем его прогресс
            if (currentChecklist != null)
            {
                txtProgress.Text = $"Прогресс: {currentChecklist.Progress}";
            }
            else
            {
                // показываем нулевой прогресс
                txtProgress.Text = "Прогресс: 0/0 (0%)";
            }
        }

        /// Очищает все поля ввода и отображения
        private void ClearFields()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtItemText.Text = "";
            lstItems.ItemsSource = null;
            txtProgress.Text = "Прогресс: 0/0 (0%)";
        }
    }
}