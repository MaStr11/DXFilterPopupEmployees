using DevExpress.Data.Filtering;
using DevExpress.Xpf.Grid;
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

namespace DXApplication1
{
    /// <summary>
    /// Interaktionslogik für GroupingPopupFilter.xaml
    /// </summary>
    public partial class GroupingPopupFilter : UserControl
    {
        public GroupingPopupFilter()
        {
            InitializeComponent();
            var gc = new GridControl();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DataControlBase.ItemsSourceProperty.AddOwner(typeof(GroupingPopupFilter));
        public static readonly DependencyProperty CustomColumnFilterContentPresenterProperty = DependencyProperty.Register(nameof(CustomColumnFilterContentPresenter), typeof(CustomColumnFilterContentPresenter), typeof(GroupingPopupFilter));
        public static readonly DependencyProperty FilterPropertyNameProperty = DependencyProperty.Register(nameof(FilterPropertyName), typeof(string), typeof(GroupingPopupFilter), new PropertyMetadata(null));
        public static readonly DependencyProperty GroupPropertyNameProperty = DependencyProperty.Register(nameof(GroupPropertyName), typeof(string), typeof(GroupingPopupFilter), new PropertyMetadata(null));

        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public CustomColumnFilterContentPresenter CustomColumnFilterContentPresenter
        {
            get => (CustomColumnFilterContentPresenter)GetValue(CustomColumnFilterContentPresenterProperty);
            set => SetValue(CustomColumnFilterContentPresenterProperty, value);
        }

        public string GroupPropertyName
        {
            get => (string)GetValue(GroupPropertyNameProperty);
            set => SetValue(GroupPropertyNameProperty, value);
        }

        public string FilterPropertyName
        {
            get => (string)GetValue(FilterPropertyNameProperty);
            set => SetValue(FilterPropertyNameProperty, value);
        }

        private void GridControl_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var grid = (GridControl)sender;
            var col = grid.Columns[FilterPropertyName];
            var selectedRows = grid.GetSelectedRowHandles();
            var selectedValues = selectedRows.Select(r => grid.GetCellValue(r, col)).Distinct().ToList();
            CustomColumnFilterContentPresenter.CustomColumnFilter = selectedValues.Count == 0
                ? null
                : new InOperator(CustomColumnFilterContentPresenter.ColumnFilterInfo.Column.FieldName, selectedValues);
        }

        private void GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (CustomColumnFilterContentPresenter.CustomColumnFilter is InOperator inOperator)
            {
                var filterValues = inOperator.Operands.OfType<OperandValue>().Select(c => c.Value).Distinct().ToList();
                if (filterValues.Count > 0)
                {
                    var grid = (GridControl)sender;
                    var column = grid.Columns[FilterPropertyName];
                    for (int i = 0; i < grid.VisibleRowCount; i++)
                    {
                        var rowHandle = grid.GetRowHandleByVisibleIndex(i);
                        if (grid.IsGroupRowHandle(rowHandle))
                        {
                            continue;
                        }
                        var rowValue = grid.GetCellValue(rowHandle, column);
                        if (filterValues.Contains(rowValue))
                        {
                            grid.SelectItem(rowHandle);
                        }
                    }
                }
            }
        }
    }
}
