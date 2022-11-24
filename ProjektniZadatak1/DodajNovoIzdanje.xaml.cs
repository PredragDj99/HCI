using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for DodajNovoIzdanje.xaml
    /// </summary>
    public partial class DodajNovoIzdanje : Window
    {

        public DodajNovoIzdanje()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cmbColor.ItemsSource = typeof(Colors).GetProperties();

            cmbKategorija.ItemsSource = Classes.Knjige.kategorijeKnjiga;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void NapustiAplikacijuDugme_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
            }
        }

        private void DodajDugme_Click(object sender, RoutedEventArgs e)
        {
            // Ovo se dodaje u dataGrid
            Classes.Knjige noveKnjige;

            if (validate())
            {
                //Disable avast da bi proslo    
                string pathZaRtf = "../../bin/Debug/" + TextBoxNaslov.Text + ".rtf"; // \ProjektniZadatak1\bin\Debug\
                FileStream fileStream = new FileStream(pathZaRtf, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);

                noveKnjige = new Classes.Knjige(imageBox.Source, TextBoxNaslov.Text, Int32.Parse(TextBoxBrojKnjiga.Text), cmbKategorija.SelectedValue.ToString(), datePicker.SelectedDate.GetValueOrDefault(), pathZaRtf);
                MainWindow.Knjige.Add(noveKnjige);

                fileStream.Close();
                this.Close();
            }
            else
            {
                MessageBox.Show("Popunite sva polja obavezna polja", "Greska!",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private bool validate()
        {
            bool result = true;
            
            //naslov
            if (TextBoxNaslov.Text.Trim().Equals(""))
            {
                result = false;
                TextBoxNaslov.BorderBrush = Brushes.Red;
                TextBoxNaslov.BorderThickness = new Thickness(1);
                lblNaslovGreska.Content = "Ne moze biti prazno!";
            }
            else if (TextBoxNaslov.Text.Length > 40)
            {
                result = false;
                TextBoxNaslov.BorderBrush = Brushes.Red;
                TextBoxNaslov.BorderThickness = new Thickness(1);
                lblNaslovGreska.Content = "Maksimalna duzina naslova je 40 karaktera!";
            }
            else
            {
                TextBoxNaslov.BorderBrush = Brushes.Green;
                lblNaslovGreska.Content = string.Empty;
            }


            // broj knjiga
            int parsiranjeBrojKnjiga;

            if (!int.TryParse(TextBoxBrojKnjiga.Text, out parsiranjeBrojKnjiga))
            {
               lblGodinaBrojKnjiga.Content= "Unesite broj knjiga spojenim ciframa bez koriscenja slova!";

                if (TextBoxBrojKnjiga.Text.Trim().Equals(""))
                {
                    result = false;
                    lblGodinaBrojKnjiga.Content = "Unesite broj knjiga!";
                }

                TextBoxBrojKnjiga.BorderBrush = Brushes.Red;
                TextBoxBrojKnjiga.BorderThickness = new Thickness(1);
                result = false;
            }
            else if (parsiranjeBrojKnjiga>1000000)
            {
                lblGodinaBrojKnjiga.Content = "Broj knjiga ne moze biti veci od 1 000 000!";
                TextBoxBrojKnjiga.BorderBrush = Brushes.Red;
                TextBoxBrojKnjiga.BorderThickness = new Thickness(1);
                result = false;
            }
            else
            {
                TextBoxBrojKnjiga.BorderBrush = Brushes.Green;
                lblGodinaBrojKnjiga.Content = string.Empty;
            }


            // kategorije
            if (cmbKategorija.SelectedItem == null)
            {
                result = false;
                cmbKategorija.BorderBrush = Brushes.Red;
                cmbKategorija.BorderThickness = new Thickness(1);
                lblKategorijaGreska.Content = "Mora biti odabrana opcija!";
            }
            else
            {
                cmbKategorija.BorderBrush = Brushes.Green;
                lblKategorijaGreska.Content = string.Empty;
            }

            // datum
            if (datePicker.SelectedDate == null)
            {
                result = false;
                datePicker.BorderBrush = Brushes.Red;
                datePicker.BorderThickness = new Thickness(1);
                lblDatumGreska.Content = "Izaberite datum!";
            }
            else
            {
                datePicker.BorderBrush = Brushes.Green;
                lblDatumGreska.Content = string.Empty;
            }

            // validacija slike
            if (imageBox.Source == null)
            {
                  result = false;
                  BrowseButton.BorderBrush = Brushes.Red;
                  BrowseButton.BorderThickness = new Thickness(1);
                  lblBrowseGreska.Content = "Izaberite sliku!";
             }
             else
             {
                  BrowseButton.BorderBrush = Brushes.Green;
                  lblBrowseGreska.Content = string.Empty;
             }

            // rtb tekst
            if (sbBrojReci.Text.Equals("Broj reci: 0"))
            {
                result = false;
                rtbEditor.BorderBrush = Brushes.Red;
                rtbEditor.BorderThickness = new Thickness(1);
                lblTekstGreska.Content = "Popunite opis knjige!";
            }
            else
            {
                rtbEditor.BorderBrush = Brushes.Green;
                lblTekstGreska.Content = string.Empty;
            }

            return result;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                openFileDialog1.InitialDirectory = path;
                // ili   openFileDialog1.InitialDirectory = @"C:\";   pa putanja

                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.DefaultExt = "jpg";
                openFileDialog1.Filter = "Image Files| *.jpg; *.jpeg; *.png";

                if (openFileDialog1.ShowDialog() == true)
                {
                    imageBox.Source = new BitmapImage(new Uri(openFileDialog1.FileName));
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Nije moguce izabrati sliku", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
 

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {

            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);

            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();

            //BROJAC RECI
            string rtbText = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            int brReci = Regex.Matches(rtbText,@"[^\s]+").Count;    //  \s znaci bilo sta osim whitespace-a
            sbBrojReci.Text = "Broj reci: " +brReci;
        }

        private void CmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbColor.SelectedItem != null)
            {
                try
                {
                    var izabrana = (PropertyInfo)cmbColor.SelectedItem;
                    var boja = (Color)izabrana.GetValue(null, null);
                    rtbEditor.Selection.ApplyPropertyValue(Inline.ForegroundProperty, boja.ToString());
                    rtbEditor.Focus();
                }
                catch (Exception)
                {
                    MessageBox.Show("Prvo selektujte tekst pa promenite boju teksta", "Greska!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }      
        }

        private void cmbSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cmbFontSize.SelectedItem != null)
            {
                try
                {
                    rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
                    rtbEditor.Focus();
                }
                catch (Exception)
                {
                    MessageBox.Show("Prvo selektujte tekst pa promenite velicinu slova", "Greska!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void CmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
            {
                try
                {

                    rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
                    rtbEditor.Focus();
                }
                catch
                {
                    MessageBox.Show("Prvo selektujte tekst pa promenite font kojim zelite da pisete", "Greska!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }
    }
}
