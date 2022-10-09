namespace GR44.VendingMachine.Orders
{
    public class OrderService
    {
        
        private List<IOrderItem> Items = new();

        public void AddItem(IOrderItem item)
        {
            var index = Items.IndexOf(item);
            if (index >= 0)
                Items[index].ItemCount++;
            else
                Items.Add(item);
        }

        public void Clear() => Items.Clear();
        public int OrderSum => Items.Sum((item) => item.ItemSum);

        
    }


    public interface IOrderItem
    {
        int ItemCount { get; set; }
        string ItemName { get; }
        int ItemPrice { get; }

        virtual int ItemSum => ItemCount * ItemPrice; // default implementation
    }
}

