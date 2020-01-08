using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Threading.Tasks;
/*Luetaan ehdokkaiden äänet puolueittain tietokannan taulusta ehdokas ja tallennetaan kokonaisäänimäärä tauluun puolue.
 * Äänimäärät tallennetaan myös listaan. Puolueen ehdokkaat laitetaan järjestykseen sql-lauseen avulla.*/
namespace tietokantatesti_hr
{
    class Puolue
    {
        //puoluelista
        private String[] puolueet = { "KOK", "PS", "SDP", "KESK", "VIHR", "VAS", "KD", "KTP", "SSP", "MUUT"};
        //kokonaisäänimäärien lista
        private List<int> aanimaara = new List<int>();
        //update-lauseiden lista
        private List<string> updlauseet = new List<string>();
        //haetaan puolueiden äänet tietokannasta ja tallennetaan kokonaisäänimäärät
        public List<int> PuolueAanet()
        {
            NpgsqlConnection conn =
                new NpgsqlConnection(
                    "Server=127.0.0.1;User Id=postgres;" +
                    "Password=salasana;Database=vaalit;");
            conn.Open();
            Console.WriteLine("\n Tallennetaan puoleiden äänet\n "); 
            for (int i = 0; i < puolueet.Length; i++)
            {
                //sql-lause, jolla lasketaan yksittäisen puolueen äänet Ehdokas-taulusta
                String lause = "select sum(aanet) from ehdokas where puolue = '" + puolueet[i] + "'";
                //alustetaan komento
                NpgsqlCommand cmd = new NpgsqlCommand(lause, conn);
                //suoritetaan lause
                NpgsqlDataReader dr = cmd.ExecuteReader();
                //luetaan data
                dr.Read();
                //sql-lause, jolla tallennetaan puolueen kaikki äänet tauluun Puolue
                String lause2 = "update puolue set kaikkiaanet = " + dr[0] + "WHERE lyhenne = '" + puolueet[i] + "'";
                Console.WriteLine(puolueet[i] + " yht " + dr[0]);
                //yhden puolueen äänet kokonaislukuna
                int aani = Convert.ToInt32(dr[0]);
                //lisätään puolueen äänet listaan
                aanimaara.Add(aani);
                //suljetaan lukija
                dr.DisposeAsync();
                //tallennetaan äänet tietokantaan
                NpgsqlCommand cmd2 = new NpgsqlCommand(lause2, conn);
                NpgsqlDataReader dr2 = cmd2.ExecuteReader();
                dr2.Read();
                dr2.DisposeAsync();
                cmd.Dispose();
                cmd2.Dispose();
            }
            
            conn.Close();
            return aanimaara;
        }

        //järjestään ehdokkaat äänimäärän mukaiseen järjestykseen
        public void Jarjesta()
        {
            NpgsqlConnection conn =
                new NpgsqlConnection(
                    "Server=127.0.0.1;User Id=postgres;" +
                    "Password=salasana;Database=vaalit;");
            conn.Open();
            
            //käydään ehdokkaat läpi puolueittain ja laitetaan järjestykseen
            for (int i = 0; i < puolueet.Length; i++)
            {
                //sql-lause, jossa ehdokkaat järjestetään äänten mukaan puolueittain
                String lause = "select henk_id, aanet from ehdokas where puolue = '" + puolueet[i] + "' order by aanet desc";
                //alustetaan komento
                NpgsqlCommand cmd = new NpgsqlCommand(lause, conn);
                //suoritetaan lause
                NpgsqlDataReader dr = cmd.ExecuteReader();
                int kaikki = aanimaara[i];
                int nro = 0;
                Console.WriteLine("\n Järjestysluvut, " + puolueet[i] + ", äänet yhteensä " + kaikki + "\n");
                //luetaan tietokannat rivit 
                while (dr.Read())
                {
                    //ehdokkaan id-numero
                    var henkilo = dr[0];
                    var aanet = dr[1];
                    //järjestysnumero
                    nro++;
                    //vertailuluku
                    int vertluku = kaikki/nro;
                    Console.WriteLine( "Nro " + nro + " Id " + henkilo + " Äänet " + aanet + " Vertluku " + vertluku );
                    //sql-lause, jolla tallennetaan järjestysnumero ja vertailuluku tietokantaan
                    string updlause = "update puolueessa set j_nro = " + nro + ", vertluku = " + vertluku + " WHERE e_id = " + henkilo;
                    //tallennetaan update-lauseet listaan
                    updlauseet.Add(updlause);  
                }
                
                dr.DisposeAsync();
                
                cmd.Dispose();
            }
            conn.Close();
        }
        //tallennetaan järjestystysluku ja vertailuluku tietokantaan tauluun Puolueessa
        public void TallennaJarjestys()
        {
            NpgsqlConnection conn =
                new NpgsqlConnection(
                    "Server=127.0.0.1;User Id=postgres;" +
                    "Password=salasana;Database=vaalit;");
            conn.Open();
            //haetaan sql-lauseet listasta ja suoritetaan
            foreach (var item in updlauseet)
            {
                string updlause = item;
                NpgsqlCommand komento = new NpgsqlCommand(updlause, conn);
                //suoritetaan lause
                komento.ExecuteScalar();
                //vapautetaan resurssit
                komento.Dispose();
            }
            
            conn.Close();
        }
    }
}
