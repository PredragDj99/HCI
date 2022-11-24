using System;
using System.Collections.Generic;
using System.IO;
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

namespace ProjektniZadatak1
{
    /// <summary>
    /// Interaction logic for Pogledaj.xaml
    /// </summary>
    public partial class Pogledaj : Window
    {
        public Pogledaj()
        {
            InitializeComponent();

        }

        public Pogledaj(int id)
        {
            InitializeComponent();
            lblNaslov.Content = MainWindow.Knjige[id].Naziv;
            lblBrojKnjiga.Content = MainWindow.Knjige[id].Primeraka;
            lblDatum.Content = MainWindow.Knjige[id].Datum;
            lblKategorija.Content = MainWindow.Knjige[id].Kategorija;
            imageBox.Source = MainWindow.Knjige[id].Slika;

            string pathRTF = MainWindow.Knjige[id].Naziv + ".rtf";   // \ProjektniZadatak1\bin\Debug\

            FileStream fileStream = new FileStream(pathRTF, FileMode.Open);
            TextRange range = new TextRange(rtbRead.Document.ContentStart, rtbRead.Document.ContentEnd);
            range.Load(fileStream, DataFormats.Rtf);
            fileStream.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void NapustiAplikacijuDugme_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
