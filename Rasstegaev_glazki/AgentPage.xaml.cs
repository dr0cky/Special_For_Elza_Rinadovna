using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GubaidullinGlazki
{
    public partial class AgentPage : Page
    {
        private int CountRecords;
        private int CountPage;
        private int CurrentPage;
        private readonly List<Agent> CurrentPageList = new();
        private List<Agent> TableList;

        public AgentPage()
        {
            InitializeComponent();
            var currentAgents = Rasstegaev_glazkiEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgents;
            SortCombo.SelectedIndex = 0;
            FilterCombo.SelectedIndex = 0;
            AgentListView.SelectedItems.Clear();
            UpdateAgents();
            EditButton.Visibility = Visibility.Hidden;
        }

        private void ChangePage(int direction, int? selectedPage)
            {
                CurrentPageList.Clear();
                CountRecords = TableList.Count;
                if (CountRecords % 10 > 0)
                    CountPage = CountRecords / 10 + 1;
                else
                    CountPage = CountRecords / 10;
                var ifUpdate = true;
                int min;
                if (selectedPage.HasValue)
                {
                    if (selectedPage >= 0 && selectedPage <= CountPage)
                    {
                        CurrentPage = (int)selectedPage;
                        min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                        for (var i = CurrentPage * 10; i < min; i++) CurrentPageList.Add(TableList[i]);
                    }
                }
                else
                {
                    switch (direction)
                    {
                        case 1:
                            if (CurrentPage > 0)
                            {
                                CurrentPage--;
                                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                                for (var i = CurrentPage * 10; i < min; i++) CurrentPageList.Add(TableList[i]);
                            }
                            else
                            {
                                ifUpdate = false;
                            }

                            break;
                        case 2:
                            if (CurrentPage < CountPage - 1)
                            {
                                CurrentPage++;
                                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                                for (var i = CurrentPage * 10; i < min; i++) CurrentPageList.Add(TableList[i]);
                            }
                            else
                            {
                                ifUpdate = false;
                            }

                            break;
                    }
                }

                if (ifUpdate)
                {
                    PageListBox.Items.Clear();
                    for (var i = 1; i <= CountPage; i++) PageListBox.Items.Add(i);
                    PageListBox.SelectedIndex = CurrentPage;

                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    TBCount.Text = min.ToString();
                    TBAllRecords.Text = " из " + CountRecords;

                    AgentListView.ItemsSource = CurrentPageList;
                    AgentListView.Items.Refresh();
                }
            }

            private void UpdateAgents()
            {
                var currentAgent = Rasstegaev_glazkiEntities.GetContext().Agent.ToList();

                if (SortCombo.SelectedIndex == 1) currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
                if (SortCombo.SelectedIndex == 2) currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
                if (SortCombo.SelectedIndex == 3) currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();
                if (SortCombo.SelectedIndex == 4) currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();

                currentAgent = currentAgent.Where(p => PhoneFormat(p.Phone.ToLower()).Contains(TBoxSearch.Text.ToLower())
                                                       || p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())
                                                       || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

                if (FilterCombo.SelectedIndex == 0) 
                    currentAgent = currentAgent;
                if (FilterCombo.SelectedIndex == 1)
                    currentAgent = currentAgent.Where(p => p.AgentTypeString == "МФО").ToList();
                if (FilterCombo.SelectedIndex == 2)
                    currentAgent = currentAgent.Where(p => p.AgentTypeString == "ЗАО").ToList();
                if (FilterCombo.SelectedIndex == 3)
                    currentAgent = currentAgent.Where(p => p.AgentTypeString == "МКК").ToList();
                if (FilterCombo.SelectedIndex == 4)
                    currentAgent = currentAgent.Where(p => p.AgentTypeString == "ОАО").ToList();
                if (FilterCombo.SelectedIndex == 5)
                    currentAgent = currentAgent.Where(p => p.AgentTypeString == "ПАО").ToList();
                if (FilterCombo.SelectedIndex == 6)
                    currentAgent = currentAgent.Where(p => p.AgentTypeString == "ООО").ToList();


                AgentListView.ItemsSource = currentAgent;
                TableList = currentAgent;
                ChangePage(0, 0);
            }

            private string PhoneFormat(string phone)
            {
                return phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            }

            private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
            {
                UpdateAgents();
            }

            private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                UpdateAgents();
            }

            private void ComboAgentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                UpdateAgents();
            }

            private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                UpdateAgents();
            }

            private void TBoxSearch_TextChanged_1(object sender, TextChangedEventArgs e)
            {
                UpdateAgents();
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

            private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
            {
                Manager.MainFrame.Navigate(new AddEditPage((sender as Button)?.DataContext as Agent));
            }

            private void AddBtn_OnClick(object sender, RoutedEventArgs e)
            {
                Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
            }

            private void AgentPage_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                if (Visibility == Visibility.Visible)
                {
                Rasstegaev_glazkiEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    AgentListView.ItemsSource = Rasstegaev_glazkiEntities.GetContext().Agent.ToList();
                }

                UpdateAgents();
            }

            private void EditPriority_OnClick(object sender, RoutedEventArgs e)
            {
                var p = (AgentListView.SelectedItems.Cast<Agent>().Select(selectedItem => selectedItem.Priority)).Prepend(0).Max();
                var window = new EditPriorityWindow(p);
                window.ShowDialog();
                if (string.IsNullOrEmpty(window.Priority.Text))
                {
                    return;
                }

                foreach (Agent selectedItem in AgentListView.SelectedItems)
                {
                    selectedItem.Priority = Convert.ToInt32(window.Priority.Text);
                }

                try
                {
                Rasstegaev_glazkiEntities.GetContext().SaveChanges();
                    window.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                UpdateAgents();
            }

            private void AgentListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                EditButton.Visibility = AgentListView.SelectedItems.Count > 1 ? Visibility.Visible : Visibility.Hidden;
            }
    }
}