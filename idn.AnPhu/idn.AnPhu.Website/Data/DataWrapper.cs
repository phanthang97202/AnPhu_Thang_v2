using Client.Core.Data.Entities;
using Client.Core.Data.Entities.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace idn.AnPhu.Website.Data
{
    public delegate T DataWrapperParseRow<T>(DataRow dr, out Dictionary<string, string> errors);
    public delegate bool DataWrapperFinalCheck<T>(DataTable dt, List<T> itemList, ref DataWrapperErrorDic errors);


    public class DataWrapperErrorDic
    {
        private Dictionary<int, Dictionary<string, string>> dic;


        public void SetError(int rowNo, string field, string errorMessage)
        {
            if (dic == null) dic = new Dictionary<int, Dictionary<string, string>>();
            if (!dic.ContainsKey(rowNo))
            {

                dic[rowNo] = new Dictionary<string, string>();

            }

            dic[rowNo][field] = errorMessage;
        }


        public void SetError(int rowNo, Dictionary<string, string> errors)
        {
            if (dic == null) dic = new Dictionary<int, Dictionary<string, string>>();
            if (!dic.ContainsKey(rowNo))
            {

                dic[rowNo] = errors;

            }
            else
            {
                foreach (var er in errors)
                {
                    dic[rowNo][er.Key] = er.Value;
                }
            }


        }


        public bool HasError(int rowNo)
        {
            return dic != null && dic.ContainsKey(rowNo);
        }

        public bool HasError(int rowNo, string field)
        {
            return (dic != null && dic.ContainsKey(rowNo) && dic[rowNo].ContainsKey(field));
        }
        public string GetErrorMessage(int rowNo, string field)
        {
            if (HasError(rowNo, field))
                return dic[rowNo][field];
            return null;
        }


        public int ErrorCount
        {
            get
            {
                return dic != null ? dic.Count : 0;
            }
        }


    }


    public class DataWrapper<T> where T : EntityBase
    {

        public DataTable DataSource
        {
            get;
            set;
        }

        public List<T> DataList
        {
            get;
            private set;
        }



        private DataTable ConstructDataTable(DataTable dt, Dictionary<string, string> dicColumnNames)
        {


            DataTable dtRet = new DataTable();

            foreach (var col in dicColumnNames.Keys)
            {
                var nname = dicColumnNames[col];
                dtRet.Columns.Add(nname, dt.Columns[col].DataType);
            }

            foreach (DataRow dr in dt.Rows)
            {
                DataRow nDr = dtRet.NewRow();

                foreach (var col in dicColumnNames.Keys)
                {
                    nDr[dicColumnNames[col]] = dr[col];
                }

                dtRet.Rows.Add(nDr);
            }


            return dtRet;



        }

        public DataTable RenameColumnTable(DataTable dt, Dictionary<string, string> dicColumnNames)
        {

            if (dt != null && dt.Columns.Count > 0)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    var colName = col.ColumnName;
                    foreach (var dicKey in dicColumnNames)
                    {
                        if (colName == dicKey.Key)
                        {
                            dt.Columns[colName].ColumnName = dicKey.Value;
                            break;
                        }
                    }
                }
            }

            return dt;



        }

        public DataWrapperErrorDic ValidationErrors
        {
            get;
            private set;
        }

        public bool HasError
        {
            get;
            private set;
        }

        /// <summary>
        /// default binding
        /// </summary>
        /// <param name="columnMap"></param>
        /// <param name="defaultValues"></param>
        public void BindData(Dictionary<string, string> columnMap, Dictionary<string, object> defaultValues, DataWrapperFinalCheck<T> funcFinalCheck = null)
        {
            this.ValidationErrors = new DataWrapperErrorDic();
            var dt = ConstructDataTable(this.DataSource, columnMap);
            var _list = EntityBase.ParseListFromTable<T>(dt);
            this.DataList = new List<T>();
            this.DataList = _list;

            for (int i = 0; i < this.DataList.Count; ++i)
            {
                var item = this.DataList[i];


                try
                {
                    if (defaultValues != null)
                        foreach (var d in defaultValues)
                        {
                            item.SetValue(d.Key, d.Value);
                        }

                    item.ValidateFields();


                }
                catch (ValidationException ex)
                {
                    this.HasError = true;



                    foreach (var error in ex.ValidationErrors)
                    {
                        var colName = error.FieldName;


                        foreach (var col in columnMap)
                        {
                            if (col.Value.Equals(colName))
                            {
                                colName = col.Key;
                            }
                        }

                        this.ValidationErrors.SetError(i, colName, error.Type.ToString());
                    }





                }

                //unique

                foreach (var col in columnMap)
                {
                    var colName = col.Key;
                    var propName = col.Value;
                    if (item.HasAttribute(propName, typeof(UniqueAttribute)))
                    {
                        var propValue = item.GetValue(propName);
                        if (propValue != null)
                            for (int j = 0; j < i; ++j)
                            {
                                if (propValue.Equals(DataList[j].GetValue(propName)))
                                {
                                    this.HasError = true;

                                    this.ValidationErrors.SetError(i, colName, "Unique");


                                }
                            }

                    }
                }




            }

            if (funcFinalCheck != null)
            {
                var errList = this.ValidationErrors;
                if (funcFinalCheck(DataSource, DataList, ref errList) == true) this.HasError = true;
                this.ValidationErrors = errList;
            }
        }


        /// <summary>
        /// custom binding
        /// </summary>
        /// <param name="funcParseRow"></param>
        public void BindData(DataWrapperParseRow<T> funcParseRow, DataWrapperFinalCheck<T> funcFinalCheck = null)
        {
            this.ValidationErrors = new DataWrapperErrorDic();
            this.DataList = new List<T>();
            for (int i = 0; i < DataSource.Rows.Count; ++i)
            {
                DataRow dr = DataSource.Rows[i];

                Dictionary<string, string> errorDic = null;
                T item = funcParseRow(dr, out errorDic) as T;


                if (item != null) this.DataList.Add(item);

                if (errorDic != null && errorDic.Count > 0)
                {
                    this.HasError = true;

                    this.ValidationErrors.SetError(i, errorDic);
                }

            }

            if (funcFinalCheck != null)
            {
                var errList = this.ValidationErrors;
                if (funcFinalCheck(DataSource, this.DataList, ref errList) == true) this.HasError = true;
                this.ValidationErrors = errList;
            }
        }


        #region utils

        #endregion
    }
}