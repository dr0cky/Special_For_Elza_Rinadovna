using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace GubaidullinGlazki
{
    public partial class AddEditPage : Page
    {
        private Agent _currentAgent = new();
        public static List<Agent> Agents = new();

        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();
            if (SelectedAgent != null)
            {
                _currentAgent = SelectedAgent;
            }

            DataContext = _currentAgent;
        }

        private void ChangePictureBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var opf = new OpenFileDialog();
            if (opf.ShowDialog() != true) return;
            _currentAgent.Logo = opf.FileName;
            LogoImage.Source = new BitmapImage(new Uri(opf.FileName));
        }

        private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentAgent.Title))
            {
                errors.AppendLine("Укажите наименование агента");
            }
            
            if (string.IsNullOrWhiteSpace(_currentAgent.Address))
            {
                errors.AppendLine("Укажите адрес агента");
            }

            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName))
            {
                errors.AppendLine("Укажите ФИО директора");
            }

            if (ComboType.SelectedIndex == null)
            {
                errors.AppendLine("Укажите тип агента");
            }

            if (string.IsNullOrWhiteSpace(_currentAgent.Priority.ToString()))
            {
                errors.AppendLine("Укажите приоритет агента");
            }

            if (_currentAgent.Priority <= 0)
            {
                errors.AppendLine("Укажите положительный приоритет агента");
            }

            if (string.IsNullOrWhiteSpace(_currentAgent.INN))
            {
                errors.AppendLine("Укажите ИНН агента");
            }

            if (string.IsNullOrWhiteSpace(_currentAgent.KPP))
            {
                errors.AppendLine("Укажите КПП агента");
            }

            if (string.IsNullOrWhiteSpace(_currentAgent.Phone))
            {
                errors.AppendLine("Укажите телефон агента");
            }
            else
            {
                var ph = _currentAgent.Phone.Replace("(", "").Replace("-", "").Replace("+", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8')) && ph.Length != 11 ||
                    (ph[1] == '3' && ph.Length != 12))
                {
                    errors.AppendLine("Укажите правильно телефон агента");
                }

                if (string.IsNullOrWhiteSpace(_currentAgent.Email))
                {
                    errors.AppendLine("Укажите почту агента");
                }
                else if(!EmailValidator(_currentAgent.Email))
                {
                    errors.AppendLine("Почта в неправильном формате");
                }
                
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString());
                    return;
                }

                //MessageBox.Show(ComboType.SelectedIndex.ToString());
                _currentAgent.AgentTypeID = ComboType.SelectedIndex + 1;
                if (_currentAgent.ID == 0)
                {
                    Rasstegaev_glazkiEntities.GetContext().Agent.Add(_currentAgent);
                }

                try
                {
                    Rasstegaev_glazkiEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private bool EmailValidator(string email)
        {
            return email.Contains("@") && email.Contains('.');
        }
        private void DeleteBtn_OnClick(object sender, RoutedEventArgs e)
        {

            var currentAgent = (sender as Button)?.DataContext as Agent;

            var currentProducts = Rasstegaev_glazkiEntities.GetContext().ProductSale.ToList();
            currentProducts = currentProducts.Where(p => p.AgentID == currentAgent.ID).ToList();
            if (currentProducts.Count != 0)
            {
                MessageBox.Show("Невозможно выполнить удаления, так как существуют записи на эту услугу");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?",
                        "Внимание!",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    try
                    {
                        Rasstegaev_glazkiEntities.GetContext().Agent.Remove(currentAgent!);
                        Rasstegaev_glazkiEntities.GetContext().SaveChanges();
                        Manager.MainFrame.GoBack();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
            }
        }

        private void OpenSalesButton_OnClick(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new SalesPage(_currentAgent));
        }
    }
}