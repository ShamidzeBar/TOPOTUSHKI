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
        List<Product> SelectedProducts = new List<Product>();
        private Order currentOrder = new Order();
        private OrderProduct currentOrderProduct = new OrderProduct();

        public OrderWindow(List<Product> selectedproducts, User user)
        {
            InitializeComponent();
            PickupPoint_ComboBox.ItemsSource = TradeEntities.GetContext().PickupPoint.ToList();
            ClientName.Text = user.UserSurname + " " + user.UserName+ " " + user.UserPatronymic;

            OrderListView.ItemsSource = selectedproducts;

        }
    }
}
