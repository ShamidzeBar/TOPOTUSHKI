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
        List<Product> SelectedProducts = new List<Product>();
        List<Product> DBProducts;
        User user;

        public OrderWindow(List<Product> selectedproducts, User user)
        {
            InitializeComponent();
            DBProducts = TradeEntities.GetContext().Product.ToList();
            OrderListView.ItemsSource = selectedproducts;
            SelectedProducts = selectedproducts;
            PickupPoint_ComboBox.ItemsSource = TradeEntities.GetContext().PickupPoint.ToList();

            this.user = user;
            ClientName.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            OrderDate.Text = DateTime.Now.ToString();
            OrderID.Text = (TradeEntities.GetContext().Order.ToList().Last().OrderID + 1).ToString();

            SetDeliveryTime();
        }

        private DateTime SetDeliveryTime()
        {
            bool fastDelivery = true;
            foreach (var selectedproduct in SelectedProducts)
            {
                foreach (var DBproduct in DBProducts)
                {
                    if (selectedproduct.ProductArticleNumber == DBproduct.ProductArticleNumber)
                    {
                        if (DBproduct.ProductQuantityInStock < 4)
                            fastDelivery = false;
                    }
                }
            }

            if (fastDelivery)
            {
                DeliveryDate.Text = DateTime.Now.AddDays(3).ToString();
                return DateTime.Now.AddDays(3);
            }
            else
            {
                DeliveryDate.Text = DateTime.Now.AddDays(6).ToString();
                return DateTime.Now.AddDays(6);
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
            if (PickupPoint_ComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите пункт выдачи заказа!");
                return;
            }

            SelectedProducts = SelectedProducts.FindAll(product => product.ProductQuantityInStock > 0);

            List<OrderProduct> orderProducts = new List<OrderProduct>();
            Order NewOrder = new Order();
            int OrderID = TradeEntities.GetContext().Order.ToList().Count + 1;

            NewOrder.IDClient = user.UserID;
            NewOrder.OrderDeliveryDate = SetDeliveryTime();
            NewOrder.OrderDate = DateTime.Now;
            NewOrder.PickupPoint = TradeEntities.GetContext().PickupPoint.ToList()[PickupPoint_ComboBox.SelectedIndex];
            NewOrder.OrderReceiveCode = (910 + OrderID).ToString();
            NewOrder.OrderStatus = "Новый";

            TradeEntities.GetContext().Order.Add(NewOrder);
            TradeEntities.GetContext().SaveChanges();

            foreach (var selectedproduct in SelectedProducts)
            {
                OrderProduct orderProduct = new OrderProduct();
                orderProduct.ProductArticleNumber = selectedproduct.ProductArticleNumber;
                orderProduct.Amount = selectedproduct.ProductQuantityInStock;
                orderProduct.OrderID = TradeEntities.GetContext().Order.ToList().Last().OrderID;
                TradeEntities.GetContext().OrderProduct.Add(orderProduct);
                TradeEntities.GetContext().SaveChanges();
            }

            MessageBox.Show("Заказ успешно сформирован!");
            SelectedProducts.Clear();
            this.Close();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            Product product = (sender as Button).DataContext as Product;

            SelectedProducts.Remove(SelectedProducts.Find(selpro => selpro.ProductArticleNumber == product.ProductArticleNumber));

            OrderListView.Items.Refresh();
        }
    }
}
