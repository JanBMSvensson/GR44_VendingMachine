
namespace GR44.VendingMachine.SalesPromotions
{

    public interface IPromotable
    {
        string Key { get; }
        decimal Price { get; }
    }

    public abstract class SalesPromotion
    {
        public readonly IPromotable TriggerItem;
        public readonly DateTime? Expires;

        public virtual bool IsValid => Expires is null ? true : Expires > DateTime.Now;

        public SalesPromotion(IPromotable triggerItem, DateTime? expires = null)
        {
            TriggerItem = triggerItem;
            Expires = expires;
        }
    }

    public class CampaignPrice : SalesPromotion
    {
        private decimal DefaultPriceSEK;
        private decimal PromotionPriceSEK;
        public CampaignPrice(IPromotable triggerItem, decimal promotionPrice, DateTime? expires = null) : base(triggerItem, expires) 
        {
            this.PromotionPriceSEK = promotionPrice;
            this.DefaultPriceSEK = triggerItem.Price; // keep a copy of the default price
        }

    }

    public class CrossSell : SalesPromotion
    {
        // Promote to buy a different kind of product

        private IPromotable PromoteItem;
        private decimal DefaultPriceSEK; 
        private decimal PromotionPriceSEK;

        public CrossSell(IPromotable triggerItem, IPromotable promoteItem, decimal promotionPrice, DateTime? expires = null) : base(triggerItem, expires)
        {
            this.PromoteItem = promoteItem;
            this.PromotionPriceSEK = promotionPrice;
            this.DefaultPriceSEK = promoteItem.Price; // keep a copy of the default price
        }

        public override bool IsValid => base.IsValid && this.PromoteItem.Price == this.DefaultPriceSEK; // not valid if the default price has changed
    }

    public class MultiSell : SalesPromotion
    {
        // Promote to buy more than one of a product

        private int PayCount;
        private int ReceiveCount;

        public MultiSell(IPromotable triggerItem, int payCount, int receiveCount, DateTime? expires = null) : base(triggerItem, expires)
        {
            this.PayCount = payCount;
            this.ReceiveCount = receiveCount;
        }

    }
}
