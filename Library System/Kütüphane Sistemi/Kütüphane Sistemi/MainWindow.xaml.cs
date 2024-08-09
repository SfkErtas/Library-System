using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Kütüphane_Sistemi
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            initComboBox();
            tabstudents.IsEnabled = false;
            tabteachers.IsEnabled = false;
            tabadmin.IsEnabled = false;
            tabstudents.Visibility = Visibility.Hidden;
            tabadmin.Visibility = Visibility.Hidden;
            tabteachers.Visibility = Visibility.Hidden;
        }
        SqlConnection connect = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
        public static string username = "";
        private void dataregister()
        {
            string quary = "";
            if (txtid.Text.Contains(" "))
            {
                MessageBox.Show("You can not use space button in your id!");
                return;
            }
            if (txtpass.Password.Contains(" "))
            {
                MessageBox.Show("You can not use space button in your password!");
                return;
            }
            if (txtid.Text.Contains("@"))
            {
                quary = "Insert into dbo.firstTable (email,Password,Rank,WantedRank) values(@number,@password,@Rank,@wantedrank)";
            }
            else if (txtid.Text.All(char.IsDigit))
            {
                if (txtid.Text.Length > 9)
                {
                    MessageBox.Show("your id can't be bigger than 9!");
                    return;
                }
                else
                {
                    quary = "Insert into dbo.firstTable (StudentNumber,Password,Rank,WantedRank) values(@number,@password,@Rank,@wantedrank)";
                }
            }
            else
            {
                MessageBox.Show("your id can't have letters!");
                return;
            }
            SqlCommand register = new SqlCommand(quary, connect);
            SqlCommand control = new SqlCommand("Select StudentNumber,email from dbo.firstTable", connect);
            connect.Open();
            SqlDataReader reader = control.ExecuteReader();
            while (reader.Read())
            {
                if (txtid.Text.Trim() == reader["StudentNumber"].ToString().Trim())
                {
                    MessageBox.Show("There are same Student Number in Database!");
                    connect.Close();
                    return;
                }
                else if(txtid.Text.Trim() == reader["email"].ToString().Trim())
                {
                    MessageBox.Show("There are same email in Database!");
                    connect.Close();
                    return;
                }
            }
            int defaultrank = 3;
            register.Parameters.AddWithValue("@number", txtid.Text.Trim());
            register.Parameters.AddWithValue("@password", sha256(txtpass.Password.Trim()));
            register.Parameters.AddWithValue("@Rank", defaultrank);
            register.Parameters.AddWithValue("@wantedrank", (int)cmbrank.SelectedValue);
            reader.Close();
            register.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("register succesful");
            username = txtid.Text.Trim();
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            dataregister();
        }
        private void datalogin()
        {
            string loginquary = "";
            if (txtidlogin.Text.Contains("@"))
            {
                loginquary = "Select email,Password  from dbo.firstTable where email = @number and Password = @password ";
            }
            else
            {
                loginquary = "Select StudentNumber,Password  from dbo.firstTable where StudentNumber = @number and Password = @password ";
            }
            SqlConnection connect = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
            connect.Open();
            SqlCommand login = new SqlCommand(loginquary, connect);
            login.Parameters.AddWithValue("@number", txtidlogin.Text.Trim());
            login.Parameters.AddWithValue("@password", sha256(txtpasslogin.Password.Trim()));
            SqlDataAdapter loginadapter = new SqlDataAdapter(login);
            DataTable tablelogin = new DataTable();
            loginadapter.Fill(tablelogin);
            if (tablelogin.Rows.Count == 1)
            {
                MessageBox.Show("login succesful");
                username = txtidlogin.Text.Trim();
                connect.Close();
                string quary = "";
                if (txtidlogin.Text.Trim().Contains("@"))
                {
                    quary = "Select Rank from dbo.FirstTable Where email = @number";
                }
                else
                {
                    quary = "Select Rank from dbo.FirstTable Where StudentNumber = @number";
                }
                SqlConnection connect2 = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
                connect.Open();
                SqlCommand rank = new SqlCommand(quary, connect);
                rank.Parameters.AddWithValue("@number", txtidlogin.Text.Trim());
                SqlDataAdapter rankDA = new SqlDataAdapter(rank);
                DataTable rankDT = new DataTable();
                rankDA.Fill(rankDT);
                DataRow[] rankDR = rankDT.Select();
                int RankNumber;
                string stRanknumber = "";
                foreach (DataRow row in rankDR)
                {
                    stRanknumber = row["Rank"].ToString();
                }
                RankNumber = int.Parse(stRanknumber);
                if (RankNumber == 1)
                {
                    tabadmin.Visibility = Visibility.Visible;
                    tabadmin.IsSelected = true;
                    tabadmin.IsEnabled = true;
                    RegisterAndLoginZone.IsEnabled = false;
                    btnLogOut.Visibility = Visibility.Visible;
                }
                else if (RankNumber == 2)
                {
                    tabteachers.Visibility = Visibility.Visible;
                    tabteachers.IsSelected = true;
                    tabteachers.IsEnabled = true;
                    RegisterAndLoginZone.IsEnabled = false;
                    btnLogOut.Visibility = Visibility.Visible;
                }
                else if (RankNumber == 3)
                {
                    tabstudents.Visibility = Visibility.Visible;
                    tabstudents.IsSelected = true;
                    tabstudents.IsEnabled = true;
                    RegisterAndLoginZone.IsEnabled = false;
                    btnLogOut.Visibility = Visibility.Visible;
                    notification();
                }
            }
            else
            {
                MessageBox.Show("login failed");
            }
        }
        private void notification()
        {
            SqlConnection connect = new SqlConnection("Data Source =.; Initial Catalog = Library; Integrated Security = True");
            SqlCommand command = new SqlCommand($"Select * from dbo.ThirdTable where email = '{username}' or studentnumber = '{username}' and timeLimit < 0 and showInReturnlist = 1",connect);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dtTable = new DataTable();
            da.Fill(dtTable);
            foreach (DataRow dtRow in dtTable.Rows)
            {
                string field1 = dtRow["Owned_Books"].ToString();
                MessageBox.Show("You are late for returning " + field1.Trim() + " Please return it.");
            }
        }
        public class csUserRanks
        {
            public int irRank { get; set; }
            public string srRankTitle { get; set; }
        }
        public void initComboBox()
        {
            List<csUserRanks> lstUsers = new List<csUserRanks>
            {
                new csUserRanks { irRank = -1, srRankTitle = "Select Rank" },
                //new csUserRanks { irRank = 1, srRankTitle = "Admin" },
                new csUserRanks { irRank = 2, srRankTitle = "Teacher" },
                new csUserRanks { irRank = 3, srRankTitle = "Student" }
            };
            cmbrank.ItemsSource = lstUsers;
            cmbrank.DisplayMemberPath = "srRankTitle";
            cmbrank.SelectedValuePath = "irRank";
            cmbrank.SelectedIndex = 0;
        }
        static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            datalogin();
            showdata();
            user.Content = txtidlogin.Text.Trim().ToString();
            userTeacher.Content = txtidlogin.Text.Trim().ToString();
        }
        public void showdata()
        {
            connect.Open();
            SqlCommand commandshow = new SqlCommand("Select *from dbo.SecondTable ", connect);
            SqlDataAdapter da = new SqlDataAdapter(commandshow);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg1.ItemsSource = dt.DefaultView;
            dg3.ItemsSource = dt.DefaultView;
            dg4.ItemsSource = dt.DefaultView;
            connect.Close();
        }
        private void booktaking_Click(object sender, RoutedEventArgs e)
        {
            string book = "";
            int quantity;
            try
            {
                IList rows = dg1.SelectedItems;
                DataRowView row = (DataRowView)dg1.SelectedItems[0];
                book = row["Book"].ToString();
                quantity = Convert.ToInt32(row["quantity"]);
            }
            catch (Exception)
            {
                MessageBox.Show("Select a book to take");
                return;
            }
            if (quantity == 0)
            {
                MessageBox.Show("The book you have chosen is not present in the library!");
                return;
            }
            else
            {
                string quary = "Select * from firstTable where [StudentNumber] =  @username or [email] = @username ";
                SqlConnection connect = new SqlConnection("Data Source =.; Initial Catalog = Library; Integrated Security = True");
                SqlCommand comm = new SqlCommand(quary, connect);
                comm.Parameters.AddWithValue("@username", username);
                SqlDataAdapter dataadapter = new SqlDataAdapter(comm);
                DataTable datatable = new DataTable();
                dataadapter.Fill(datatable);
                DataRow[] row1 = datatable.Select();
                string currentLimit = "0";
                foreach (DataRow item in row1)
                {
                    currentLimit = (item["Limit"].ToString().Trim());
                }
                if (int.Parse(currentLimit.Trim()) == 0)
                {
                    MessageBox.Show("You can't take book anymore!");
                    return;
                }
                SqlConnection update = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
                string quary2 = "update dbo.SecondTable set quantity = quantity -1 where Book = @book update dbo.firstTable set Limit = Limit -1 where StudentNumber = @username ";
                SqlCommand updatecommand = new SqlCommand(quary2, update);
                updatecommand.Parameters.AddWithValue("@book", book);
                updatecommand.Parameters.AddWithValue("@username", username);
                update.Open();
                updatecommand.ExecuteNonQuery();
                update.Close();
                MessageBox.Show("Book successfully taken!");
                SqlConnection writeowned = new SqlConnection("Data Source =.; Initial Catalog = Library; Integrated Security = True");
                string quary3 = "";
                if (username.Contains("@"))
                {
                    quary3 = $"INSERT INTO dbo.ThirdTable (Owned_Books, Email,rentedTime,timeLimit) VALUES ('{book}','{username}',getdate(),7)";
                }
                else if (username.All(char.IsDigit))
                {
                    quary3 = $"INSERT INTO dbo.ThirdTable (Owned_Books, StudentNumber,rentedTime,timeLimit) VALUES ('{book}','{username}',getdate(),7)";
                }
                SqlCommand write = new SqlCommand(quary3, writeowned);
                writeowned.Open();
                write.ExecuteNonQuery();
                writeowned.Close();
                showdata();
            }
        }
        private void searchbutton_Click(object sender, RoutedEventArgs e)
        {
            searching();
        }
        private void btnownedbooks_Click(object sender, RoutedEventArgs e)
        {
            Books newform = new Books();
            newform.ShowDialog();
        }
        private void btnNot_Click(object sender, RoutedEventArgs e)
        {
            notification();
        }
        private void chckFilter_Checked(object sender, RoutedEventArgs e)
        {
            searching();
        }
        private void chckQuick_Checked(object sender, RoutedEventArgs e)
        {
            searching();
        }
        private void chckFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            searching();
        }
        private void chckQuick_Unchecked(object sender, RoutedEventArgs e)
        {
            searching();
        }
        public void searching()
        {
            string searchquary = "";
            if (chckFilter.IsChecked == true && chckQuick.IsChecked == true)
            {
                searchquary = "Select * from dbo.SecondTable where Book like @SEARCH + '%'  AND quantity != 0";
            }
            else if (chckFilter.IsChecked == false && chckQuick.IsChecked == true)
            {
                searchquary = "Select * from dbo.SecondTable where Book like @SEARCH + '%'  ";
            }
            else if (chckFilter.IsChecked == true && chckQuick.IsChecked == false)
            {
                searchquary = "Select * from dbo.SecondTable where Book like '%' + @SEARCH + '%' AND quantity != 0";
            }
            else if (chckFilter.IsChecked == false && chckQuick.IsChecked == false)
            {
                searchquary = "Select * from dbo.SecondTable where Book like '%' + @SEARCH + '%' ";
            }
            SqlConnection connect = new SqlConnection("Data Source =.; Initial Catalog = Library; Integrated Security = True");
            SqlCommand cmd = new SqlCommand(searchquary, connect);
            cmd.Parameters.AddWithValue("@search", txtsearch.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg1.ItemsSource = dt.DefaultView;
        }
        private void bookTakingTeachers_Click(object sender, RoutedEventArgs e)
        {
            string book = "";
            int quantity;
            try
            {
                IList rows = dg3.SelectedItems;
                DataRowView row = (DataRowView)dg3.SelectedItems[0];
                book = row["Book"].ToString();
                quantity = Convert.ToInt32(row["quantity"]);
            }
            catch (Exception)
            {
                MessageBox.Show("Select a book to take");
                return;
            }
            if (quantity == 0)
            {
                MessageBox.Show("This book has no stock!");
                return;
            }
            else
            {
                SqlConnection update = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
                string quary2 = "update dbo.SecondTable set quantity = quantity -1 where Book = @book ";
                SqlCommand updatecommand = new SqlCommand(quary2, update);
                updatecommand.Parameters.AddWithValue("@book", book);
                update.Open();
                updatecommand.ExecuteNonQuery();
                update.Close();
                MessageBox.Show("Book successfully taken!");
                SqlConnection writeowned = new SqlConnection("Data Source =.; Initial Catalog = Library; Integrated Security = True");
                string quary3 = "";
                if (username.Contains("@"))
                {
                    quary3 = $"INSERT INTO dbo.ThirdTable (Owned_Books, Email , rentedTime) VALUES ('{book}','{username}',getdate())";
                }
                else if (username.All(char.IsDigit))
                {
                    quary3 = $"INSERT INTO dbo.ThirdTable (Owned_Books, StudentNumber,rentedTime) VALUES ('{book}','{username}',getdate())";
                }
                SqlCommand write = new SqlCommand(quary3, writeowned);
                writeowned.Open();
                write.ExecuteNonQuery();
                writeowned.Close();
                showdata();
            }
        }
        public void searchingForTeacher()
        {
            string searchquary = "";
            if (chckTeacherFilter.IsChecked == true && chckTeacherQuick.IsChecked == true)
            {
                searchquary = "Select * from dbo.SecondTable where Book like @SEARCH + '%'  AND quantity != 0";
            }
            else if (chckTeacherFilter.IsChecked == false && chckTeacherQuick.IsChecked == true)
            {
                searchquary = "Select * from dbo.SecondTable where Book like  @SEARCH + '%'  ";
            }
            else if (chckTeacherFilter.IsChecked == true && chckTeacherQuick.IsChecked == false)
            {
                searchquary = "Select * from dbo.SecondTable where Book like '%' + @SEARCH + '%' AND quantity != 0";
            }
            else if (chckTeacherFilter.IsChecked == false && chckTeacherQuick.IsChecked == false)
            {
                searchquary = "Select * from dbo.SecondTable where Book like '%' + @SEARCH + '%' ";
            }
            SqlConnection connect = new SqlConnection("Data Source =.; Initial Catalog = Library; Integrated Security = True");
            SqlCommand cmd = new SqlCommand(searchquary, connect);
            cmd.Parameters.AddWithValue("@search", txtTeacherSearch.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg3.ItemsSource = dt.DefaultView;
        }
        private void btnTeacherSearch_Click(object sender, RoutedEventArgs e)
        {
            searchingForTeacher();
        }
        private void chckTeacherFilter_Checked(object sender, RoutedEventArgs e)
        {
            searchingForTeacher();
        }
        private void chckTeacherQuick_Checked(object sender, RoutedEventArgs e)
        {
            searchingForTeacher();
        }
        private void chckTeacherQuick_Unchecked(object sender, RoutedEventArgs e)
        {
            searchingForTeacher();
        }
        private void chckTeacherFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            searchingForTeacher();
        }
        private void OwnedTeacherBook_Click(object sender, RoutedEventArgs e)
        {
            Books newform = new Books();
            newform.ShowDialog();
        }
        private void btnAddBooks_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection update = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
            string newquary = "insert into dbo.SecondTable (Book,Page,Category,Writer,BookYear,quantity) values (@Book,@Page,@Category,@Writer,@BookYear,@quantity)";
            SqlCommand updatecommand = new SqlCommand(newquary, update);
            updatecommand.Parameters.AddWithValue("@Book", txtBookName.Text);
            updatecommand.Parameters.AddWithValue("@Page", txtBookPage.Text);
            updatecommand.Parameters.AddWithValue("@Category", txtBookCategory.Text);
            updatecommand.Parameters.AddWithValue("@Writer", txtBookWriter.Text);
            updatecommand.Parameters.AddWithValue("@BookYear", txtBookYear.Text);
            updatecommand.Parameters.AddWithValue("@quantity", txtBookQuantity.Text);
            try
            {
                update.Open();
                updatecommand.ExecuteNonQuery();
                update.Close();
                showdata();
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.ToString());
                return;
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string book = "";
            IList rows = dg4.SelectedItems;
            DataRowView row = (DataRowView)dg4.SelectedItems[0];
            book = row["Book"].ToString();
            SqlConnection update = new SqlConnection("Data Source=.;Initial Catalog=Library;Integrated Security=True");
            string deletequary = "delete from dbo.SecondTable where Book = @book ";
            SqlCommand updatecommand = new SqlCommand(deletequary, update);
            updatecommand.Parameters.AddWithValue("@book", book);
            update.Open();
            updatecommand.ExecuteNonQuery();
            update.Close();
            showdata();
        }
        private void txtBookPage_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex acceptingNumberOnly = new Regex("[^0-9]+");
            e.Handled = acceptingNumberOnly.IsMatch(e.Text);
        }
        private void txtBookYear_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex acceptingNumberOnly = new Regex("[^0-9]+");
            e.Handled = acceptingNumberOnly.IsMatch(e.Text);
        }
        private void txtBookQuantity_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex acceptingNumberOnly = new Regex("[^0-9]+");
            e.Handled = acceptingNumberOnly.IsMatch(e.Text);
        }
        private void btnTeacherApprovals_Click(object sender, RoutedEventArgs e)
        {
            TeacherApproval newform = new TeacherApproval();
            newform.ShowDialog();
        }
        private void btnBookApproval_Click(object sender, RoutedEventArgs e)
        {
            BookApproval newform = new BookApproval();
            newform.ShowDialog();
        }
        private void btnLimitChange_Click(object sender, RoutedEventArgs e)
        {
            limits newform = new limits();
            newform.ShowDialog();
        }
        private void btnShowRentedBooks_Click(object sender, RoutedEventArgs e)
        {
            OwnedBooks newform = new OwnedBooks();
            newform.ShowDialog();
        }
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            RegisterAndLoginZone.IsEnabled = true;
            RegisterAndLoginZone.Focus();
            tabstudents.IsEnabled = false;
            tabteachers.IsEnabled = false;
            tabadmin.IsEnabled = false;
            tabstudents.Visibility = Visibility.Hidden;
            tabteachers.Visibility = Visibility.Hidden;
            tabadmin.Visibility = Visibility.Hidden;
            btnLogOut.Visibility = Visibility.Hidden;
            username = "";
            txtid.Clear();
            txtidlogin.Clear();
            txtpass.Clear();
            txtpasslogin.Clear();
            cmbrank.Text = "Select Rank";
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            showdata();
        }
    }
}