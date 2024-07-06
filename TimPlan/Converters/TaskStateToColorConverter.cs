using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TimPlan.Models.TaskModel;

namespace TimPlan.Converters
{
    public class TaskStateToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TaskState state)
            {
                switch (state)
                {
                    case TaskState.Completed:
                        return Brushes.PaleGreen;
                    case TaskState.Accepted:
                        return Brushes.LightSkyBlue;
                    case TaskState.Suspended:
                        return Brushes.LemonChiffon;
                    default:
                        return Brushes.Transparent;
                }
            }
            return Brushes.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
