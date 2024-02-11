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
        public TopotushkiPage()
        {
            InitializeComponent();

            UpdateTopoPage();
        }

        private void UpdateTopoPage()
        {
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
    }
}
