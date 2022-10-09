
namespace GR44.VendingMachine.SalesPromotions
{
    public class SalesPromotionService
    {
        private Dictionary<string, SalesPromotion> Promo = new();

        public void Add(SalesPromotion promo) => Promo.Add(promo.TriggerItem.Key, promo);

        public SalesPromotion? FindSalesPromotion(IPromotable item)
        {
            if (Promo.ContainsKey(item.Key))
                return Promo[item.Key];
            else
                return null;
        }

    }
}
