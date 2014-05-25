using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService
{
    public class Item
    {
        public virtual ICollection<Tag> Tags { get; set; }
    }

    public class Tag
    {
        public virtual ICollection<Item> Items { get; set; }
    }

    public class SomeService
    {
        public void DoStuff(Item item)
        {
            // Do stuff
        }
    }
}
