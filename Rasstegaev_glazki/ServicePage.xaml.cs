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
    /// 
    public partial class ServicePage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;

        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
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
            TableList = currentServices;
            ChangePage(0, 0);
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

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear(); CountRecords = TableList.Count;
            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }
            Boolean Ifupdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage; min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--; min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++; min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;
                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();
                ServiceListView.ItemsSource = CurrentPageList; ServiceListView.Items.Refresh();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Rasstegaev_glazkiEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ServiceListView.ItemsSource = Rasstegaev_glazkiEntities.GetContext().Agent.ToList();
            }
            UpdateServices();
        }
    }
}
