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
using System.Data.SqlClient;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ZaladujGrid();
        }

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-8GAO498\SQLEXPRESS;Initial Catalog=NewDB;Integrated Security=True");

        public void wyczyscDane()
        {
            nazwa_txt.Clear();
            adres_txt.Clear();
            nip_txt.Clear();
            email_txt.Clear();
            szukaj_txt.Clear();
        }

        public bool isValid()
        {
            if (nazwa_txt.Text == string.Empty)
            {
                MessageBox.Show("Nazwa jest pusta", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (adres_txt.Text == string.Empty)
            {
                MessageBox.Show("Adres jest pusty", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (nip_txt.Text == string.Empty)
            {
                MessageBox.Show("Nip jest pusty", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (email_txt.Text == string.Empty)
            {
                MessageBox.Show("Email jest pusty", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public void ZaladujGrid()
        {
            SqlCommand cmd = new SqlCommand("select * from FirstTable", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;

        }
private void DodajBtn_txt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO FirstTable VALUES (@Nazwa, @Adres, @Nip, @Email)", con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Nazwa", nazwa_txt.Text);
                    cmd.Parameters.AddWithValue("@Adres", adres_txt.Text);
                    cmd.Parameters.AddWithValue("@Nip", nip_txt.Text);
                    cmd.Parameters.AddWithValue("@Email", email_txt.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ZaladujGrid();
                    MessageBox.Show("Klienta dodano pomyślnie", "Zapisano", MessageBoxButton.OK, MessageBoxImage.Information);
                    wyczyscDane();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    
    private void WyczyscBtn_txt_Click(object sender, RoutedEventArgs e)
        {
            wyczyscDane();
        }

        private void UsunBtn_txt_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from FirstTable where ID = "+szukaj_txt.Text+ " ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Klient został usunięty", "Usunięto", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                wyczyscDane();
                ZaladujGrid();
                con.Close();
            }
            catch (SqlException ex)
            {

                MessageBox.Show("Nie usunięto" +ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void ZmienBtn_txt_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update FirstTABLE set Nazwa = '" + nazwa_txt.Text + "', Adres = '" + adres_txt.Text + "', Nip = '" + nip_txt.Text + "', Email = '" + email_txt.Text + "' WHERE ID = '"+szukaj_txt.Text+"' ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Dane klienta zostały zaktualizowane", "Zaktualizowane", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                wyczyscDane();
                ZaladujGrid();
            }
        }
    }
}
