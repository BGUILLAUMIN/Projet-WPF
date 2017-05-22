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




namespace JobOverview.View
{
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
            return (ModesEdition)value == ModesEdition.Consultation ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

