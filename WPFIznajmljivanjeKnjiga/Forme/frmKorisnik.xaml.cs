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
    /// Interaction logic for frmKorisnik.xaml
    /// </summary>
    public partial class frmKorisnik : Window
    {
        public SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmKorisnik()
        {
            InitializeComponent();
        }
        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                if (MainWindow.azuziraj)
                {
                    DataRowView red = (DataRowView)MainWindow.selektovan;

                    string upit = @"Update tblKorisnik
                            Set ImeKorisnika='" + txtImeKorisnika.Text + "', PrezimeKorisnika='" + txtPrezimeKorisnika.Text + "', JMBGKorisnika='" + txtJMBGKorisnika.Text + "', AdresaKorisnika='" + txtAdresaKorisnika.Text + "', GradKorisnika='" + txtGradKorisnika.Text + "', KontaktKorisnika='" + txtKontaktKorisnika.Text + "' Where KorisnikID=" + red["ID"];

                    SqlCommand komanda = new SqlCommand(upit, konekcija);
                    komanda.ExecuteNonQuery();
                    MainWindow.selektovan = null;
                    this.Close();
                }
                else
                { 
                string insert = @"insert into tblKorisnik values('" + txtImeKorisnika.Text + "','"
                    + txtPrezimeKorisnika.Text + "','" + txtJMBGKorisnika.Text + "','"
                    + txtAdresaKorisnika.Text + "','" + txtGradKorisnika.Text + "','" + txtKontaktKorisnika.Text + "');";
                SqlCommand cmd = new SqlCommand(insert, konekcija);
                cmd.ExecuteNonQuery();
                this.Close();
                }

            }
            catch (SqlException)
            {
                MessageBox.Show("Unos određenih podataka nije validan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

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
