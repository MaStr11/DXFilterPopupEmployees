using System.Collections.Generic;

namespace DXApplication1
{
    public class Order
    {
        public int Id { get; set; }
        public Employee AssignedTo { get; set; }
    }
    public static class Orders
    {
        public static Order Order1 { get; } = new Order { Id = 1, AssignedTo = Employees.Employee1 };
        public static Order Order2 { get; } = new Order { Id = 2, AssignedTo = Employees.Employee1 };
        public static Order Order3 { get; } = new Order { Id = 3, AssignedTo = Employees.Employee2 };
        public static Order Order4 { get; } = new Order { Id = 4, AssignedTo = Employees.Employee4 };
        public static Order Order5 { get; } = new Order { Id = 5, AssignedTo = Employees.Employee4 };

        public static IList<Order> AllOrders { get; } = new List<Order>
        {
            Order1, Order2, Order3, Order4, Order5
        };
    }
}