using BusinessObject.Model;
using DataAccess.Model;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace SaleWPFApp
{
    /// <summary>
    /// Interaction logic for AdminProduct.xaml
    /// </summary>
    public partial class AdminProduct : Page
    {
        private readonly IProductRepository productRepository;
        public AdminProduct(IProductRepository _productRepository)
        {
            InitializeComponent();
            this.productRepository = _productRepository;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            dgData.ItemsSource = productRepository.List().ToList();
        }

        public void RefreshListView()
        {
            dgData.ItemsSource = productRepository.List().ToList();
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView? listView = sender as ListView;
            GridView? gridView = listView != null ? listView.View as GridView : null;

            var width = listView != null ? listView.ActualWidth - SystemParameters.VerticalScrollBarWidth : this.Width;

            var column1 = 0.1;
            var column2 = 0.2;
            var column3 = 0.1;
            var column4 = 0.2;
            var column5 = 0.2;
            var column6 = 0.2;

            if (gridView != null && width >= 0)
            {
                gridView.Columns[0].Width = width * column1;
                gridView.Columns[1].Width = width * column2;
                gridView.Columns[2].Width = width * column3;
                gridView.Columns[3].Width = width * column4;
                gridView.Columns[4].Width = width * column5;
                gridView.Columns[5].Width = width * column6;
            }
        }

        private void Button_OpenCreate(object sender, RoutedEventArgs e)
        {
            AdminProductCreate adminProductCreate = new AdminProductCreate(this, null, productRepository);
            adminProductCreate.Show();
        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            dgData.ItemsSource = productRepository.List().ToList();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you wan't remove product seledted?", "Remove product", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                List<Product> products = dgData.SelectedItems.Cast<Product>().ToList();
                products.ForEach(product => productRepository.Remove(product));
                dgData.ItemsSource = productRepository.List();
            }
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            int? id = !String.IsNullOrEmpty(searchById.Text) ? int.Parse(searchById.Text) : null;
            string? name = searchByName.Text;
            decimal? unitPrice = !String.IsNullOrEmpty(searchByUnitPrice.Text) ? decimal.Parse(searchByUnitPrice.Text) : null;
            int? unitsInStock = !String.IsNullOrEmpty(searchByUnitsInStock.Text) ? int.Parse(searchByUnitsInStock.Text) : null;

            ProductFilter productFilter = new ProductFilter();
            productFilter.ProductId = id;
            productFilter.ProductName = name;
            productFilter.UnitPrice = unitPrice;
            productFilter.UnitsInStock = unitsInStock;

            dgData.ItemsSource = productRepository.FindAllBy(productFilter);
        }
        private void Button_Edit(object sender, RoutedEventArgs e)
        {

            int count = dgData.SelectedItems.Count;
            if (count > 0)
            {
                List<Product> products = dgData.SelectedItems.Cast<Product>().ToList();
                products.ForEach(product =>
                {
                    AdminProductCreate productCreate = new AdminProductCreate(this, product, productRepository);
                    productCreate.Show();
                });
            }
            else
            {
                MessageBox.Show("Plase select product");
            }
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row =
                (DataGridRow)dataGrid.ItemContainerGenerator
                .ContainerFromIndex(dgData.SelectedIndex);
            try
            {
                DataGridCell RowColumn =
               dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string id = ((TextBlock)RowColumn.Content).Text;
                if (string.IsNullOrEmpty(id))
                {
                    btnEdit.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                }
                else
                {


                    btnEdit.IsEnabled = true;
                    btnDelete.IsEnabled = true;

                }
            }
            catch (Exception ex)
            {
                dgData.ItemsSource = productRepository.List().ToList();
            }
        }
    }
}