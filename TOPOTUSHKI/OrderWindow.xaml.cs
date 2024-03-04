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
using System.Windows.Shapes;

namespace TOPOTUSHKI
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        List<OrderProduct> SelectedOrderProducts = new List<OrderProduct>();
        private Order currentOrder = new Order();
        private OrderProduct currentOrderProduct = new OrderProduct();

        List<Product> SelectedProducts = new List<Product>();
        List<Product> DBProducts;

        public OrderWindow(List<Product> selectedproducts, User user)
        {
            InitializeComponent();
            DBProducts = TradeEntities.GetContext().Product.ToList();
            PickupPoint_ComboBox.ItemsSource = TradeEntities.GetContext().PickupPoint.ToList();
            ClientName.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            OrderListView.ItemsSource = selectedproducts;
            SelectedProducts = selectedproducts;
            OrderDate.Text = DateTime.Now.ToString();
            SetDeliveryTime();
        }

        private void SetDeliveryTime()
        {
            bool fastDelivery = true;
            foreach (var selectedproduct in SelectedProducts)
            {
                foreach (var DBproduct in DBProducts)
                {
                    if(selectedproduct.ProductArticleNumber == DBproduct.ProductArticleNumber)
                    {
                        if (DBproduct.ProductQuantityInStock < 4)
                            fastDelivery = false;
                    }
                }
            }

            if(fastDelivery)
            {
                DeliveryDate.Text = DateTime.Now.AddDays(3).ToString();
            }
            else
            {
                DeliveryDate.Text = DateTime.Now.AddDays(6).ToString();
            }
        }

        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            Product product = (sender as Button).DataContext as Product;

            foreach (var selectedproduct in SelectedProducts)
            {
                if (product.ProductArticleNumber == selectedproduct.ProductArticleNumber)
                {
                    selectedproduct.ProductQuantityInStock++;
                }
            }
            OrderListView.Items.Refresh();
            SetDeliveryTime();
        }

        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            Product product = (sender as Button).DataContext as Product;

            foreach (var selectedproduct in SelectedProducts)
            {
                if (product.ProductArticleNumber == selectedproduct.ProductArticleNumber)
                {
                    if (selectedproduct.ProductQuantityInStock > 0)
                        selectedproduct.ProductQuantityInStock--;
                }
            }
            OrderListView.Items.Refresh();
            SetDeliveryTime();
        }

        private void SaveOrderBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
