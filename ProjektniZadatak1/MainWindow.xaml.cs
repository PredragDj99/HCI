using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjektniZadatak1 //Predrag Djoric PR100/2018
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static List<Classes.Knjige> Knjige = new List<Classes.Knjige>();

        public MainWindow()
        {
            InitializeComponent();

            dataGridKnjige.ItemsSource = Knjige;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void NovoIzdanjeDugme_Click(object sender, RoutedEventArgs e)
        {
            DodajNovoIzdanje DodajIzdanje = new DodajNovoIzdanje();
            DodajIzdanje.ShowDialog();
            dataGridKnjige.Items.Refresh();
        }

        private void NapustiAplikacijuDugme_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnPogledaj_Click(object sender, RoutedEventArgs e)
        {      
            Pogledaj zelimDaPogledam = new Pogledaj(dataGridKnjige.SelectedIndex);
            zelimDaPogledam.Show();
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            Izmeni zelimDaIzmenim = new Izmeni(dataGridKnjige.SelectedIndex);
            zelimDaIzmenim.ShowDialog();

            dataGridKnjige.Items.Refresh();
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            //brise opis knjige
            try
            {
                int id = dataGridKnjige.SelectedIndex;

                string nazivv = MainWindow.Knjige[id].Naziv.ToString();
                string pathRTF = "../../bin/Debug/" + nazivv + ".rtf";

                File.Delete(pathRTF);
            }
            catch (Exception)
            {
                MessageBox.Show(" Opis knjige nije obrisan! Potrebno je rucno brisanje fajla na njegovoj lokaciji(ProjektniZadatak1,bin,Debug) ", "Attention", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            //Brise iz dataGrid-a
            try
            {
                Knjige.RemoveAt(dataGridKnjige.SelectedIndex);
                dataGridKnjige.Items.Refresh();
            }
            catch(Exception )
            {
                MessageBox.Show("Nije moguce obrisati!","Greska",MessageBoxButton.OK,MessageBoxImage.Error);
            }   
        }
    }
}
