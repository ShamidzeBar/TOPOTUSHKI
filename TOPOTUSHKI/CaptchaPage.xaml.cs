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
    /// Логика взаимодействия для CaptchaPage.xaml
    /// </summary>
    public partial class CaptchaPage : Page
    {
        public CaptchaPage()
        {
            InitializeComponent();

        }

        private BitmapImage MakeCaptcha(int width, int height)
        {
            BitmapImage captcha = new BitmapImage();
            



            return captcha;
        }
    }
}
