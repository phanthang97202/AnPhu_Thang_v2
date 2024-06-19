using Client.Core.Data.DataAccess;
using Client.Core.Data.Entities;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Persistance.Interface;
using idn.AnPhu.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace idn.AnPhu.Biz.Persistance.SqlServer
{
    class ProductVersionsProvider : DataAccessBase, IProductVersionsProvider
    {
        public void Add(ProductVersions item)
        {
            DbCommand comm = this.GetCommand("Sp_ProductVersions_Create");

            comm.AddParameter<int>(this.Factory, "ProductId", item.ProductId);
            comm.AddParameter<string>(this.Factory, "VersionTitle", (item.VersionTitle != null && item.VersionTitle.Trim().Length > 0) ? item.VersionTitle.Trim() : null);
            comm.AddParameter<string>(this.Factory, "VersionCode", (item.VersionCode != null && item.VersionCode.Trim().Length > 0) ? item.VersionCode.Trim() : null);
            comm.AddParameter<int>(this.Factory, "VersionPrice", item.VersionPrice);
            comm.AddParameter<string>(this.Factory, "VersionDescription", (item.VersionDescription != null && item.VersionDescription.Trim().Length > 0) ? item.VersionDescription.Trim() : null);
            comm.AddParameter<string>(this.Factory, "CreateBy", (item.CreateBy != null && item.CreateBy.Trim().Length > 0) ? item.CreateBy.Trim() : null);
            comm.AddParameter<int>(this.Factory, "RegistrationFeeHN", item.RegistrationFeeHN);
            comm.AddParameter<int>(this.Factory, "LicenseFeeHN", item.LicenseFeeHN);
            comm.AddParameter<int>(this.Factory, "RoadTaxHN", item.RoadTaxHN);
            comm.AddParameter<int>(this.Factory, "InsuranceFeeHN", item.InsuranceFeeHN);
            comm.AddParameter<int>(this.Factory, "RegistrationFeeHCM", item.RegistrationFeeHCM);
            comm.AddParameter<int>(this.Factory, "LicenseFeeHCM", item.LicenseFeeHCM);
            comm.AddParameter<int>(this.Factory, "CertificateFeeHCM", item.CertificateFeeHCM);
            comm.AddParameter<int>(this.Factory, "RoadTaxHCM", item.RoadTaxHCM);
            comm.AddParameter<int>(this.Factory, "InsuranceFeeHCM", item.InsuranceFeeHCM);
            comm.AddParameter<int>(this.Factory, "RegistrationFeeKHAC", item.RegistrationFeeKHAC);
            comm.AddParameter<int>(this.Factory, "LicenseFeeKHAC", item.LicenseFeeKHAC);
            comm.AddParameter<int>(this.Factory, "RoadTaxKHAC", item.RoadTaxKHAC);
            comm.AddParameter<int>(this.Factory, "InsuranceFeeKHAC", item.InsuranceFeeKHAC);
            comm.AddParameter<int>(this.Factory, "CertificateFeeKHAC", item.CertificateFeeKHAC);
            comm.AddParameter<int>(this.Factory, "CertificateFeeHN", item.CertificateFeeHN);
            comm.AddParameter<bool>(this.Factory, "IsRegister", item.IsRegister);
            comm.AddParameter<bool>(this.Factory, "IsActive", item.IsActive);
            comm.AddParameter<int>(this.Factory, "OrderNo", item.OrderNo);
            comm.AddParameter<string>(this.Factory, "Culture", (item.Culture != null && item.Culture.Trim().Length > 0) ? item.Culture.Trim() : null);

            this.SafeExecuteNonQuery(comm);
        }

        public ProductVersions Get(ProductVersions dummy)
        {
            DbCommand comm = this.GetCommand("Sp_ProductVersions_GetById");

            comm.AddParameter<int>(this.Factory, "versionId", dummy.VersionId);

            var table = this.GetTable(comm);
            table.TableName = TableName.ProductVersions;
            //ProductVersions versions = EntityBase.ParseListFromTable<ProductVersions>(table).FirstOrDefault();

            var properties = typeof(ProductVersions).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Create a list to hold the converted objects
            var result = new List<ProductVersions>();

            // Iterate over the rows of the DataTable
            foreach (DataRow row in table.Rows)
            {
                // Create a new instance of T
                ProductVersions item = new ProductVersions();

                // Set the properties of T based on the columns in the DataTable
                foreach (var property in properties)
                {
                    if (table.Columns.Contains(property.Name) && row[property.Name] != DBNull.Value)
                    {
                        property.SetValue(item, Convert.ChangeType(row[property.Name], property.PropertyType));
                    }
                }

                // Add the object to the list
                result.Add(item);
            }

            return result[0];
        }

        public List<ProductVersions> GetAll(int startIndex, int count, ref int totalItems)
        {
            throw new System.NotImplementedException();
        }

        public List<ProductVersions> GetAllVersions(int productId, int startIndex, int count, ref int totalItems)
        {
            DbCommand comm = this.GetCommand("Sp_ProductVersions_GetAll");
            comm.AddParameter<int>(this.Factory, "productId", productId);
            DataTable table = this.GetTable(comm);
            table.TableName = TableName.ProductVersions;
            List<ProductVersions> versions = EntityBase.ParseListFromTable<ProductVersions>(table);
            return versions;
        }

        public void Import(List<ProductVersions> list, bool deleteExist)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(ProductVersions item)
        {
            DbCommand comm = this.GetCommand("Sp_ProductVersions_Delete");
            if (comm == null) return;
            comm.AddParameter<int>(this.Factory, "VersionId", item.VersionId);
            this.SafeExecuteNonQuery(comm);
        }

        public List<ProductVersions> Search(int productId, string txtSearch, int startIndex, int pageCount, ref int totalItems)
        {
            DbCommand comm = this.GetCommand("Sp_ProductVersions_Search");
            comm.AddParameter<int>(this.Factory, "productId", productId);
            comm.AddParameter<string>(this.Factory, "txtSearch", (txtSearch != null && txtSearch.Trim().Length > 0) ? txtSearch : null);
            comm.AddParameter<int>(this.Factory, "startIndex", startIndex);
            comm.AddParameter<int>(this.Factory, "count", pageCount);

            DbParameter totalItemsParam = comm.AddParameter(this.Factory, "totalItems", DbType.Int32, null);
            totalItemsParam.Direction = ParameterDirection.Output;

            var table = this.GetTable(comm);
            table.TableName = TableName.ProductVersions;

            if (totalItemsParam.Value != DBNull.Value)
            {
                totalItems = Convert.ToInt32(totalItemsParam.Value);
            }
            List<ProductVersions> versions = EntityBase.ParseListFromTable<ProductVersions>(table);
            return versions;
        }

        public void Update(ProductVersions @new, ProductVersions old)
        {
            var item = @new;
            item.VersionId = old.VersionId;
            DbCommand comm = this.GetCommand("Sp_ProductVersions_Update");
            if (comm == null) return;
            comm.AddParameter<int>(this.Factory, "VersionId", item.VersionId);
            comm.AddParameter<string>(this.Factory, "VersionTitle", (item.VersionTitle != null && item.VersionTitle.Trim().Length > 0) ? item.VersionTitle.Trim() : null);
            comm.AddParameter<string>(this.Factory, "VersionCode", (item.VersionCode != null && item.VersionCode.Trim().Length > 0) ? item.VersionCode.Trim() : null);
            comm.AddParameter<int>(this.Factory, "VersionPrice", item.VersionPrice);
            comm.AddParameter<string>(this.Factory, "VersionDescription", (item.VersionDescription != null && item.VersionDescription.Trim().Length > 0) ? item.VersionDescription.Trim() : null);
            comm.AddParameter<int>(this.Factory, "RegistrationFeeHN", item.RegistrationFeeHN);
            comm.AddParameter<int>(this.Factory, "LicenseFeeHN", item.LicenseFeeHN);
            comm.AddParameter<int>(this.Factory, "RoadTaxHN", item.RoadTaxHN);
            comm.AddParameter<int>(this.Factory, "InsuranceFeeHN", item.InsuranceFeeHN);
            comm.AddParameter<int>(this.Factory, "RegistrationFeeHCM", item.RegistrationFeeHCM);
            comm.AddParameter<int>(this.Factory, "LicenseFeeHCM", item.LicenseFeeHCM);
            comm.AddParameter<int>(this.Factory, "CertificateFeeHCM", item.CertificateFeeHCM);
            comm.AddParameter<int>(this.Factory, "RoadTaxHCM", item.RoadTaxHCM);
            comm.AddParameter<int>(this.Factory, "InsuranceFeeHCM", item.InsuranceFeeHCM);
            comm.AddParameter<int>(this.Factory, "RegistrationFeeKHAC", item.RegistrationFeeKHAC);
            comm.AddParameter<int>(this.Factory, "LicenseFeeKHAC", item.LicenseFeeKHAC);
            comm.AddParameter<int>(this.Factory, "RoadTaxKHAC", item.RoadTaxKHAC);
            comm.AddParameter<int>(this.Factory, "InsuranceFeeKHAC", item.InsuranceFeeKHAC);
            comm.AddParameter<int>(this.Factory, "CertificateFeeKHAC", item.CertificateFeeKHAC);
            comm.AddParameter<int>(this.Factory, "CertificateFeeHN", item.CertificateFeeHN);
            comm.AddParameter<bool>(this.Factory, "IsRegister", item.IsRegister);
            comm.AddParameter<bool>(this.Factory, "IsActive", item.IsActive);
            comm.AddParameter<int>(this.Factory, "OrderNo", item.OrderNo);
            comm.AddParameter<string>(this.Factory, "Culture", (item.Culture != null && item.Culture.Trim().Length > 0) ? item.Culture.Trim() : null);

            this.SafeExecuteNonQuery(comm);
        }
    }
}
