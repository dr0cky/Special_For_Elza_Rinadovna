using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rasstegaev_glazki
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();
            var currentServices = Rasstegaev_glazkiEntities.GetContext().Agent.ToList();
            ServiceListView.ItemsSource = currentServices;
            TypeCombo.SelectedIndex = 0;


            UpdateServices();
        }

        private void UpdateServices()
        {
            var currentServices = Rasstegaev_glazkiEntities.GetContext().Agent.ToList();

            currentServices = currentServices.Where(p => p.Title.ToLower().Contains(TBSearch.Text.ToLower()) || p.Phone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-","").ToLower().Contains(TBSearch.Text.ToLower()) || p.Email.ToLower().Contains(TBSearch.Text.ToLower())).ToList();

            if (SortCombo.SelectedIndex == 0)
            {
                currentServices = currentServices.OrderBy(p => p.Title).ToList();
            }

            if (SortCombo.SelectedIndex == 1)
            {
                currentServices = currentServices.OrderByDescending(p => p.Title).ToList();
            }

            if (SortCombo.SelectedIndex == 2)
            {
                currentServices = currentServices.OrderBy(p => p.Priority).ToList();
            }

            if (SortCombo.SelectedIndex == 3)
            {
                currentServices = currentServices.OrderByDescending(p => p.Priority).ToList();
            }

            if (TypeCombo.SelectedIndex == 0)
            {
                currentServices = currentServices;
            }

            if (TypeCombo.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "МФО").ToList();
            }

            if (TypeCombo.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ООО").ToList();
            }
            if (TypeCombo.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ЗАО").ToList();
            }

            if (TypeCombo.SelectedIndex == 4)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "МКК").ToList();
            }
            if (TypeCombo.SelectedIndex == 5)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ОАО").ToList();
            }
            if (TypeCombo.SelectedIndex == 6)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ПАО").ToList();
            }

            ServiceListView.ItemsSource = currentServices;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void TypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }
    }
}
