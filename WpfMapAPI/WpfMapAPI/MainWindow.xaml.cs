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

namespace WpfMapAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchTest(tbox_query.Text);
            
        }

        async void SearchTest(string query)
        {
            try
            {
                var mls = await KakaoAPI.Search(query);
                lbox_locale.ItemsSource = mls;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error While KakaoAPI.Search\r\n" + ex.ToString());
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbox_locale.SelectedIndex == -1)
            {
                return;
            }

            var ml = lbox_locale.SelectedItem as MyLocale;
            if (ml == null)
            {
                return;
            }

            object[] ps = new object[] { ml.Lat, ml.Lng };
            try
            {
                // https://apis.map.kakao.com/web/guide/#step2
                wb.InvokeScript("setCenter", ps);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error While InvokeScript\r\n" + ex.ToString());
            }
        }
    }
}
