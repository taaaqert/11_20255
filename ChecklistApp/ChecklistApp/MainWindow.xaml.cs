using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChecklistApp
{
    // Элемент чек-листа
    public class ChecklistItem : INotifyPropertyChanged
    {
        private string _description;
        private bool _isCompleted;

        // Текст задачи
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        // Галочка выполнения
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        // Для обновления интерфейса
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Весь чек-лист
    public class Checklist
    {
        public string Title { get; set; } = "Новый чек-лист";
        public string Description { get; set; } = "";
        public ObservableCollection<ChecklistItem> Items { get; set; } = new ObservableCollection<ChecklistItem>();

        // Считаем прогресс
        public string Progress
        {
            get
            {
                if (Items.Count == 0) return "0/0 (0%)";
                int completed = Items.Count(item => item.IsCompleted);
                int percent = (completed * 100) / Items.Count;
                return $"{completed}/{Items.Count} ({percent}%)";
            }
        }
    }

    public partial class MainWindow : Window
    {
        // Все чек-листы
        private ObservableCollection<Checklist> checklists = new ObservableCollection<Checklist>();
        // Текущий выбранный
        private Checklist currentChecklist;

        public MainWindow()
        {
            InitializeComponent();
            lstChecklists.ItemsSource = checklists;
           
            CreateStudentScheduleChecklist(); // Добавляем расписание
        }

        // Расписание студента как в таблице
        private void CreateStudentScheduleChecklist()
        {
            var studentSchedule = new Checklist
            {
                Title = "Расписание студента на день",
                Description = "Ежедневный план занятий"
            };

            // Утренний блок - 75% выполнено
            studentSchedule.Items.Add(new ChecklistItem { Description = "УТРЕННИЙ БЛОК", IsCompleted = true });
            studentSchedule.Items.Add(new ChecklistItem { Description = "1.1 Подъем, душ (07:00 - 07:20)", IsCompleted = true });
            studentSchedule.Items.Add(new ChecklistItem { Description = "1.2 Завтрак (07:20 - 07:40)", IsCompleted = true });
            studentSchedule.Items.Add(new ChecklistItem { Description = "1.3 Подготовка к парам (07:40 - 08:00)", IsCompleted = true });
            studentSchedule.Items.Add(new ChecklistItem { Description = "1.4 Дорога в Колледж (08:00 - 08:30)", IsCompleted = true });

            // Учебный блок - 80% выполнено
            studentSchedule.Items.Add(new ChecklistItem { Description = "УЧЕБНЫЙ БЛОК", IsCompleted = true });
            studentSchedule.Items.Add(new ChecklistItem { Description = "2.1 Посещение пар/практики (8:30 - 11:50)", IsCompleted = true });
            studentSchedule.Items.Add(new ChecklistItem { Description = "2.2 Обеденный перерыв (11:50 – 12:40)", IsCompleted = true });
            studentSchedule.Items.Add(new ChecklistItem { Description = "2.3 Посещение пар/практики (12:45 – 16:00)", IsCompleted = true });

            // Дневной блок - 50% выполнено
            studentSchedule.Items.Add(new ChecklistItem { Description = "ДНЕВНОЙ БЛОК", IsCompleted = false });
            studentSchedule.Items.Add(new ChecklistItem { Description = "3.1 Посещение секций (16:30 – 17:30)", IsCompleted = false });
            studentSchedule.Items.Add(new ChecklistItem { Description = "3.2 Встреча с друзьями (18:00 – 20:00)", IsCompleted = true });

            // Вечерний блок - 70% выполнено
            studentSchedule.Items.Add(new ChecklistItem { Description = "ВЕЧЕРНИЙ БЛОК", IsCompleted = false });
            studentSchedule.Items.Add(new ChecklistItem { Description = "4.1 Приготовление ужина (20:00 – 21:00)", IsCompleted = true });
            studentSchedule.Items.Add(new ChecklistItem { Description = "4.2 Свободное время (21:00 – 22:00)", IsCompleted = true });
            studentSchedule.Items.Add(new ChecklistItem { Description = "4.3 Подготовка ко сну (22:00 – 23:00)", IsCompleted = false });

            checklists.Add(studentSchedule);
            lstChecklists.SelectedItem = studentSchedule; // Сразу выбираем расписание
        }

        // Создать новый чек-лист
        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            var newChecklist = new Checklist
            {
                Title = $"Чек-лист {checklists.Count + 1}"
            };

            checklists.Add(newChecklist);
            lstChecklists.SelectedItem = newChecklist;
        }

        // Удалить чек-лист
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (currentChecklist == null) return;

            checklists.Remove(currentChecklist);
            currentChecklist = null;
            ClearFields();
        }

        // Добавить задачу
        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (currentChecklist == null) return;

            var newItem = new ChecklistItem { Description = "Новый элемент" };
            currentChecklist.Items.Add(newItem);
            lstItems.SelectedItem = newItem;
            UpdateProgress();
        }

        // Удалить задачу
        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (currentChecklist == null) return;

            var selectedItem = lstItems.SelectedItem as ChecklistItem;
            if (selectedItem != null)
            {
                currentChecklist.Items.Remove(selectedItem);
                UpdateProgress();
            }
        }

        // Удалить все выполненные
        private void BtnClearCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (currentChecklist == null) return;

            var completedItems = currentChecklist.Items.Where(item => item.IsCompleted).ToList();
            if (completedItems.Count == 0)
            {
                MessageBox.Show("Нет выполненных элементов", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            foreach (var item in completedItems)
            {
                currentChecklist.Items.Remove(item);
            }
            UpdateProgress();
        }

        // Выбрали чек-лист
        private void LstChecklists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentChecklist = lstChecklists.SelectedItem as Checklist;

            if (currentChecklist != null)
            {
                txtTitle.Text = currentChecklist.Title;
                txtDescription.Text = currentChecklist.Description;
                lstItems.ItemsSource = currentChecklist.Items;
                UpdateProgress();
            }
            else
            {
                ClearFields();
            }
        }

        // Выбрали задачу
        private void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = lstItems.SelectedItem as ChecklistItem;
            if (selectedItem != null)
            {
                txtItemText.Text = selectedItem.Description;
            }
            else
            {
                txtItemText.Text = "";
            }
        }

        // Поставили галочку
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProgress();
        }

        // Убрали галочку
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateProgress();
        }

        // Изменили название
        private void TxtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentChecklist != null)
            {
                currentChecklist.Title = txtTitle.Text;
            }
        }

        // Изменили описание
        private void TxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentChecklist != null)
            {
                currentChecklist.Description = txtDescription.Text;
            }
        }

        // Изменили текст задачи
        private void TxtItemText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var selectedItem = lstItems.SelectedItem as ChecklistItem;
            if (selectedItem != null)
            {
                selectedItem.Description = txtItemText.Text;
            }
        }

        // Обновить прогресс
        private void UpdateProgress()
        {
            if (currentChecklist != null)
            {
                txtProgress.Text = $"Прогресс: {currentChecklist.Progress}";
            }
            else
            {
                txtProgress.Text = "Прогресс: 0/0 (0%)";
            }
        }

        // Очистить все поля
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