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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFIznajmljivanjeKnjiga.Forme;

namespace WPFIznajmljivanjeKnjiga
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public static string ucitanaTabela;

        public static bool azuziraj;
        public static object selektovan;

        #region Selectupiti

        static string korisniciSelect = @"Select KorisnikID as ID,ImeKorisnika as Ime, PrezimeKorisnika as Prezime, JMBGKorisnika as JBMG, AdresaKorisnika as Adresa, 
        GradKorisnika as Grad, KontaktKorisnika as Kontakt From tblKorisnik;";
        static string vrsteSelect = @"Select VrstaID as ID, NazivVrste as Naziv From tblVrstaKnjige;";        
        static string autoriSelect = @"Select AutorID as ID, ImeAutora + ' ' + PrezimeAutora as 'Autor' From tblAutor;";
        static string iznajmljivanjeSelect = @"
         Select IznajmljivanjeID as ID, DatumIznajmiljivanja, DatumVracanja, Kazna, IznosKazne, NazivKnjige as 'Knjiga', PrezimeKorisnika as 'Korisnik'
         from tblIznajmljivanje  inner join tblKnjiga on tblIznajmljivanje.KnjigaID = tblKnjiga.KnjigaID
         inner join tblKorisnik on tblIznajmljivanje.KorisnikID = tblKorisnik.KorisnikID;";

      


        static string knjigeSelect = @" Select KnjigaID as ID,  NazivKnjige as 'Knjiga', Izdavalac, PrezimeAutora as 'Autor', NazivVrste as 'Vrsta knjige'
         from tblKnjiga  inner join tblAutor on tblKnjiga.AutorID = tblAutor.AutorID
         inner join tblVrstaKnjige on tblKnjiga.VrstaID = tblVrstaKnjige.VrstaID;";

        #endregion

        #region Select upiti sa uslovom

        static string selectUslovKorisnici = @"Select * From tblKorisnik where KorisnikID =";
        static string selectUslovVrste = @"Select * From tblVrstaKnjige where VrstaID =";
        static string selectUslovAutori = @"Select * From tblAutor where AutorID =";
        static string selectUslovIznajmljivanje = @" Select * From tblIznajmljivanje where IznajmljivanjeID =";
        static string selectUslovKnjige = @"Select * From tblKnjiga where KnjigaID =";
        //private SqlCommand selectUpit;
        


        #endregion

        #region Delete upiti
        static string korisniciDelete = @"Delete From tblKorisnik Where KorisnikID=";
        static string vrsteDelete = @"Delete From tblVrstaKnjige Where VrstaID =";
        static string autoriDelete = @"Delete From tblAutor Where AutorID =";
        static string iznajmljivanjeDelete = @"Delete From tblIznajmljivanje Where IznajmljivanjeID =";
        static string knjigeDelete = @"Delete From tblKnjiga Where KnjigaID =";
        

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            UcitajPodatke(dataGridCentralni, korisniciSelect); //podaci se odmah izgenerisu pri otvaranju
        }

        private void UcitajPodatke(DataGrid grid, string selectUpit)
        {

            try
            {
                konekcija.Open();
                SqlDataAdapter da = new SqlDataAdapter(selectUpit, konekcija);                
                DataTable dt = new DataTable();
                da.Fill(dt);
                grid.ItemsSource = dt.DefaultView;
                ucitanaTabela = selectUpit;                               
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }
        static void PopuniFormu(DataGrid grid, string selectUslov)
        {
            try
            {
                azuziraj = true;
                konekcija.Open();
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                selektovan = red;
                string upit = selectUslov + red["ID"];
                SqlCommand komanda = new SqlCommand(upit, konekcija);

                SqlDataReader citac = komanda.ExecuteReader();
                while (citac.Read())
                {
                    if (ucitanaTabela.Equals(korisniciSelect))
                    {
                        frmKorisnik prozorKorisnik = new frmKorisnik();

                        prozorKorisnik.txtImeKorisnika.Text = citac["ImeKorisnika"].ToString();
                        prozorKorisnik.txtPrezimeKorisnika.Text = citac["PrezimeKorisnika"].ToString();
                        prozorKorisnik.txtJMBGKorisnika.Text = citac["JMBGKorisnika"].ToString();
                        prozorKorisnik.txtAdresaKorisnika.Text = citac["AdresaKorisnika"].ToString();
                        prozorKorisnik.txtGradKorisnika.Text = citac["GradKorisnika"].ToString();
                        prozorKorisnik.txtKontaktKorisnika.Text = citac["KontaktKorisnika"].ToString();

                        prozorKorisnik.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(vrsteSelect))
                    {
                        frmVrstaKnjige prozorVrstaKnjige = new frmVrstaKnjige();
                        prozorVrstaKnjige.txtNazivVrste.Text = citac["NazivVrste"].ToString();                    
                        prozorVrstaKnjige.ShowDialog();
                    }

                    else if (ucitanaTabela.Equals(autoriSelect))
                    {
                        frmAutor prozorAutor = new frmAutor();

                        prozorAutor.txtImeAutora.Text = citac["ImeAutora"].ToString();
                        prozorAutor.txtPrezimeAutora.Text = citac["PrezimeAutora"].ToString();
                        prozorAutor.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(iznajmljivanjeSelect))
                    {
                        Iznajmljivanje prozorIznajmljivanje = new Iznajmljivanje();
                        
                        prozorIznajmljivanje.dpDatumIznajmiljivanja.Text = citac["DatumIznajmiljivanja"].ToString();
                        prozorIznajmljivanje.dpDatumVracanja.Text = citac["DatumVracanja"].ToString();
                        prozorIznajmljivanje.cbKazna.IsChecked = (bool)citac["Kazna"];
                        prozorIznajmljivanje.txtIznosKazne.Text = citac["IznosKazne"].ToString();
                        prozorIznajmljivanje.cbxKnjiga.SelectedValue = citac["KnjigaID"].ToString();
                        prozorIznajmljivanje.cbxKorisnik.SelectedValue = citac["KorisnikID"].ToString();

                        prozorIznajmljivanje.ShowDialog();
                    }

                    else if (ucitanaTabela.Equals(knjigeSelect))
                    {
                        frmKnjiga prozorKnjiga = new frmKnjiga();


                        prozorKnjiga.txtNazivKnjige.Text = citac["NazivKnjige"].ToString();
                        prozorKnjiga.txtIzdavalac.Text = citac["Izdavalac"].ToString();

                       prozorKnjiga.cbxVrstaKnjige.SelectedValue = citac["VrstaID"].ToString();
                        prozorKnjiga.cbxAutor.SelectedValue = citac["AutorID"].ToString();


                        prozorKnjiga.ShowDialog();
                    }                    
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();

                azuziraj = false;
            }
        }
        static void ObrisiZapis(DataGrid grid, string deleteUpit)
        {
            try
            {
                konekcija.Open();

                DataRowView red = (DataRowView)grid.SelectedItems[0];
                string upit = deleteUpit + red["ID"];

                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (rezultat == MessageBoxResult.Yes)
                {
                    SqlCommand komanda = new SqlCommand(upit, konekcija);
                    komanda.ExecuteNonQuery();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u drugim tabelama.", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }






        //ISPIS
        private void BtnKorisnici_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, korisniciSelect);
        }

        private void BtnIznajmljivanje_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, iznajmljivanjeSelect);
        }

        private void BtnKnjige_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, knjigeSelect);
        }

        private void BtnAutor_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, autoriSelect);
        }

        private void BtnVrstaKnjige_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, vrsteSelect);
        }
        



        //DODAJ
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;

            if (ucitanaTabela.Equals(korisniciSelect))
            {
                prozor = new frmKorisnik();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, korisniciSelect); //automatski se desi

            }
            else if (ucitanaTabela.Equals(iznajmljivanjeSelect))
            {
                prozor = new Iznajmljivanje();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, iznajmljivanjeSelect);
            }
                        
            else if (ucitanaTabela.Equals(knjigeSelect))
            {
                prozor = new frmKnjiga();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, knjigeSelect);
            }                
            
            else if (ucitanaTabela.Equals(autoriSelect))
            {
                prozor = new frmAutor();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, autoriSelect);
            }                      
           
            else if (ucitanaTabela.Equals(vrsteSelect))
            {
                prozor = new frmVrstaKnjige();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, vrsteSelect);
            }            

        }
        //IZMENI
        private void BtnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(korisniciSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovKorisnici);
                UcitajPodatke(dataGridCentralni, korisniciSelect);
            }
            else if (ucitanaTabela.Equals(knjigeSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovKnjige);
                UcitajPodatke(dataGridCentralni, knjigeSelect);
            }
            else if (ucitanaTabela.Equals(autoriSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovAutori);
                UcitajPodatke(dataGridCentralni, autoriSelect);
            }
            else if (ucitanaTabela.Equals(iznajmljivanjeSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovIznajmljivanje);
                UcitajPodatke(dataGridCentralni, iznajmljivanjeSelect);
            }
            else if (ucitanaTabela.Equals(vrsteSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovVrste);
                UcitajPodatke(dataGridCentralni, vrsteSelect);
            }
        }
        //OBRISI
        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(korisniciSelect))
            {
                ObrisiZapis(dataGridCentralni, korisniciDelete);
                UcitajPodatke(dataGridCentralni, korisniciSelect);
            }
            else if (ucitanaTabela.Equals(knjigeSelect))
            {
                ObrisiZapis(dataGridCentralni, knjigeDelete);
                UcitajPodatke(dataGridCentralni, knjigeSelect);
            }
            else if (ucitanaTabela.Equals(autoriSelect))
            {
                ObrisiZapis(dataGridCentralni, autoriDelete);
                UcitajPodatke(dataGridCentralni, autoriSelect);
            }
            else if (ucitanaTabela.Equals(iznajmljivanjeSelect))
            {
                ObrisiZapis(dataGridCentralni, iznajmljivanjeDelete);
                UcitajPodatke(dataGridCentralni, iznajmljivanjeSelect);
            }
            else if (ucitanaTabela.Equals(vrsteSelect))
            {
                ObrisiZapis(dataGridCentralni, vrsteDelete);
                UcitajPodatke(dataGridCentralni, vrsteSelect);
            }
        }
    }
}
