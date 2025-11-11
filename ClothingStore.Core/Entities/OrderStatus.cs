using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.Core.Entities
{
    public enum OrderStatus
    {
    Pending = 0,
    Approved = 1,
    Shipped = 2,
    Cancelled = 3,
    Delivered = 4
    }
}
