using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace tietokantatesti_hr
{
    class Valitut 
    {
        string ehdokas2 = "";
        List<string> ehdokkaat2 = new List<string>();
        string ehdokas = "";
        List<string> ehdokkaat = new List<string>();
        public void TulostaValitut()
        {
            NpgsqlConnection conn =
                new NpgsqlConnection(
                    "Server=127.0.0.1;User Id=postgres;" +
                    "Password=salasana;Database=vaalit;");
            conn.Open();
            //sql-lause, jolla haetaan ehdokkaat tietokannasta vertailuluvun mukaisessa järjestyksessä
            String lause = "Select enimi, snimi, henk_id, vertluku, lyh from ehdokas join puolueessa " +
             "on ehdokas.henk_id = puolueessa.e_id order by vertluku desc" ;
            //alustetaan komento
            NpgsqlCommand cmd = new NpgsqlCommand(lause, conn);
            //suoritetaan lause
            NpgsqlDataReader dr = cmd.ExecuteReader();
            //luetaan data
            //dr.Read();
            int i = 1;
            while (dr.Read() && i <= 51 )  //haetaan 51 ehdokasta
            {
                
                var enimi = dr[0];
                var snimi = dr[1];
                var henk_id = dr[2];
                var vertluku = dr[3];
                var lyh = dr[4];
                
                //Console.WriteLine( henk_id + " " + enimi + " " + snimi +  " " + vertluku +" " + lyh + " ");
                ehdokas2 = i + " " + henk_id + " " + enimi + " " + snimi + "  " + vertluku + "  " + lyh + " ";
                //tallennetaan ehdokkaat listaan
                ehdokkaat2.Add(ehdokas2);
              
                i++;
            }
            dr.DisposeAsync();
            cmd.Dispose();
            conn.Close();
            Console.WriteLine("\n Seinäjoen kaupungin valtuustoon valitut henkilöt");
            //tulostetaan ehdokkaat
            foreach (var item in ehdokkaat2)
            {
                Console.WriteLine(item);
            }
        }

        public void TulostaPuolueittain()
        {
            NpgsqlConnection conn =
                new NpgsqlConnection(
                    "Server=127.0.0.1;User Id=postgres;" +
                    "Password=salasana;Database=vaalit;");
            conn.Open();
            //sql-lause, jolla haetaan tietokannasta valitut ehdokkaat puolueittain, kun tiedetään pienin vrtluku
            String lause = "Select enimi, snimi, henk_id, vertluku, lyh from ehdokas join puolueessa " + 
             "on ehdokas.henk_id = puolueessa.e_id where vertluku >= 465 order by lyh, vertluku desc ";
            //alustetaan komento
            NpgsqlCommand cmd = new NpgsqlCommand(lause, conn);
            //suoritetaan lause
            NpgsqlDataReader dr = cmd.ExecuteReader();
            
            int i = 1;
            while (dr.Read())
            {

                var enimi = dr[0];
                var snimi = dr[1];
                var henk_id = dr[2];
                var vertluku = dr[3];
                var lyh = dr[4];

                //Console.WriteLine( henk_id + " " + enimi + " " + snimi +  " " + vertluku +" " + lyh + " ");
                ehdokas = i + " " + henk_id + " " + enimi + " " + snimi + "  " + vertluku + "  " + lyh + " ";
                ehdokkaat.Add(ehdokas);

                i++;
            }
            dr.DisposeAsync();
            cmd.Dispose();
            conn.Close();
            Console.WriteLine("\n Seinäjoen kaupungin valtuustoon valitut henkilöt puolueittain");
            //tulostetaan
            foreach (var item in ehdokkaat)
            {
                Console.WriteLine(item);
            }
        }
    }
}
