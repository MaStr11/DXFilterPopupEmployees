﻿using DevExpress.Data;
using DevExpress.Data.Access;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Xpf.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DXApplication1
{
    public partial class GroupingPopupFilter : UserControl, INotifyPropertyChanged
    {

        private object _FilteredItemSource;

        public GroupingPopupFilter()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DataControlBase.ItemsSourceProperty.AddOwner(typeof(GroupingPopupFilter), new PropertyMetadata(new PropertyChangedCallback(ItemsSourceChanged)));
        public static readonly DependencyProperty CustomColumnFilterContentPresenterProperty = DependencyProperty.Register(nameof(CustomColumnFilterContentPresenter), typeof(CustomColumnFilterContentPresenter), typeof(GroupingPopupFilter));
        public static readonly DependencyProperty FilterPropertyNameProperty = DependencyProperty.Register(nameof(FilterPropertyName), typeof(string), typeof(GroupingPopupFilter), new PropertyMetadata(null));
        public static readonly DependencyProperty GroupPropertyNameProperty = DependencyProperty.Register(nameof(GroupPropertyName), typeof(string), typeof(GroupingPopupFilter), new PropertyMetadata(null));

        public event PropertyChangedEventHandler PropertyChanged;

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GroupingPopupFilter)d).RefreshItemsSourceView();
        }

        private void RefreshItemsSourceView()
        {
            _FilteredItemSource = FilteredItemSource();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemsSourceView)));
        }

        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public object ItemsSourceView => _FilteredItemSource;

        private object FilteredItemSource()
        {
            var iColInfo = CustomColumnFilterContentPresenter?.ColumnFilterInfo?.Column as IDataColumnInfo;
            var colInfo = iColInfo.Controller.FindColumn(iColInfo.FieldName);
            var propDesc = colInfo.PropertyDescriptor;
            var filteredGridItems = new HashSet<object>();
            foreach (var item in iColInfo.Controller.ListSource)
            {
                filteredGridItems.Add(propDesc.GetValue(item));
            }

            var filteredList = new ArrayList();
            if (ItemsSource is IEnumerable enumerable)
            {
                var propDescFilter = new ComplexPropertyDescriptorReflection(grid, FilterPropertyName);
                foreach (var o in enumerable)
                {
                    var propValue = propDescFilter.GetValue(o);
                    if (filteredGridItems.Contains(propValue))
                    {
                        filteredList.Add(o);
                    }
                }
            }

            return filteredList;
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
            // Apply the selection to the CustomColumnFilter of the grid being filtered.
            // this is done by "converting" the selection into a CriteriaOperator.
            var grid = (GridControl)sender;
            var col = grid.Columns[FilterPropertyName];
            var selectedRows = grid.GetSelectedRowHandles();
            var selectedValues = selectedRows.Select(r => grid.GetCellValue(r, col)).Distinct().ToList();
            var sortedSelectedValues = SortListIfPossible(selectedValues);
            CustomColumnFilterContentPresenter.CustomColumnFilter = selectedValues.Count == 0
                ? null
                : new InOperator(CustomColumnFilterContentPresenter.ColumnFilterInfo.Column.FieldName, sortedSelectedValues);
        }

        private IList<object> SortListIfPossible(IList<object> selectedValues)
        {
            if (selectedValues.Count == 0)
            {
                return selectedValues;
            }

            var firstEntry = selectedValues[0];
            return firstEntry is IComparable
                ? selectedValues.OrderBy(i => i).ToList()
                : selectedValues;
        }

        private void GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (CustomColumnFilterContentPresenter.CustomColumnFilter is InOperator inOperator)
            {
                // The filtered grid has a CustomColumnFilter set, that was previously set by this filter pop-up.
                // We need to restore the selection based on the criteria.
                // This is the reverse operation to what is done in GridControl_SelectionChanged.
                var filterValues = inOperator.Operands.OfType<OperandValue>().Select(c => c.Value).ToHashSet();
                if (filterValues.Count > 0)
                {
                    var grid = (GridControl)sender;
                    var column = grid.Columns[FilterPropertyName];
                    // Iterate all rows and select rows which value, is one of the "in" operator operands
                    for (int i = 0; i < grid.VisibleRowCount; i++)
                    {
                        var rowHandle = grid.GetRowHandleByVisibleIndex(i);
                        if (grid.IsGroupRowHandle(rowHandle))
                        {
                            // Group rows value is the value of the first item in the group, which can cause bugs.
                            // There is no need to set the group selection, because
                            // the grid updates the group selector checkbox according to the elected group items
                            // automatically.
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

        private void GridControl_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            var filteredColumn = CustomColumnFilterContentPresenter?.ColumnFilterInfo?.Column as GridColumn;
            var filteredDataControl = filteredColumn?.View?.DataControl as GridControl;
            if (filteredDataControl == null)
            {
                return;
            }

            var filterGrid = (GridControl)sender;
            var filterColumn = filterGrid.Columns[FilterPropertyName];
            for (int i = 0; i < filterGrid.VisibleRowCount; i++)
            {
                var rowHandle = filterGrid.GetRowHandleByVisibleIndex(i);
                var cellValue = filterGrid.GetCellValue(rowHandle, filterColumn);

                var filteredRowHandle = filteredDataControl.FindRowByValue(filteredColumn, cellValue);
                if (filteredRowHandle == DataControlBase.InvalidRowHandle)
                {
                    ((GridViewBase)filterGrid.View).DeleteRow(filteredRowHandle);
                }
            }
        }
    }
}
