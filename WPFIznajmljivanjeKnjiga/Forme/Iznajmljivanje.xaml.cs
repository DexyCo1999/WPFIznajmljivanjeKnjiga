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


namespace WPFIznajmljivanjeKnjiga.Forme
{
    /// <summary>
    /// Interaction logic for Iznajmljivanje.xaml
    /// </summary>
    public partial class Iznajmljivanje : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public Iznajmljivanje()
        {
            InitializeComponent();

            try
            {
                konekcija.Open();

                string vratiKnjige = @"Select KnjigaID, NazivKnjige from tblKnjiga";
                DataTable dtKnjige = new DataTable();
                SqlDataAdapter daKnjige = new SqlDataAdapter(vratiKnjige, konekcija);
                daKnjige.Fill(dtKnjige);
                cbxKnjiga.ItemsSource = dtKnjige.DefaultView;

                string vratiKorisnike = @"Select KorisnikID, PrezimeKorisnika from tblKorisnik";
                DataTable dtKorisnik = new DataTable();
                SqlDataAdapter daKorisnik = new SqlDataAdapter(vratiKorisnike, konekcija);
                daKorisnik.Fill(dtKorisnik);
                cbxKorisnik.ItemsSource = dtKorisnik.DefaultView;
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                if (MainWindow.azuziraj)
                {
                    DataRowView red = (DataRowView)MainWindow.selektovan;
                    

                    string upit = @"Update tblIznajmljivanje
                        Set DatumIznajmiljivanja ='" + dpDatumIznajmiljivanja.SelectedDate + 
                        "', DatumVracanja='" + dpDatumVracanja.SelectedDate +
                        "', Kazna =" + Convert.ToInt32(cbKazna.IsChecked) +
                        ", IznosKazne=" + txtIznosKazne.Text + 
                        ", KnjigaID=" + cbxKnjiga.SelectedValue +
                        ", KorisnikID=" + cbxKorisnik.SelectedValue +
                        " Where IznajmljivanjeID=" + red["ID"];
                    SqlCommand komanda = new SqlCommand(upit, konekcija);
                    komanda.ExecuteNonQuery();
                    MainWindow.selektovan = null;
                    this.Close();
                }
                else
                {                   

                    string insert = @"insert into tblIznajmljivanje (DatumIznajmiljivanja, DatumVracanja,Kazna, IznosKazne, KnjigaID, KorisnikID)
                        values('" + dpDatumIznajmiljivanja.SelectedDate +
                        "', '" + dpDatumVracanja.SelectedDate +
                        "',"+ Convert.ToInt32(cbKazna.IsChecked) +
                            "," + txtIznosKazne.Text +
                            ", " + cbxKnjiga.SelectedValue +
                            ", " + cbxKorisnik.SelectedValue + ");";

                    SqlCommand cmd = new SqlCommand(insert, konekcija);
                cmd.ExecuteNonQuery();
                this.Close();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos određenih podataka nije validan! \n Popuniti sva polja!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Odaberite datum!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }

        }

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}



