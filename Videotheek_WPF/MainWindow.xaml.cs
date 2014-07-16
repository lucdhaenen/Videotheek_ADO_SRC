using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
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

using Videotheek_DLL.Classes;
using Videotheek_DLL.Services;

namespace Videotheek_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Film> films = new ObservableCollection<Film>();
        private ObservableCollection<Genre> genres = new ObservableCollection<Genre>();
        private FilmService filmService = new FilmService();
        private GenreService genreService = new GenreService();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VulSource(); 
        }

        private void VulSource()
        {
            CollectionViewSource filmViewSource = ((CollectionViewSource)(this.FindResource("filmViewSource")));
            films = filmService.GetFilms();
            filmViewSource.Source = films;
            CollectionViewSource genreViewSource = ((CollectionViewSource)(this.FindResource("genreViewSource")));
            genres = genreService.GetGenres();
            genreViewSource.Source = genres;
        }

        private Film LeesGegevens()
        {
            Int32 bandNr = new Int32();
            if (bandNrTextBox.Text == string.Empty)
            {
                bandNr = 0;
            }
            else if(!Int32.TryParse(bandNrTextBox.Text, out bandNr))
            {
                MessageBox.Show("Fout bandnummer", "Toevoegen bandnummer", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

            string titel = string.Empty;
            if (titelTextBox.Text != string.Empty)
            {
                titel = titelTextBox.Text.ToUpper();   
            }
            else
            {
                MessageBox.Show("Gelieve een titel in te geven", "Toevoegen titel", MessageBoxButton.OK, MessageBoxImage.Stop);
                titelTextBox.Focus();
            }

            Int32 genreNr = new Int32();
            if (genreNrComboBox.SelectedIndex != -1)
            {
                genreNr = (Int32)genreNrComboBox.SelectedValue;  
            }
            else
            {
                MessageBox.Show("Gelieve een genre te kiezen", "Toevoegen genre", MessageBoxButton.OK, MessageBoxImage.Stop);
                genreNrComboBox.Focus();
            }

            Int32 inVoorraad = new Int32();
            if (!Int32.TryParse(inVoorraadTextBox.Text, out inVoorraad))
            {
                MessageBox.Show("Gelieve een juiste voorraad in te vullen", "Toevoegen in voorraad", MessageBoxButton.OK, MessageBoxImage.Stop);
                inVoorraadTextBox.Focus();
            }

            Int32 uitVoorraad = new Int32();
            if (!Int32.TryParse(uitVoorraadTextBox.Text, out uitVoorraad))
            {
                MessageBox.Show("Gelieve een juiste voorraad in te vullen", "Toevoegen uit voorraad", MessageBoxButton.OK, MessageBoxImage.Stop);
                uitVoorraadTextBox.Focus();
            }

            decimal prijs = new decimal();
            if (!decimal.TryParse(prijsTextBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out prijs))
            {
                MessageBox.Show("Gelieve een juiste prijs in te vullen", "Toevoegen prijs", MessageBoxButton.OK, MessageBoxImage.Stop);
                prijsTextBox.Focus();
            }

            Int32 totaalVerhuurd = new Int32();
            if (!Int32.TryParse(totaalVerhuurdTextBox.Text, out totaalVerhuurd))
            {
                MessageBox.Show("Gelieve een juist totaal verhuurd in te vullen", "Toevoegen totaal verhuurd", MessageBoxButton.OK, MessageBoxImage.Stop);
                totaalVerhuurdTextBox.Focus();
            }

            Film film = new Film(bandNr, titel, genreNr, inVoorraad, uitVoorraad, prijs, totaalVerhuurd);

            return film;
        }

        private void SelectFilm(Film film)
        {
            VulSource();
            listBoxFilms.SelectedValue = film.BandNr;
            listBoxFilms.ScrollIntoView(listBoxFilms.SelectedItem);
            ListBoxItem selected = (ListBoxItem)listBoxFilms.ItemContainerGenerator.ContainerFromIndex(listBoxFilms.SelectedIndex);
            selected.Focus();
        }

        private void SwitchEditMode()
        {
            if (buttonToevoegen.Content.ToString() == "Toevoegen annuleren")
            {
                buttonToevoegen.Content = "Toevoegen";
                buttonVerwijderen.IsEnabled = true;
                buttonOpslaan.Content = "Wijzigingen opslaan";
                listBoxFilms.Focus();
                listBoxFilms.IsEnabled = true;                
            }
            else
            {
                buttonToevoegen.Content = "Toevoegen annuleren";
                buttonVerwijderen.IsEnabled = false;
                buttonOpslaan.Content = "Nieuwe film opslaan";
                listBoxFilms.SelectedIndex = -1;
                listBoxFilms.IsEnabled = false;                
                titelTextBox.Focus();
            }
        }        

        private void prijsTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Decimal tempPrijs = new Decimal();
            if (Decimal.TryParse(prijsTextBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out tempPrijs))
                prijsTextBox.Text = tempPrijs.ToString();
            prijsTextBox.SelectAll();
        }

        private void prijsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Decimal tempPrijs = new Decimal();
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.CurrencySymbol = "€";
            if (Decimal.TryParse(prijsTextBox.Text.Replace(",", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator).Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), NumberStyles.Currency, CultureInfo.CurrentCulture, out tempPrijs))
                prijsTextBox.Text = String.Format(nfi,"{0:c}", tempPrijs);
        }

        private void buttonToevoegen_Click(object sender, RoutedEventArgs e)
        {
            SwitchEditMode();
        }

        private void buttonOpslaan_Click(object sender, RoutedEventArgs e)
        {
            Film film = LeesGegevens();
            if (buttonToevoegen.Content.ToString() == "Toevoegen annuleren")
            {
                film.BandNr = filmService.Toevoegen(film);
                SwitchEditMode();
                MessageBox.Show("De film is toegevoegd", "Toevoegen film", MessageBoxButton.OK, MessageBoxImage.Information);                
            }
            else
            {
                filmService.Wijzigen(film);
                MessageBox.Show("De gegevens zijn gewijzigd", "Wijzigen film", MessageBoxButton.OK, MessageBoxImage.Information);                
            }            
            SelectFilm(film);
        }

        private void buttonVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxFilms.SelectedIndex < 0)
            {
                MessageBox.Show("Gelieve een film te selecteren", "Verwijderen", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (MessageBox.Show("Weet u zeker dat u deze film wilt verwijderen?", "Verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    filmService.Verwijderen((Film)listBoxFilms.SelectedItem);
                    MessageBox.Show("De film is verwijderd", "Verwideren film", MessageBoxButton.OK, MessageBoxImage.Information);
                    listBoxFilms.SelectedIndex = 1;
                    SelectFilm((Film)listBoxFilms.SelectedItem);
                }
            }
        }

    }
}
