using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace Kütüphane_Sistemi
{
    /// <summary>
    /// OwnedBooks.xaml etkileşim mantığı
    /// </summary>
    public partial class OwnedBooks : Window
    {
        public OwnedBooks()
        {
            InitializeComponent();
            showdata11();
        }
        private void showdata11()
        {
            SqlConnection connect = new SqlConnection("Data Source =.; Initial Catalog = Library; Integrated Security = True");
            connect.Open();
            SqlCommand commandshow = new SqlCommand("Select *from dbo.ThirdTable where showinreturnlist = 1 ", connect);
            SqlDataAdapter da = new SqlDataAdapter(commandshow);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg9.ItemsSource = dt.DefaultView;
            connect.Close();
        }
    }
}
