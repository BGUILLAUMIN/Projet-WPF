using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.Entity;
using System.Windows.Data;
using System.Globalization;
using JobOverview.ViewModel;
using JobOverview.View;
using JobOverview.Model;
using System.Windows.Media;
using System.Windows;

namespace JobOverview.View
{
    #region Convertisseurs pour les passages en mode consultation ou edition
    public class ConvModeEditActivation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ModesEdition)value == ModesEdition.Edition ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvModeEditLectureSeule : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ModesEdition)value == ModesEdition.Consultation);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Convertisseurs pour les changements de couleur dans Versions
    public class IntToColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = Colors.White;
            int d = (int)value;
            var seuil = int.Parse(parameter.ToString());
            if (d > seuil)
                c = Colors.LightGreen;
            else if (d < seuil)
                c = Colors.Tomato;

            return new SolidColorBrush(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Convertisseurs pour les passages en mode visible invisible, convertisseur non utilisé à l'état actuel
    public class ConvModeEditVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ModesEdition)value == ModesEdition.Consultation ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Convertisseurs pour bloquer la Saisie dans les expander aux personnes qui ne sont pas manager
    public class ConvModeEditManager : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();   // TODO bloquer l'accés à toutes modif pour les personnes qui ne sont pas manager
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    } 
    #endregion


}

