using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using ToastNotifications.Core;

namespace EduFlow.Cashier.Desktop.Components.GroupForComponents;

/// <summary>
/// Interaction logic for GroupForComponent.xaml
/// </summary>
public partial class GroupForComponent : UserControl
{
    private long Id { get; set; }
    public Func<Task> isClicked { get; set; }
    public GroupForComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, string name, int lessonCount, DateTime CreatedDate)
    {
        this.Id = id;
        tbName.Text = name;
        tbLessonCount.Text = lessonCount.ToString();
        tbCreatedDate.Text = CreatedDate.ToString("dd/MM/yyyy");
    }

    public long GetId()
        => this.Id;

    public void SelectedState(bool isSelected)
    {
        if (isSelected)
            mainBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B6B6B6"));
        else
            mainBorder.Background = Brushes.White;
    }

    private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        => isClicked?.Invoke();
}
