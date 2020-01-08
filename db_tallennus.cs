using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.IO;
/*Tallennetaan ehdokkaat-tiedoston sisältö tietokantaan. Tallennetaan tauluihin ehdokas ja puolueessa. 
 Taulun puolue tiedot (10 riviä) on syötetty etukäteen*/
namespace tietokantatesti_hr
{
    class Db_tallennus
    {
        
        string[] rivit;
        string enimi;
        string snimi;
        string puolue;
        string aanet;
        int aanet2 = 0;
        int henk_id;
        public void Tallenna()
        {
            //luetaan tiedostosta rivit taulukkoon, ääkköset encodingilla ok
            rivit = File.ReadAllLines("ehdokkaat.txt", Encoding.Default);  
            //tietokantayhteyden määrittely
            NpgsqlConnection conn =
                 new NpgsqlConnection(
                     "Server=127.0.0.1;User Id=postgres;" +
                     "Password=salasana;Database=vaalit;");
            //yhteyden avaaminen
            conn.Open();
           try {
                for (int i = 0; i < rivit.Length; i++)
                {
                    //pilkotaan ehdokastietorivi taulukosta rivit toiseen taulukkoon
                    string[] kentat = rivit[i].Split();
                    //etunimi on indeksissä 0, sukunimi indeksissä 1 jne.
                    enimi = kentat[0];                          
                    snimi = kentat[1];
                    puolue = kentat[2];
                    aanet = kentat[3];
                    //muunnetaan merkkijono aanet kokonaisluvuksi
                    aanet2 = int.Parse(aanet);
                    //annetaan henkilölle id-numero
                    henk_id = i + 1;
                
                    //sql-lauseiden muodostaminen
                    String lause = "insert into ehdokas (enimi, snimi, puolue, aanet, henk_id) values ('" + enimi + "', '" +
                    snimi + "', '" + puolue + "', " + aanet2 + ", " + henk_id + ")";
                    String lause2 = "insert into puolueessa (e_id, lyh) values (" + henk_id + ", '" + puolue + "')"; 
                    //alustetaan komento
                    NpgsqlCommand cmd = new NpgsqlCommand(lause, conn);
                    //suoritetaan lause
                    cmd.ExecuteScalar();
                    NpgsqlCommand cmd2 = new NpgsqlCommand(lause2, conn);
                    cmd2.ExecuteScalar();
                    //vapautetaan resurssit
                    cmd.Dispose();
                    cmd2.Dispose();
                }
            }
            catch (Exception)
            {   
                //samaa id:tä (henk_id) ei voi tallentaa uudestaan
                Console.WriteLine("Virhe! Ehdokkaiden tiedot on jo tallennettu.");
            }
            //suljetaan tietokantayhteys
            conn.Close();
        }
    }
}

