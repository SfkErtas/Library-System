using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Kütüphane_Sistemi
{
    /// <summary>
    /// BookApproval.xaml etkileşim mantığı
    /// </summary>
    public partial class BookApproval : Window
    {
        public BookApproval()
        {
            InitializeComponent();
            showdata5();
        }
        SqlConnection connect = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");

        private void btnReturnApprove_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnApproveBook_Click(object sender, RoutedEventArgs e)
        {
            string book = "";
            int ID,studentNumber;
            string email = "";
            try
            {
                IList rows = dg6.SelectedItems;
                DataRowView row = (DataRowView)dg6.SelectedItems[0];
                book = row["ReturnedBook"].ToString();
                ID = int.Parse(row["ID"].ToString());
                studentNumber = int.Parse(row["StudentNumber"].ToString());
                email = row["email"].ToString();

            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Select a book to return");
                return;
            }
            SqlConnection insert = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
            string returnquary = "";
            returnquary = "delete from dbo.FourthTable where ID = @ID Update dbo.SecondTable set quantity = quantity + 1  where [Book] = @book update dbo.firstTable set [Limit] = [Limit] + 1 where [StudentNumber] = @studentNumber or [email] = @email ";
            SqlCommand returncommand = new SqlCommand(returnquary, insert);
            returncommand.Parameters.AddWithValue("@book", book);
            returncommand.Parameters.AddWithValue("@username", MainWindow.username);
            returncommand.Parameters.AddWithValue("@ID", ID);
            returncommand.Parameters.AddWithValue("@studentNumber", studentNumber);
            returncommand.Parameters.AddWithValue("@email", email);
            insert.Open();
            returncommand.ExecuteNonQuery();
            insert.Close();
            showdata5();
        }
        private void showdata5()
        {
            connect.Open();
            SqlCommand commandshow = new SqlCommand("Select * from dbo.FourthTable ", connect);
            SqlDataAdapter da = new SqlDataAdapter(commandshow);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg6.ItemsSource = dt.DefaultView;
            connect.Close();
        }
    }
}
