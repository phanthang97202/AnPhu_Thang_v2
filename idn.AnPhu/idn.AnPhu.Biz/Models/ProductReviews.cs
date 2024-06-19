using Client.Core.Data.Entities;
using System;

namespace idn.AnPhu.Biz.Models
{
    public class ProductReviews : EntityBase
    {
        [DataColum]
        public int ReviewId { get; set; }

        [DataColum]
        public int ProductId { get; set; }

        [DataColum]
        public string ReviewLogo { get; set; }

        [DataColum]
        public string ReviewTitle { get; set; }

        [DataColum]
        public string ReviewBody { get; set; }

        [DataColum]
        public string ReviewSource { get; set; }

        [DataColum]
        public string ReviewKeyword { get; set; }

        [DataColum]
        public string ReviewDescription { get; set; }

        [DataColum]
        public int OrderNo { get; set; }

        [DataColum]
        public bool IsActive { get; set; }

        [DataColum]
        public string Culture { get; set; }

        [DataColum]
        public DateTime CreateDate { get; set; }

        [DataColum]
        public DateTime UpdateDate { get; set; }

        [DataColum]
        public string UpdateBy { get; set; }

        [DataColum]
        public string CreateBy { get; set; }

    }
}
