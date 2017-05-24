using JobOverview.ViewModel;
using System.Windows;

namespace JobOverview.View
{
	/// <summary>
	/// Fenêtre principale de l'application
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
            // Définition du language pour l'affichage des dates.
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            InitializeComponent();
			DataContext = new VMMain();

			Loaded += MainWindow_Loaded;
		}

		// Après chargement de la fenêtre.
		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			// Affichage d'une fenêtre modale d'identification.
			var dlg = new ModalWindow(new VMLogin());
			dlg.Title = "Identification";
			bool? res = dlg.ShowDialog();

			// Si l'utilisateur annule, on ferme l'application.
			if (!res.Value) Close();
		}
	}
}
