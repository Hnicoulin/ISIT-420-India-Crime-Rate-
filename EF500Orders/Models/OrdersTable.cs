using System;
using System.Collections.Generic;

namespace EF500Orders.Models
{
    public partial class OrdersTable
    {
        public int OrdersID { get; set; }
        public int StoreID { get; set; }
        public int SalesPersonID { get; set; }
        public int CdID { get; set; }
        public int PricePaid { get; set; }
        public string Date { get; set; } = null!;

        public virtual CdTable Cd { get; set; } = null!;
        public virtual SalesPersonTable SalesPerson { get; set; } = null!;
        public virtual StoreTable Store { get; set; } = null!;
    }
}
