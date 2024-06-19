using Client.Core.Data.Entities;
using System;

namespace idn.AnPhu.Biz.Models
{
    public class ProductVersions : EntityBase
    {
        [DataColum]
        public int VersionId { get; set; }

        [DataColum]
        public int ProductId { get; set; }

        [DataColum]
        public string VersionCode { get; set; }

        [DataColum]
        public string VersionTitle { get; set; }

        [DataColum]
        public int VersionPrice { get; set; }

        [DataColum]
        public string VersionDescription { get; set; }

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

        [DataColum]
        public bool IsRegister { get; set; }

        [DataColum]
        public int RegistrationFeeHN { get; set; }

        [DataColum]
        public int LicenseFeeHN { get; set; }

        [DataColum]
        public int RoadTaxHN { get; set; }

        [DataColum]
        public int InsuranceFeeHN { get; set; }

        [DataColum]
        public int RegistrationFeeHCM { get; set; }

        [DataColum]
        public int LicenseFeeHCM { get; set; }

        [DataColum]
        public int RoadTaxHCM { get; set; }

        [DataColum]
        public int CertificateFeeHCM { get; set; }

        [DataColum]
        public int InsuranceFeeHCM { get; set; }

        [DataColum]
        public int RegistrationFeeKHAC { get; set; }

        [DataColum]
        public int LicenseFeeKHAC { get; set; }

        [DataColum]
        public int RoadTaxKHAC { get; set; }

        [DataColum]
        public int CertificateFeeKHAC { get; set; }

        [DataColum]
        public int InsuranceFeeKHAC { get; set; }

        [DataColum]
        public string CreateBy { get; set; }

        [DataColum]
        public int CertificateFeeHN { get; set; }

    }
}
