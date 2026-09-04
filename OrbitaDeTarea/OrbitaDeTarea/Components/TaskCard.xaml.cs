namespace OrbitaDeTarea.Components;
using OrbitaDeTarea.Models;

public partial class TaskCard : ContentView
{
    public event EventHandler<TaskItem>? EditClicked;
    public event EventHandler<TaskItem>? DeleteClicked;
    public event EventHandler<TaskItem>? MoveClicked;
    public TaskCard()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskItem task)
        {
            EditClicked?.Invoke(this, task);
        }
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        if (BindingContext is TaskItem task)
        {
            DeleteClicked?.Invoke(this, task);
        }
    }

    private void Button_Clicked_2(object sender, EventArgs e)
    {
        if (BindingContext is TaskItem task)
        {
            MoveClicked?.Invoke(this, task);
        }
    }
}