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
    /// Interaction logic for frmKnjiga.xaml
    /// </summary>
    public partial class frmKnjiga : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmKnjiga()
        {
            InitializeComponent();
            try
            {
                konekcija.Open();

                string vratiAutore = "Select AutorID, ImeAutora, PrezimeAutora from tblAutor";
                DataTable dtAutor = new DataTable();
                SqlDataAdapter daAutor = new SqlDataAdapter(vratiAutore, konekcija);
                daAutor.Fill(dtAutor);
                cbxAutor.ItemsSource = dtAutor.DefaultView;


                string vratiVrste = "Select VrstaID, NazivVrste from tblVrstaKnjige";
                DataTable dtVrsta = new DataTable();
                SqlDataAdapter daVrsta = new SqlDataAdapter(vratiVrste, konekcija);
                daVrsta.Fill(dtVrsta);
                cbxVrstaKnjige.ItemsSource = dtVrsta.DefaultView;
               
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
                    
                    string upit = @"Update tblKnjiga
                                    Set NazivKnjige='" + txtNazivKnjige.Text +
                                    "',Izdavalac='" + txtIzdavalac.Text +
                                    "',AutorID = " + cbxAutor.SelectedValue +
                                    ", VrstaID = " + cbxVrstaKnjige.SelectedValue +
                                
                                    " Where KnjigaID = " + red["ID"];
                    SqlCommand komanda = new SqlCommand(upit, konekcija);
                    komanda.ExecuteNonQuery();
                    MainWindow.selektovan = null;
                    this.Close();
                }
                else
                {

                    string insert = @"insert into tblKnjiga(NazivKnjige, Izdavalac, AutorID, VrstaID)
                        values('" + txtNazivKnjige.Text + "', '" + txtIzdavalac.Text + "', " + cbxAutor.SelectedValue + ", " + cbxVrstaKnjige.SelectedValue + ");";
                    
                  SqlCommand cmd = new SqlCommand(insert, konekcija);
                cmd.ExecuteNonQuery();
                this.Close();
                }

            }
            catch (SqlException)
            {
                MessageBox.Show("Unos određenih podataka nije validan! \n Popuniti sva polja!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

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



