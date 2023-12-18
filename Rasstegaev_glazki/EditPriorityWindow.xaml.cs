using System.Windows;

namespace GubaidullinGlazki
{
    public partial class EditPriorityWindow : Window
    {
        private Agent _currentAgent = new();
        public EditPriorityWindow(int p)
        {
            InitializeComponent();
            Priority.Text = p.ToString();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Priority.Text, out var s))
            {
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка! Введите правильный приоритет");
            }
        }
    }
}