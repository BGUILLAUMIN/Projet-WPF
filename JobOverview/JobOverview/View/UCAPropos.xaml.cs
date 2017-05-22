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

            //Initialisation du timer
            System.Windows.Threading.DispatcherTimer Timer1 = new System.Windows.Threading.DispatcherTimer();
            Timer1.Tick += new EventHandler(dispatcherTimer_Tick);
            Timer1.Interval = new TimeSpan(0, 0, 0, 0, 1);
            Timer1.Start(); // on lance le timer
        }



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            sb_max_height = scrollViewer.ScrollableHeight; //on recupère la position maximum de la scrollbar
            actual_pos = scrollViewer.VerticalOffset; // on recupère la position actuel de la scrollbar

            if (actual_pos == sb_max_height)
            {
                scrollViewer.ScrollToVerticalOffset(0); // on revient au debut
                actual_pos = 0;
            }
            else
            {
                calcul = actual_pos + 1; // on augmente la position
                if (calcul > sb_max_height) //si la nouvelle position dépasse la taille max on modifie la nouvelle position qui sera égale a la taille max (on évite les dépassements)
                    calcul = sb_max_height;
                scrollViewer.ScrollToVerticalOffset(calcul); // on ajoute la nouvelle position
            }
        }
    }


}


