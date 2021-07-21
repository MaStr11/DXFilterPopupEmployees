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
            var (grid, presenter) = GetGridAndPresenter(sender);
            if (presenter != null)
            {
                var selection = grid.SelectedItems.Cast<TeamMember>().Select(tm => tm.MemberId).Distinct().ToList();
                if (selection.Count == 0)
                {
                    var visitor = new DeleteAssignedToVistor();
                    var dataControl = presenter.ColumnFilterInfo.Column.View.DataControl;
                    dataControl.FilterCriteria = visitor.Process(dataControl.FilterCriteria);
                }
                else
                {
                    presenter.CustomColumnFilter = new InOperator("AssignedTo.Id", selection);
                }
            }
        }

        private (GridControl filterGrid, CustomColumnFilterContentPresenter presenter) GetGridAndPresenter(object sender)
        {
            var grid = (GridControl)sender;
            var templatedParent = grid.TemplatedParent as CustomColumnFilterContentPresenter;
            return (grid, templatedParent);
        }

        private void GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            var (grid, presenter) = GetGridAndPresenter(sender);
            if (presenter != null)
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

    class DeleteAssignedToVistor : DevExpress.Data.Filtering.Helpers.ClientCriteriaVisitorBase
    {
        protected override CriteriaOperator Visit(InOperator inOperator)
        {
            if (inOperator.LeftOperand is OperandProperty prop && prop.PropertyName == "AssignedTo.Id")
            {
                return new ConstantValue(true);
            }
            return base.Visit(inOperator);
        }
        public new CriteriaOperator Process(CriteriaOperator input)
        {
            return base.Process(input);
        }

    }
}
