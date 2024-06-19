using Client.Core.Data.Entities;
using System;

namespace idn.AnPhu.Biz.Models
{
    public class PrdCategories : EntityBase
    {
        [DataColum]
        public int PrdCategoryId { get; set; }

        [DataColum]
        public int ParentId { get; set; }

        [DataColum]
        public string PrdCategoryTitle { get; set; }

        [DataColum]
        public string PrdCategoryDescription { get; set; }

        [DataColum]
        public string PrdCategoryKeyword { get; set; }

        [DataColum]
        public string PrdCategoryShortName { get; set; }

        [DataColum]
        public string PrdCategorySummary { get; set; }

        [DataColum]
        public string CreateBy { get; set; }

        [DataColum]
        public DateTime CreateDate { get; set; }

        [DataColum]
        public DateTime UpdateDate { get; set; }

        [DataColum]
        public bool IsActive { get; set; }

        [DataColum]
        public int OrderNo { get; set; }

        [DataColum]
        public string Culture { get; set; }
    }
}
