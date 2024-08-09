using System;
using System.Collections;
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
     //<summary>
     //limits.xaml etkileşim mantığı
     //</summary>
    public partial class limits : Window
    {
        public limits()
        {
            InitializeComponent();
            showdata10();
            showdata11();
        }
       private void showdata10()
        {
            SqlConnection connect = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
            connect.Open();
            SqlCommand commandshow = new SqlCommand($"Select [ID] , [StudentNumber] , [email],[Limit] from dbo.firstTable where Rank = 3 ", connect);
            SqlDataAdapter da = new SqlDataAdapter(commandshow);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg7.ItemsSource = dt.DefaultView;
            connect.Close();
        }
        private void showdata11()
        {
            SqlConnection connect = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
            connect.Open();
            SqlCommand commandshow = new SqlCommand($"Select ID,Owned_Books,Email,StudentNumber,rentedTime,timeLimit,remainingTime from dbo.ThirdTable where timeLimit IS NOT NULL and showInReturnlist = 1", connect);
            SqlDataAdapter da = new SqlDataAdapter(commandshow);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg8.ItemsSource = dt.DefaultView;
            connect.Close();
        }
        private void btnBookLimit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IList rows = dg7.SelectedItems;
                DataRowView row = (DataRowView)dg7.SelectedItems[0];
                int id = int.Parse(row["ID"].ToString());
                SqlConnection connect = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
                connect.Open();
                SqlCommand commandshow = new SqlCommand($"update dbo.firstTable set Limit = Limit + 1 where ID = @ID", connect);
                commandshow.Parameters.AddWithValue("@ID", id);
                commandshow.ExecuteNonQuery();
                connect.Close();
                showdata10();
            }
            catch (Exception)
            {
                MessageBox.Show("Select a user");
                return;
            }
        }
        private void btnTimeLimit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IList rows = dg8.SelectedItems;
                DataRowView row = (DataRowView)dg8  .SelectedItems[0];
                int id = int.Parse(row["ID"].ToString());
                SqlConnection connect = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
                connect.Open();
                SqlCommand commandshow = new SqlCommand($"update dbo.ThirdTable set timeLimit = timeLimit + 1 where ID = @ID", connect);
                commandshow.Parameters.AddWithValue("@ID", id);
                commandshow.ExecuteNonQuery();
                connect.Close();
                showdata11();
            }
            catch (Exception)
            {
                MessageBox.Show("Please select book");
                return;
            }
        }
    }
}
