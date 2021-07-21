using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXApplication1
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {

        }
        public IList<Order> OrderItems { get; } = Orders.AllOrders;
    }
}
