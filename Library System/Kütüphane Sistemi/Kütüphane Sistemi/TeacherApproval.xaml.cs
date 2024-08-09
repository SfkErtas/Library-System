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
    /// TeacherApproval.xaml etkileşim mantığı
    /// </summary>
    public partial class TeacherApproval : Window
    {
        public TeacherApproval()
        {
            InitializeComponent();
            showdata();
        }
        SqlConnection connect = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IList rows = dg5.SelectedItems;
                DataRowView row = (DataRowView)dg5.SelectedItems[0];
                int ID = int.Parse(row["ID"].ToString());
                SqlConnection update = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
                string approvalcommand = "update dbo.firstTable set Rank = 2 where WantedRank = 2 and ID = @ID ";
                SqlCommand updatecommand = new SqlCommand(approvalcommand, update);
                updatecommand.Parameters.AddWithValue("@ID", ID);
                update.Open();
                updatecommand.ExecuteNonQuery();
                update.Close();
                showdata();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Select User to approve his rank.");
            }
        }
        private void showdata()
        {
            connect.Open();
            SqlCommand commandshow = new SqlCommand("Select ID,StudentNumber,email,WantedRank from dbo.firstTable where WantedRank = 2 and Rank = 3 ", connect);
            SqlDataAdapter da = new SqlDataAdapter(commandshow);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg5.ItemsSource = dt.DefaultView;
            connect.Close();
        }
        private void btnReturnAdmin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
