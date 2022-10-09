using Microsoft.VisualBasic;
using System.Text;

namespace GR44.VendingMachine.Products
{
    public abstract class Product
    {
        public readonly string Name;
        public readonly int Price;
        public readonly string Code;

        public Product(string name, int price, string code)
        {
            Name = name;
            Price = price;
            Code = code;
        }

        public virtual string[] Examine() => new string[] {$"Product: {Name}", $"Price: {Price} kr"};
    }

    public class Drink : Product, IHasAgeLimitation
    {
        public int AgeLimit => _AgeRequirement;
        private int _AgeRequirement;

        public readonly decimal AlcoholLevel;
        public Drink(string name, int price, string productCode, decimal alcoholLevel = 0m) : base(name,price, productCode) 
        { 
            AlcoholLevel = alcoholLevel;
            if (alcoholLevel > 1.5m)
                _AgeRequirement = 18;
        }

        public override string[] Examine()
        {
            var list = new List<string>(base.Examine());
            if (AlcoholLevel > 0)
                list.Add($"NOTE: this drink contains alcohol ({AlcoholLevel}%)");

            if (AgeLimit > 0)
                list.Add($"NOTE: this drink has an age limit ({AgeLimit}+)");
            
            return list.ToArray();
        }
    }

    public class Food : Product
    {
        public readonly string[] AllergySubstances = Array.Empty<string>();
        public Food(string name, int price, string productCode, params string[] allergySubstances) : base(name, price, productCode) { AllergySubstances = allergySubstances; }

        public override string[] Examine()
        {
            var list = new List<string>(base.Examine());
            if (AllergySubstances.Length > 0)
                list.Add($"NOTE: the product contains {Microsoft.VisualBasic.Strings.Join(AllergySubstances, ", ")}");

            return list.ToArray();
        }
    }

    public class Toy : Product, IHasAgeLimitation
    {
        public int AgeLimit => _AgeRequirement;
        private int _AgeRequirement;
        public Toy(string name, int price, string productCode, int ageRequirement = 0) : base(name, price, productCode)
        {
            _AgeRequirement = ageRequirement;
        }

        public override string[] Examine()
        {
            var list = new List<string>(base.Examine());

            if (AgeLimit > 0)
                list.Add($"Note that this toy is not suitable for children younger than {AgeLimit} years old.");

            return list.ToArray();
        }

    }


    public interface IHasAgeLimitation
    {
        public int AgeLimit { get; }
    }

}