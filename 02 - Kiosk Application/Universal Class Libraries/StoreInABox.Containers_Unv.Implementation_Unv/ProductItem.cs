using System.Collections.ObjectModel;

namespace StoreInABox.Container_Unv
{
    public class ProductItem
    {        
        public string Name { get; set; }
        public decimal Cost
        {
            get;
            set;
        }

        public ProductItem Clone()
        {
            return new ProductItem { Cost = this.Cost, Name = this.Name };
        }
    }

    public static class ProductItems
    {
        public static ObservableCollection<ProductItem> Items = new ObservableCollection<ProductItem>();

        static ProductItems()
        {
            ProductItems.Items.Add(new ProductItem { Name = "Pepsi", Cost = 1.5m });
            ProductItems.Items.Add(new ProductItem { Name = "Talking Rain", Cost = 0.8m });
            ProductItems.Items.Add(new ProductItem { Name = "7-Select Water 20oz", Cost = 0.99m });
            ProductItems.Items.Add(new ProductItem { Name = "Aquafina Water 20oz", Cost = 1.59m });
            ProductItems.Items.Add(new ProductItem { Name = "Redbull 12oz", Cost = 3.69m });
            ProductItems.Items.Add(new ProductItem { Name = "Monster Energy 16oz", Cost = 2.89m });
            ProductItems.Items.Add(new ProductItem { Name = "Rockstar Energy 16oz", Cost = 2.59m });
            ProductItems.Items.Add(new ProductItem { Name = "Redbull Sugar Free 12oz", Cost = 3.69m });
            ProductItems.Items.Add(new ProductItem { Name = "Starbucks Mocha 13.7oz", Cost = 3.29m });
            ProductItems.Items.Add(new ProductItem { Name = "Starbucks Vanilla 13.7oz", Cost = 3.29m });
            ProductItems.Items.Add(new ProductItem { Name = "Pureleaf Sweet Tea", Cost = 1.99m });
            ProductItems.Items.Add(new ProductItem { Name = "Diet Coke 20oz", Cost = 2.29m });
            ProductItems.Items.Add(new ProductItem { Name = "Mountain Dew 20oz", Cost = 2.19m });
            ProductItems.Items.Add(new ProductItem { Name = "Diet Mountain Dew 20oz", Cost = 2.19m });
            ProductItems.Items.Add(new ProductItem { Name = "Volpi - Roltini Singles w/ Mozzarella & Prosciutto", Cost = 1.99m });
            ProductItems.Items.Add(new ProductItem { Name = "Sargento String Cheese", Cost = 0.98m });
            ProductItems.Items.Add(new ProductItem { Name = "Babybel Original .7oz", Cost = 0.89m });
            ProductItems.Items.Add(new ProductItem { Name = "Turkey Havarti", Cost = 4.69m});
            ProductItems.Items.Add(new ProductItem { Name = "Turkey Ham Combo on Wheat", Cost = 4.69m });
            ProductItems.Items.Add(new ProductItem { Name = "Chicken Caesar Salad", Cost = 3.99m});
            ProductItems.Items.Add(new ProductItem { Name = "Kale and Quinoa Salad", Cost = 3.99m });
            ProductItems.Items.Add(new ProductItem { Name = "Apple Walnut Salad", Cost = 3.99m });
            ProductItems.Items.Add(new ProductItem { Name = "Apple Walnut Salad with Chicken", Cost = 3.99m });
        }

        public static ProductItem SevenSelectWater { get { return new ProductItem { Name = "Select Water 20oz", Cost = 0.99m }; } }
        public static ProductItem AquafinaWater { get { return new ProductItem { Name = "Aquafine Water 20oz", Cost = 1.59m }; } }
        public static ProductItem Redbull { get { return new ProductItem { Name = "Redbull 12oz", Cost = 3.69m }; } }
        public static ProductItem MonsterEnergy { get { return new ProductItem { Name = "Monster Energy 16oz", Cost = 2.89m }; } }
        public static ProductItem RockstarEnergy { get { return new ProductItem { Name = "Rockstar Energy 16oz", Cost = 2.59m }; } }
        public static ProductItem RedbullSugarFree { get { return new ProductItem { Name = "Redbull Sugar Free 12oz", Cost = 3.69m }; } }
        public static ProductItem StarbucksMocha { get { return new ProductItem { Name = "Starbucks Mocha 13.7oz", Cost = 3.29m }; } }
        public static ProductItem StarbucksVanilla { get { return new ProductItem { Name = "Starbucks Vanilla 13.7oz", Cost = 3.29m }; } }
        public static ProductItem PureleafSweetTea { get { return new ProductItem { Name = "Pureleaf Sweet Tea", Cost = 1.99m }; } }
        public static ProductItem DietCoke { get { return new ProductItem { Name = "Diet Coke 20oz", Cost = 2.29m }; } }
        public static ProductItem MountainDew { get { return new ProductItem { Name = "Mountain Dew 20oz", Cost = 2.19m }; } }
        public static ProductItem Volpi { get { return new ProductItem { Name = "Volpi - Roltini Singles w/ Mozzarella & Prosciutto", Cost = 1.99m }; } }
        public static ProductItem SargentoStringCheese { get { return new ProductItem { Name = "Sargento String Cheese", Cost = 0.98m }; } }
        public static ProductItem BabybelOriginal { get { return new ProductItem { Name = "Babybel Original .7oz", Cost = 0.89m }; } }
        public static ProductItem TurkeyHavarti { get { return new ProductItem { Name = "Turkey Havarti", Cost = 4.69m }; } }
        public static ProductItem TurkeyHamCombo { get { return new ProductItem { Name = "Turkey Ham Combo on Wheat", Cost = 4.69m }; } }
        public static ProductItem ChickenCaesarSalad { get { return new ProductItem { Name = "Chicken Caesar Salad", Cost = 3.99m }; } }
        public static ProductItem KaleandQuinoaSalad { get { return new ProductItem { Name = "Kale and Quinoa Salad", Cost = 3.99m }; } }
        public static ProductItem AppleWalnutSalad { get { return new ProductItem { Name = "Apple Walnut Salad", Cost = 3.99m }; } }
    }
}
