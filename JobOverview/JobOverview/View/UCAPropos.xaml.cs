using System;
using System.Collections.Generic;
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

namespace JobOverview.View
{
    /// <summary>
    /// Logique d'interaction pour UCAPropos.xaml
    /// </summary>
    public partial class UCAPropos : UserControl
    {
        double sb_max_height = 0;
        double actual_pos = 0;
        double calcul = 0;

        public UCAPropos()
        {
            InitializeComponent();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden; // Cache la scrollbar vertical

            // Initialisation du timer.
            System.Windows.Threading.DispatcherTimer Timer1 = new System.Windows.Threading.DispatcherTimer();
            Timer1.Tick += new EventHandler(dispatcherTimer_Tick);
            // Définition d'un interval de temps de 1ms.
            Timer1.Interval = new TimeSpan(0, 0, 0, 0, 1);
            // Lancement du timer.
            Timer1.Start(); 
        }



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // On recupère la position maximum de la scrollbar.
            sb_max_height = scrollViewer.ScrollableHeight;
            // On recupère la position actuel de la scrollbar.
            actual_pos = scrollViewer.VerticalOffset; 

            if (actual_pos == sb_max_height)
            {
                // On revient à la position initiale du scrolling.
                scrollViewer.ScrollToVerticalOffset(0); 
                actual_pos = 0;
            }
            else
            {
                // On augmente la position (c'est le pas du scrolling qui défini la rapidité).
                calcul = actual_pos + 1;
                // Si la nouvelle position dépasse la taille max on modifie la nouvelle position 
                // qui sera égale à la taille max (on évite les dépassements).
                if (calcul > sb_max_height)
                    calcul = sb_max_height;
                // On ajoute la nouvelle position.
                scrollViewer.ScrollToVerticalOffset(calcul); 
            }
        }
    }


}


