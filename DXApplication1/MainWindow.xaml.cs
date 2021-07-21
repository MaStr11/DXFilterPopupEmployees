using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GridControl_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var grid = (GridControl)sender;
            var templatedParent = grid.TemplatedParent;
            if (templatedParent is CustomColumnFilterContentPresenter presenter)
            {
                var selection = grid.SelectedItems.Cast<TeamMember>().Select(tm => tm.MemberId).Distinct().ToList();
                presenter.CustomColumnFilter = selection.Count == 0
                    ? (CriteriaOperator)new UnaryOperator(UnaryOperatorType.Not, new InOperator("AssignedTo.Id"))
                    : new InOperator("AssignedTo.Id", selection);
            }
        }

        private void GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = (GridControl)sender;
            var templatedParent = grid.TemplatedParent;
            if (templatedParent is CustomColumnFilterContentPresenter presenter)
            {
                var dataControl = presenter.ColumnFilterInfo.Column.View.DataControl;
                var baseFilterCriteria = dataControl.FilterCriteria;
                if (baseFilterCriteria is null)
                {
                    return;
                }
                var split = CriteriaColumnAffinityResolver.SplitByColumns(baseFilterCriteria);
                if (split.TryGetValue(new OperandProperty("AssignedTo.Id"), out var criteria))
                {
                    if (criteria is InOperator op)
                    {
                        var consts = op.Operands.OfType<OperandValue>().Select(c => c.Value).OfType<int>().ToList();
                        var selection = ((IEnumerable<TeamMember>)grid.ItemsSource).Where(tm => consts.Contains(tm.MemberId));
                        foreach (var item in selection)
                        {
                            grid.SelectedItems.Add(item);
                        }
                    }
                }
            }
        }
    }
}
