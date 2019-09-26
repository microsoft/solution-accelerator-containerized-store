using System.Collections.Generic;

namespace StoreInABox.Container_Unv
{
    public class StockChangedEventArgs
    {
        public List<IContainer> Added { get; }

        public List<IContainer> Removed { get; set; }
    }
}