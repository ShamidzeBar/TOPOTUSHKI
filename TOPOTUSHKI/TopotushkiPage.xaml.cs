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

namespace TOPOTUSHKI
{
    /// <summary>
    /// Логика взаимодействия для TopotushkiPage.xaml
    /// </summary>  
    public partial class TopotushkiPage : Page
    {
        private List<OrderedProducts> SelectedProducts;
        private User user;
        public TopotushkiPage(User init_user)
        {
            InitializeComponent();
            user = init_user;
            SelectedProducts = new List<OrderedProducts>();
            if(user == null)
                MenuItem.IsEnabled = false;
            UpdateTopoPage();
        }

        private void UpdateTopoPage()
        {
            if(user == null)
            {
                LastName_TB.Text = ("Гость");
            }
            else
            {
                LastName_TB.Text = user.UserSurname;
                FirstName_TB.Text = user.UserName;
                Patronymic_TB.Text = user.UserPatronymic;
                Role_TB.Text = "Роль: " + user.GetStringRole();
            }
            var currentShoes = TradeEntities.GetContext().Product.ToList();

            int initialcount = currentShoes.Count(),
                remaincount;

            currentShoes = currentShoes.Where(p => p.ProductName.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();

            if(OrderBy.IsChecked.Value)
            {
               currentShoes = currentShoes.OrderBy(p => p.ProductCost).ToList();
            }

            if (OrderByDescending.IsChecked.Value)
            {
                currentShoes = currentShoes.OrderByDescending(p => p.ProductCost).ToList();
            }

            if(DiscountComboBox.SelectedIndex == 1)
            {
                currentShoes = currentShoes.Where(p => p.ProductDiscountAmount > 0 && p.ProductDiscountAmount < 10).ToList();
            }
            if (DiscountComboBox.SelectedIndex == 2)
            {
                currentShoes = currentShoes.Where(p => p.ProductDiscountAmount > 10 && p.ProductDiscountAmount < 15).ToList();
            }
            if (DiscountComboBox.SelectedIndex == 3)
            {
                currentShoes = currentShoes.Where(p => p.ProductDiscountAmount > 15).ToList();
            }

            remaincount = currentShoes.Count();

            RemainCount.Text = remaincount.ToString();
            InitialCount.Text = initialcount.ToString(); 

            ShoesListView.ItemsSource = currentShoes;
        }

        private void SearchTextBox_Changed(object sender, TextChangedEventArgs e)
        {
            UpdateTopoPage();
        }

        private void OrderBy_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTopoPage();
        }

        private void OrderByDescending_Checked(object sender, RoutedEventArgs e)
        {

            UpdateTopoPage();
        }

        private void DiscountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTopoPage();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(ShoesListView.SelectedIndex >= 0)
            {
                List<Product> ProductsBD = TradeEntities.GetContext().Product.ToList();

                Product temp = ShoesListView.SelectedItem as Product;

                OrderedProducts product = OrderedProducts.ToOrderedProducts(temp);
                

                foreach(Product ProductBD in ProductsBD)
                {
                    if(ProductBD.ProductArticleNumber == product.ArticleNumber)
                    {
                        if(ProductBD.ProductQuantityInStock > 0)
                        {
                            bool inList = false;
                            foreach(OrderedProducts selProduct in SelectedProducts)
                            {
                                if(selProduct.ArticleNumber == product.ArticleNumber)
                                {
                                    MessageBox.Show("Товар уже был в списке, поэтому количество этого товара в заказе было увеличено на 1");
                                    selProduct.OrderedCount++;
                                    inList = true;
                                }
                            }
                            if (!inList)
                            {
                                product.OrderedCount = 1;
                                SelectedProducts.Add(product);
                                //(new OrderedProducts(product., 1, product.ProductDiscountAmount, product.ProductManufacturer, product.ProductDescription, product.ProductCost, product.ProductName, product.ProductPhoto));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Товара больше нет в наличии");
                        }
                    }
                }
            }
            if (SelectedProducts.Count > 0 && user.UserRole != 0)
                MakeOrderBtn.Visibility = Visibility.Visible;
            ShoesListView.SelectedIndex = -1;
        }

        private void MakeOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(ref SelectedProducts, user);
            orderWindow.Show();
        }
    }
}
