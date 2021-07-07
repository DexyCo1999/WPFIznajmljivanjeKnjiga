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
    /// Interaction logic for frmAutor.xaml
    /// </summary>
    public partial class frmAutor : Window
    {
        public SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmAutor()
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
                                      
                    string upit = @"Update tblAutor 
                          Set ImeAutora='" + txtImeAutora.Text + "', PrezimeAutora='" + txtPrezimeAutora.Text + "'Where AutorID=" + red["ID"];
                    SqlCommand komanda = new SqlCommand(upit, konekcija);
                    komanda.ExecuteNonQuery();
                    MainWindow.selektovan = null;
                    this.Close();
                }
                else {                    
                    string insert = @"insert into tblAutor values('" + txtImeAutora.Text + "','"  
                    + txtPrezimeAutora.Text + "');";                
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
