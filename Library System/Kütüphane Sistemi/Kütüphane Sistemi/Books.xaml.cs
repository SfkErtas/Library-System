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
using System.Data.SqlClient;
using System.Windows.Navigation;
using System.Data;
using System.Collections;

namespace Kütüphane_Sistemi
{
    /// <summary>
    /// Books.xaml etkileşim mantığı
    /// </summary>
    public partial class Books : Window
    {
        public Books()
        {
            InitializeComponent();
            showdata2();
        }
        SqlConnection connect = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
        private void btngoback_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void showdata2()
        {
            connect.Open();
            SqlCommand commandshow = new SqlCommand($"Select ID,Owned_Books,Email,StudentNumber,rentedTime,remainingTime from dbo.ThirdTable where Email ='{MainWindow.username}' or StudentNumber = '{MainWindow.username}' AND showInReturnlist = 1", connect);
            SqlDataAdapter da = new SqlDataAdapter(commandshow);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg2.ItemsSource = dt.DefaultView;
            connect.Close();
        }
        private void btnreturn_Click(object sender, RoutedEventArgs e)
        {
            string book = "";
            int ID;
            try
            {
                IList rows = dg2.SelectedItems;
                DataRowView row = (DataRowView)dg2.SelectedItems[0];
                book = row["Owned_Books"].ToString();
                ID = int.Parse(row["ID"].ToString());
            } 
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Select a book to return");
                return;
            }
            SqlConnection insert = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
            string returnquary = "";
            if (MainWindow.username.Contains("@"))
            {
                returnquary = "insert into dbo.FourthTable (ReturnedBook,email) values (@book,@username) update dbo.ThirdTable set showInReturnlist = 0 where Owned_Books = @book And Email = @username  And ID = @ID  ";
            }
            else
            {
                returnquary = "insert into  dbo.FourthTable (ReturnedBook, StudentNumber) values(@book, @username) update dbo.ThirdTable set showInReturnlist = 0 where Owned_Books = @book  AND  StudentNumber = @username  And ID = @ID ";
            }
            SqlCommand returncommand = new SqlCommand(returnquary, insert);
            returncommand.Parameters.AddWithValue("@book", book);
            returncommand.Parameters.AddWithValue("@username", MainWindow.username);
            returncommand.Parameters.AddWithValue("@ID", ID );
            insert.Open();
            returncommand.ExecuteNonQuery();
            insert.Close();
            showdata2();
        }
    }
 }
