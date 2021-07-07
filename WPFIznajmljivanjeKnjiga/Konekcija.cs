using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFIznajmljivanjeKnjiga
{
    class Konekcija
    {
        public static SqlConnection KreirajKonekciju()
        {
            SqlConnectionStringBuilder ccnSb = new SqlConnectionStringBuilder();
            ccnSb.DataSource = @"DESKTOP-OIB17NG\SQLEXPRESS02"; //@"DESKTOP-M6GDNV3\SQLEXPRESS"; //Ovde unosite svoj server name, možete ga pronaći pri konekciji u okviru Micosoft SQL Management studija
            ccnSb.InitialCatalog = "Biblioteka"; //Naziv baze u okviru vašeg projekta
            ccnSb.IntegratedSecurity = true;

            string con = ccnSb.ToString();

            SqlConnection konekcija = new SqlConnection(con);
            return konekcija;

        }
    }
}
