using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.IO;
/* Tallennetaan vaalit-tiedosto tietokantaan ja haetaan sieltä tietoja valtuustoon valituista henkilöistä*/
namespace tietokantatesti_hr
{
    class Program
    {
        static void Main(string[] args)
        {
             //vaalit.txt tiedoston sisällön tallennus tietokantaan tehtiin ensimmäisenä
             //sitä ei voi tehdä kuin yhden kerran, mutta ohjelma ei kaadu uuteen yritykseen
            Db_tallennus tallennus = new Db_tallennus();    //luodaan olio talennus
            tallennus.Tallenna();                           //kutsutaan metodia tallenna
            
            Puolue puolue = new Puolue();
            // tallentaan yksittäisen puolueen yhteenlasketut äänet tietokantaan
            puolue.PuolueAanet();
            //järjestetään ehdokkaat äänimäärän mukaan
            puolue.Jarjesta();
            //tallennetaan järjestysluku ja vertaililuku tietokantaan
            puolue.TallennaJarjestys();

            Valitut valitut = new Valitut();
            //tulostetaan valitut ehdokkaat vertailuluvun mukaisessa järjestyksessa
            valitut.TulostaValitut();
            //tulostetaan valitut ehdokkaat puolueittain
            valitut.TulostaPuolueittain();
           
            Console.ReadLine();
        }

    }
}
