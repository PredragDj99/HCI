using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Classes
{
    public class Knjige
    {
        public static List<String> kategorijeKnjiga = new List<String>() { "DECIJE", "BIOGRAFIJA" , "ISTORIJA", "NAUKA", "ROMANI", "OPSTE", "UMETNOST", "FANTASTIKA", "STRIP" };

        public ImageSource Slika { get; set; }      
        public String Naziv { get; set; }
        public Int32 Primeraka { get; set; }
        public String Kategorija { get; set; }
        public DateTime Datum { get; set; }
        public String Referenca { get; set; }

        public Knjige(string text) { } 

        public Knjige(ImageSource slika,string naziv, Int32 primeraka, string kategorija,DateTime datum, string referenca)
        {
            Slika = slika;
            Naziv = naziv;
            Primeraka = primeraka;
            Kategorija = kategorija;
            Datum = datum;
            Referenca = referenca;
        }
    }
}
