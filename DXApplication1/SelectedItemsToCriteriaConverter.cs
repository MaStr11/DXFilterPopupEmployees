using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace DXApplication1
{
    public class SelectedItemsToCriteriaConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is BinaryOperator op)
            {
                OperandValue operandValue = op.RightOperand as OperandValue;
                return operandValue.Value;
            }

            return new List<TeamMember>(0);
        }
        object IValueConverter.ConvertBack(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IList<TeamMember> selectedTeamMember)
            {

            }

            return null;
        }
    }
}
