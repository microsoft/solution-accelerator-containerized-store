using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StoreInABox.Container_Unv
{
    public interface IContainer<T> : IContainer
    {
        ObservableCollection<T> Stock { get; }

        ObservableCollection<T> Basket { get; }
    }
}
