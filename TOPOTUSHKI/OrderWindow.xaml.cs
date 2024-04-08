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
        List<OrderedProducts> SelectedProducts = new List<OrderedProducts>();
        List<OrderedProducts> RefSelectedProducts;
        List<Product> DBProducts;
        User user;

        public OrderWindow(ref List<OrderedProducts> selectedproducts, User user)
        {
            InitializeComponent();
            RefSelectedProducts = selectedproducts;
            SelectedProducts = new List<OrderedProducts>(selectedproducts);
            OrderListView.ItemsSource = SelectedProducts;
            PickupPoint_ComboBox.ItemsSource = TradeEntities.GetContext().PickupPoint.ToList();

            DBProducts = TradeEntities.GetContext().Product.ToList();


            this.user = user;
            if (user != null)
                ClientName.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            else
                ClientName.Text = "Гость";
            OrderDate.Text = DateTime.Now.ToString();
            OrderID.Text = (TradeEntities.GetContext().Order.ToList().Last().OrderID + 1).ToString();

            SetDeliveryTime();
        }

        private DateTime SetDeliveryTime()
        {
            Update();
            bool fastDelivery = true;

            foreach (var selectedproduct in SelectedProducts)
            {
                foreach (var DBproduct in DBProducts)
                {
                    if (selectedproduct.ArticleNumber == DBproduct.ProductArticleNumber)
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
            OrderedProducts product = (sender as Button).DataContext as OrderedProducts;

           
            if (DBProducts.Find(p => p.ProductArticleNumber == product.ArticleNumber).ProductQuantityInStock == 0)
            {

                DBProducts.Find(p => p.ProductArticleNumber == product.ArticleNumber).ProductQuantityInStock--;

                foreach (var selectedproduct in SelectedProducts)
                {
                    if (product.ArticleNumber == selectedproduct.ArticleNumber)
                    {
                        selectedproduct.OrderedCount++;
                    }
                }

                OrderListView.Items.Refresh();
                SetDeliveryTime();
            }
            else
            {
                MessageBox.Show("Товар закончился на складе!");
            }
        }

        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {

            OrderedProducts product = (sender as Button).DataContext as OrderedProducts;
            if (product.OrderedCount != 0)
            {
                DBProducts.Find(p => p.ProductArticleNumber == product.ArticleNumber).ProductQuantityInStock++;

                foreach (var selectedproduct in SelectedProducts)
                {
                    if (product.ArticleNumber == selectedproduct.ArticleNumber)
                    {
                        if (selectedproduct.OrderedCount > 0)
                            selectedproduct.OrderedCount--;
                    }
                }
                OrderListView.Items.Refresh();
                SetDeliveryTime();
            }
        }

        void Update()
        {
            int sum = 0;
            foreach (var product in SelectedProducts)
            {
                sum += Convert.ToInt32(product.ProductCost * product.OrderedCount);
            }

            FinalPrice.Text = sum.ToString();
        }

        private void SaveOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PickupPoint_ComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите пункт выдачи заказа!");
                return;
            }



            SelectedProducts = SelectedProducts.FindAll(product => product.OrderedCount > 0);

            if (SelectedProducts.Count == 0)
            {
                MessageBox.Show("Список заказанных товаров пуст!");
            }
            else
            {
                List<OrderProduct> orderProducts = new List<OrderProduct>();
                Order NewOrder = new Order();
                int OrderID = TradeEntities.GetContext().Order.ToList().Count + 1;

                if (user == null)
                    NewOrder.IDClient = 52;
                else
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
                    orderProduct.ProductArticleNumber = selectedproduct.ArticleNumber;
                    orderProduct.Amount = selectedproduct.OrderedCount;
                    orderProduct.OrderID = TradeEntities.GetContext().Order.ToList().Last().OrderID;
                    TradeEntities.GetContext().OrderProduct.Add(orderProduct);
                    TradeEntities.GetContext().SaveChanges();
                }

                SelectedProducts.Clear();
                MessageBox.Show("Заказ успешно сформирован!");
                this.Close();
            }

            RefSelectedProducts.Clear();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            OrderedProducts product = (sender as Button).DataContext as OrderedProducts;

            SelectedProducts.Remove(SelectedProducts.Find(selpro => selpro.ArticleNumber == product.ArticleNumber));

            OrderListView.Items.Refresh();
            SetDeliveryTime();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var somelist = SelectedProducts.FindAll(product => product.OrderedCount > 0);
            if (somelist.Count == 0)
            {
                RefSelectedProducts.Clear();
            }
            this.Close();
        }
    }
}
