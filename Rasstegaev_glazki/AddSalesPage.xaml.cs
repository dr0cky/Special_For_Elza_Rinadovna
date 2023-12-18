using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GubaidullinGlazki
{
    public partial class AddSalesPage : Page
    {

        private ProductSale currentProductSale = new ProductSale();
        private List<Product> currentProduct;
        private Agent currentAgent = new Agent();

        public AddSalesPage(Agent agent)
        {
            InitializeComponent();
            currentAgent = agent;
            currentProduct = Rasstegaev_glazkiEntities.GetContext().Product.ToList();
            ComboProduct.ItemsSource = currentProduct;

            DataContext = currentProductSale;
        }

        private void AddProdHistoryBtn_Click(object sender, RoutedEventArgs e)
        {

            if (currentProductSale.ID == 0)
            {
                Rasstegaev_glazkiEntities.GetContext().ProductSale.Add(currentProductSale);
            }
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(ProdCount.Text))
            {
                errors.AppendLine("Укажите количество");
            }
            else
            {
                int c = Convert.ToInt32(ProdCount.Text);
                if (c < 1)
                    errors.AppendLine("Укажите количество");
            }
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату");
            if (ComboProduct.SelectedItem == null)
                errors.AppendLine("Укажите наименование продукта");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            currentProductSale.ProductID = ComboProduct.SelectedIndex + 1;
            currentProductSale.AgentID = currentAgent.ID;
            currentProductSale.ProductCount = Convert.ToInt32(ProdCount.Text);
            currentProductSale.SaleDate = Convert.ToDateTime(StartDate.Text);


            if (currentProductSale.ID == 0)
            {
                Rasstegaev_glazkiEntities.GetContext().ProductSale.Add(currentProductSale);
            }
            try
            {
                Rasstegaev_glazkiEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                throw;
            }

        }

    }
}