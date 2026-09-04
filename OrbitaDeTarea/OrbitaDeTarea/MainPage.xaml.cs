using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OrbitaDeTarea.Models;
using OrbitaDeTarea.Services;
namespace OrbitaDeTarea

{
    public partial class MainPage : ContentPage
    {
        private readonly TaskService taskService;

        private TaskItem? editingTask;

        public MainPage()
        {
            InitializeComponent();

            taskService = TaskService.Instance;

            pickerAssigneeFilter.SelectedIndex = 0;
            pickerPriorityFilter.SelectedIndex = 0;

            RefreshBoard();
        }
        // Refreshes all three task columns
        private void RefreshBoard()
        {
            string selectedAssignee =
                pickerAssigneeFilter.SelectedItem?.ToString() ?? "All";

            string selectedPriority =
                pickerPriorityFilter.SelectedItem?.ToString() ?? "All";

            var tasks = taskService.Tasks.AsEnumerable();

            // Filter by assignee
            if (selectedAssignee != "All")
            {
                tasks = tasks.Where(t =>
                    t.Assignee == selectedAssignee);
            }

            // Filter by priority
            if (selectedPriority != "All")
            {
                tasks = tasks.Where(t =>
                    t.Priority == selectedPriority);
            }

            var toDo = tasks
                .Where(t => t.Column == "To do")
                .ToList();

            var inProgress = tasks
                .Where(t => t.Column == "In progress")
                .ToList();

            var done = tasks
                .Where(t => t.Column == "Done")
                .ToList();

            toDoList.ItemsSource = toDo;
            inProgressList.ItemsSource = inProgress;
            doneList.ItemsSource = done;

            lblToDoCount.Text = $"{toDo.Count} tasks";
            lblInProgressCount.Text = $"{inProgress.Count} tasks";
            lblDoneCount.Text = $"{done.Count} tasks";
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            editingTask = null;

            formTitle.Text = "Add Task";

            txtTitle.Text = "";
            txtDescription.Text = "";

            pickerAssignee.SelectedIndex = -1;
            pickerPriority.SelectedIndex = -1;
            pickerColumn.SelectedIndex = 0;

            taskForm.IsVisible = true;
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                await DisplayAlert(
                    "Validation error",
                    "Please enter a task title.",
                    "OK");

                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                await DisplayAlert(
                    "Validation error",
                    "Please enter a task description.",
                    "OK");

                return;
            }

            if (pickerAssignee.SelectedIndex == -1)
            {
                await DisplayAlert(
                    "Validation error",
                    "Please select an assignee.",
                    "OK");

                return;
            }

            if (pickerPriority.SelectedIndex == -1)
            {
                await DisplayAlert(
                    "Validation error",
                    "Please select a priority.",
                    "OK");

                return;
            }

            if (pickerColumn.SelectedIndex == -1)
            {
                await DisplayAlert(
                    "Validation error",
                    "Please select a column.",
                    "OK");

                return;
            }

            // Update existing task
            if (editingTask != null)
            {
                editingTask.Title = txtTitle.Text;
                editingTask.Description = txtDescription.Text;
                editingTask.Assignee =
                    pickerAssignee.SelectedItem.ToString()!;
                editingTask.Priority =
                    pickerPriority.SelectedItem.ToString()!;
                editingTask.Column =
                    pickerColumn.SelectedItem.ToString()!;

                taskService.UpdateTask(editingTask);

                await DisplayAlert(
                    "Success",
                    "Task updated successfully.",
                    "OK");
            }
            else
            {
                // Create new task
                TaskItem newTask = new()
                {
                    Title = txtTitle.Text,
                    Description = txtDescription.Text,
                    Assignee =
                        pickerAssignee.SelectedItem.ToString()!,
                    Priority =
                        pickerPriority.SelectedItem.ToString()!,
                    Column =
                        pickerColumn.SelectedItem.ToString()!,
                    CreatedOn = DateTime.Now
                };

                taskService.AddTask(newTask);

                await DisplayAlert(
                    "Success",
                    "Task added successfully.",
                    "OK");
            }

            taskForm.IsVisible = false;

            RefreshBoard();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            taskForm.IsVisible = false;
        }

        private void TaskCard_EditClicked(
        object? sender,
        TaskItem task)
        {
            editingTask = task;

            formTitle.Text = "Edit Task";

            txtTitle.Text = task.Title;
            txtDescription.Text = task.Description;

            pickerAssignee.SelectedItem = task.Assignee;
            pickerPriority.SelectedItem = task.Priority;
            pickerColumn.SelectedItem = task.Column;

            taskForm.IsVisible = true;
        }

        private async void TaskCard_DeleteClicked(object sender, TaskItem task)
        {
            bool confirm = await DisplayAlert(
            "Delete task",
            $"Are you sure you want to delete '{task.Title}'?",
            "Yes",
            "No");

            if (!confirm)
                return;

            taskService.DeleteTask(task);

            RefreshBoard();
        }

        private async void TaskCard_MoveClicked(object sender, TaskItem task)
        {
            string nextColumn;

            switch (task.Column)
            {
                case "To do":
                    nextColumn = "In progress";
                    break;

                case "In progress":
                    nextColumn = "Done";
                    break;

                default:
                    nextColumn = "To do";
                    break;
            }

            taskService.MoveTask(task, nextColumn);

            await DisplayAlert(
                "Task moved",
                $"The task is now in '{nextColumn}'.",
                "OK");

            RefreshBoard();
        }
        // Updates the board when a filter changes
        private void FilterChanged(
            object? sender,
            EventArgs e)
        {
            RefreshBoard();
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            pickerAssigneeFilter.SelectedIndex = 0;
            pickerPriorityFilter.SelectedIndex = 0;

            RefreshBoard();
        }
    }
}
