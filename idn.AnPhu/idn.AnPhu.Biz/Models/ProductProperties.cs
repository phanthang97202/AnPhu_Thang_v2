using Client.Core.Data.Entities;

namespace idn.AnPhu.Biz.Models
{
    public class ProductProperties : EntityBase
    {
        [DataColum]
        public int ProductPropertyId { get; set; }

        [DataColum]
        public int ProductId { get; set; }

        [DataColum]
        public string ProductPropertyTitle { get; set; }

        [DataColum]
        public string ProductPropertyBody { get; set; }

        [DataColum]
        public bool IsActive { get; set; }

        [DataColum]
        public int OrderNo { get; set; }

        [DataColum]
        public string Culture { get; set; }

        [DataColum]
        public string ProductName { get; set; }
    }
}
