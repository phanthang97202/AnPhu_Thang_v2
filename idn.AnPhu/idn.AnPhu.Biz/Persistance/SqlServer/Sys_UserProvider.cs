using Client.Core.Data.DataAccess;
using Client.Core.Data.Entities;
using Client.Core.Utils;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Persistance.Interface;
using idn.AnPhu.Constants;
using idn.AnPhu.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Persistance.SqlServer
{
    public class Sys_UserProvider : DataAccessBase, ISys_UserProvider
    {
        public Sys_User Get(Sys_User dummy)
        {
            DbCommand comm = this.GetCommand("Sp_Sys_User_GetByUserCode");

            comm.AddParameter<string>(this.Factory, "UserCode", dummy.UserCode);

            var table = this.GetTable(comm);
            table.TableName = TableName.Sys_User;
            if (table == null || table.Rows.Count == 0)
            {
                throw new SystemException("Lỗi nguy hiểm", new Exception());
            }
            return EntityBase.ParseListFromTable<Sys_User>(table).FirstOrDefault();
        }

        public List<Sys_User> GetAll(int startIndex, int count, ref int totalItems)
        {
            DbCommand comm = this.GetCommand("Sp_Sys_User_GetAll");

            //comm.AddParameter<int>(this.Factory, "StartIndex", startIndex);
            //comm.AddParameter<int>(this.Factory, "Count", count);

            //DbParameter totalItemsParam = comm.AddParameter(this.Factory, "totalItems", DbType.Int32, null);
            //totalItemsParam.Direction = ParameterDirection.Output;

            var table = this.GetTable(comm);
            table.TableName = TableName.Sys_User;

            //if (totalItemsParam.Value != DBNull.Value)
            //{
            //    totalItems = Convert.ToInt32(totalItemsParam.Value);
            //}
            return EntityBase.ParseListFromTable<Sys_User>(table);
        }

        public List<Sys_User> Search(string userCode, string fullName, string birthDayFrom, string birthDayTo, string phoneNo, string email, string sex, string flagActive, string isSysAdmin, int pageIndex, int pageCount, ref int totalItems)
        {
            DbCommand comm = this.GetCommand("Sp_Sys_User_Search");
            comm.AddParameter<string>(this.Factory, "userCode", (userCode != null && userCode.Trim().Length > 0) ? userCode : null);
            comm.AddParameter<string>(this.Factory, "fullName", (fullName != null && fullName.Trim().Length > 0) ? fullName.Trim() : null);
            comm.AddParameter<string>(this.Factory, "birthDayFrom", (birthDayFrom != null && birthDayFrom.Trim().Length > 0) ? birthDayFrom.Trim() : null);
            comm.AddParameter<string>(this.Factory, "birthDayTo", (birthDayTo != null && birthDayTo.Trim().Length > 0) ? birthDayTo.Trim() : null);
            comm.AddParameter<string>(this.Factory, "phoneNo", (phoneNo != null && phoneNo.Trim().Length > 0) ? phoneNo : null);
            comm.AddParameter<string>(this.Factory, "email", (email != null && email.Trim().Length > 0) ? email : null);
            comm.AddParameter<string>(this.Factory, "sex", (sex != null && sex.Trim().Length > 0) ? sex : null);
            comm.AddParameter<string>(this.Factory, "flagActive", flagActive);
            comm.AddParameter<string>(this.Factory, "isSysAdmin", isSysAdmin);
            comm.AddParameter<int>(this.Factory, "startIndex", pageIndex);
            comm.AddParameter<int>(this.Factory, "count", pageCount);

            DbParameter totalItemsParam = comm.AddParameter(this.Factory, "totalItems", DbType.Int32, null);
            totalItemsParam.Direction = ParameterDirection.Output;

            var table = this.GetTable(comm);
            table.TableName = TableName.Sys_User;

            if (totalItemsParam.Value != DBNull.Value)
            {
                totalItems = Convert.ToInt32(totalItemsParam.Value);
            }
            return EntityBase.ParseListFromTable<Sys_User>(table);
        }

        public void Add(Sys_User item)
        {
            var comm = this.GetCommand("Sp_Sys_User_Create");

            comm.AddParameter<string>(this.Factory, "userCode", (item != null && !CUtils.IsNullOrEmpty(item.UserCode)) ? CUtils.StrToUpper(item.UserCode) : null);
            comm.AddParameter<string>(this.Factory, "fullName", (item != null && !CUtils.IsNullOrEmpty(item.FullName)) ? CUtils.StrTrim(item.FullName) : null);
            comm.AddParameter<string>(this.Factory, "password", (item != null && !CUtils.IsNullOrEmpty(item.Password)) ? CUtils.StrTrim(item.Password) : null);
            comm.AddParameter<string>(this.Factory, "passwordSalt", (item != null && !CUtils.IsNullOrEmpty(item.PasswordSalt)) ? CUtils.StrTrim(item.PasswordSalt) : null);
            comm.AddParameter<string>(this.Factory, "phoneNo", (item != null && !CUtils.IsNullOrEmpty(item.PhoneNo)) ? CUtils.StrTrim(item.PhoneNo) : null);
            comm.AddParameter<string>(this.Factory, "email", (item != null && !CUtils.IsNullOrEmpty(item.Email)) ? CUtils.StrTrim(item.Email) : null);
            comm.AddParameter<string>(this.Factory, "sex", (item != null && !CUtils.IsNullOrEmpty(item.Sex)) ? CUtils.StrTrim(item.Sex) : null);
            comm.AddParameter<string>(this.Factory, "avatar", (item != null && !CUtils.IsNullOrEmpty(item.Avatar)) ? CUtils.StrTrim(item.Avatar) : null);
            //comm.AddParameter<DateTime>(this.Factory, "birthDay", (item != null && item.BirthDay != DateTime.MinValue && item.BirthDay.ToString(CultureInfo.InvariantCulture).Trim().Length > 0) ? item.BirthDay : DateTime.Now); //???
            comm.AddParameter<DateTime>(this.Factory, "birthDay", (item != null && CUtils.IsDateTime(item.BirthDay)) ? item.BirthDay : DateTime.MinValue);
            //comm.AddParameter<bool>(this.Factory, "flagActive", item != null && item.FlagActive); // đã mặc định trong StoredProcedure
            comm.AddParameter<bool>(this.Factory, "isSysAdmin", item != null && item.IsSysAdmin);
            comm.AddParameter<string>(this.Factory, "createBy", (item != null && !CUtils.IsNullOrEmpty(item.CreateBy)) ? CUtils.StrToUpper(item.CreateBy) : null);

            this.SafeExecuteNonQuery(comm);
        }

        public void Update(Sys_User @new, Sys_User old)
        {
            var item = @new;
            item.UserCode = old.UserCode;
            var comm = this.GetCommand("Sp_Sys_User_Update");
            if (comm == null) return;
            comm.AddParameter<string>(this.Factory, "userCode", (item != null && !CUtils.IsNullOrEmpty(item.UserCode)) ? CUtils.StrToUpper(item.UserCode) : null);
            comm.AddParameter<string>(this.Factory, "fullName", (item != null && !CUtils.IsNullOrEmpty(item.FullName)) ? CUtils.StrTrim(item.FullName) : null);
            comm.AddParameter<string>(this.Factory, "phoneNo", (item != null && !CUtils.IsNullOrEmpty(item.PhoneNo)) ? CUtils.StrTrim(item.PhoneNo) : null);
            comm.AddParameter<string>(this.Factory, "email", (item != null && !CUtils.IsNullOrEmpty(item.Email)) ? CUtils.StrTrim(item.Email) : null);
            comm.AddParameter<string>(this.Factory, "sex", (item != null && !CUtils.IsNullOrEmpty(item.Sex)) ? CUtils.StrTrim(item.Sex) : null);
            comm.AddParameter<string>(this.Factory, "avatar", (item != null && !CUtils.IsNullOrEmpty(item.Avatar)) ? CUtils.StrTrim(item.Avatar) : null);
            comm.AddParameter<DateTime>(this.Factory, "birthDay", (item != null && CUtils.IsDateTime(item.BirthDay)) ? item.BirthDay : DateTime.MinValue);
            comm.AddParameter<bool>(this.Factory, "flagActive", item != null && item.FlagActive);
            comm.AddParameter<bool>(this.Factory, "isSysAdmin", item != null && item.IsSysAdmin);
            comm.AddParameter<string>(this.Factory, "updateBy", (item != null && !CUtils.IsNullOrEmpty(item.CreateBy)) ? CUtils.StrToUpper(item.CreateBy) : null);

            this.SafeExecuteNonQuery(comm);
        }

        public void Remove(Sys_User item)
        {
            var comm = this.GetCommand("Sp_Sys_User_Delete");
            if (comm == null) return;
            comm.AddParameter<string>(this.Factory, "userCode", (item != null && !CUtils.IsNullOrEmpty(item.UserCode)) ? CUtils.StrToUpper(item.UserCode) : null);
            this.SafeExecuteNonQuery(comm);
        }

        public void Import(List<Sys_User> list, bool deleteExist)
        {


            DbCommand comm = this.GetCommandSQL("");
            DbTransaction trans = null;
            bool opened = false;

            try
            {
                StringBuilder sb = new StringBuilder();


                if (deleteExist)
                {
                    sb.Append("delete from [Sys_User];");
                }

                int i = 0;
                foreach (var item in list)
                {
                    item.FlagActive = true;
                    item.IsSysAdmin = false;

                    var salt = EncryptUtils.GenerateSalt();
                    var password = EncryptUtils.EncryptPassword(item.Password, salt);

                    item.Password = password.Trim();
                    item.PasswordSalt = salt.Trim();

                    item.ValidateFields();
                    sb.AppendFormat(@"
				        
				        BEGIN
                        Insert Into [Sign_In]
			            (
				            [UserCode],
				            [Password]
			            )
			            Values
			            (
				            @userCode_{0},
				            @password_{0}
			            )

                        -------------------------

				        Insert Into [Sys_User] 
				        (
			                [UserCode],
				            [UserName],
				            [FullName],
				            [PasswordSalt],
				            [PhoneNo],
				            [Email],
				            [Sex],
				            [FlagActive],
				            [IsSysAdmin],
				            [CreateDTime],
				            [CreateBy],
				            [Avatar],
				            [BirthDay]
				  
				         ) 
				         values(
			                @userCode_{0},
				            @userCode_{0}, -- UserName = UserCode
				            @fullName_{0},
				            @passwordSalt_{0},
				            @phoneNo_{0},
				            @email_{0},
				            @sex_{0},
				            1, -- Or 'TRUE' -- FlagActive
				            @isSysAdmin_{0},
				            GetDate(),
				            @createBy_{0},
				            @avatar_{0},
				            @birthDay_{0}
				  
				          )

				        END 
				        ", i);



                    comm.AddParameter<string>(this.Factory, string.Format("userCode_{0}", i), (item != null && !CUtils.IsNullOrEmpty(item.UserCode)) ? CUtils.StrToUpper(item.UserCode) : null);
                    comm.AddParameter<string>(this.Factory, string.Format("fullName_{0}", i), (item != null && !CUtils.IsNullOrEmpty(item.FullName)) ? CUtils.StrTrim(item.FullName) : null);
                    comm.AddParameter<string>(this.Factory, string.Format("password_{0}", i), (item != null && !CUtils.IsNullOrEmpty(item.Password)) ? CUtils.StrTrim(item.Password) : null);
                    comm.AddParameter<string>(this.Factory, string.Format("passwordSalt_{0}", i), (item != null && !CUtils.IsNullOrEmpty(item.PasswordSalt)) ? CUtils.StrTrim(item.PasswordSalt) : null);
                    comm.AddParameter<string>(this.Factory, string.Format("phoneNo_{0}", i), (item != null && !CUtils.IsNullOrEmpty(item.PhoneNo)) ? CUtils.StrTrim(item.PhoneNo) : null);
                    comm.AddParameter<string>(this.Factory, string.Format("email_{0}", i), (item != null && !CUtils.IsNullOrEmpty(item.Email)) ? CUtils.StrTrim(item.Email) : null);
                    comm.AddParameter<string>(this.Factory, string.Format("sex_{0}", i), (item != null && !CUtils.IsNullOrEmpty(item.Sex)) ? CUtils.StrTrim(item.Sex) : null);
                    comm.AddParameter<string>(this.Factory, string.Format("avatar_{0}", i), (item != null && !CUtils.IsNullOrEmpty(item.Avatar)) ? CUtils.StrTrim(item.Avatar) : null);
                    comm.AddParameter<DateTime>(this.Factory, string.Format("birthDay_{0}", i), (item != null && CUtils.IsDateTime(item.BirthDay)) ? item.BirthDay : DateTime.MinValue);
                    comm.AddParameter<bool>(this.Factory, string.Format("isSysAdmin_{0}", i), item != null && item.IsSysAdmin);
                    comm.AddParameter<string>(this.Factory, string.Format("createBy_{0}", i), (item != null && !CUtils.IsNullOrEmpty(item.CreateBy)) ? CUtils.StrToUpper(item.CreateBy) : null);

                    i++;


                }



                comm.Connection.Open();
                opened = true;
                trans = comm.Connection.BeginTransaction();
                comm.Transaction = trans;

                comm.CommandText = sb.ToString();
                comm.ExecuteNonQuery();


                trans.Commit();


            }
            catch (Exception ex)
            {
                if (opened)
                    trans.Rollback();


                throw ex;
            }

            finally
            {
                if (opened)
                    comm.Connection.Close();
            }

        }

        #region["Comment Demo Import Excel"]
        //        public void Import(List<Models.Airport> list, bool deleteExist)
        //        {


        //            DbCommand comm = this.GetCommandSQL("");
        //            DbTransaction trans = null;
        //            bool opened = false;

        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();


        //                if (deleteExist)
        //                {
        //                    sb.Append("delete from [Airport];");
        //                }

        //                int i = 0;
        //                foreach (var item in list)
        //                {

        //                    item.ValidateFields();
        //                    sb.AppendFormat(@"
        //				IF NOT EXISTS (SELECT * FROM [Airport]  Where 
        //				  [Id]=@Id_{0}		
        //
        //				)
        //				BEGIN
        //				insert into [Airport] 
        //				(
        //			      AirportCode,
        //			      CityCode,
        //			      CityName,
        //			      CountryCode,
        //			      CountryName,
        //			      CountryPriority,
        //			      StateCode,
        //			      AirportNameEN,
        //			      AirportNameVN,
        //			      SuggestNormal,
        //			      SuggestNormalVN,
        //			      SuggestMobile,
        //			      IsAutoSuggest,
        //			      IsMainAirport,
        //			      Active,
        //			      OrderDisplay
        //				  
        //				 ) 
        //				 values(
        //			      @AirportCode_{0},
        //			      @CityCode_{0},
        //			      @CityName_{0},
        //			      @CountryCode_{0},
        //			      @CountryName_{0},
        //			      @CountryPriority_{0},
        //			      @StateCode_{0},
        //			      @AirportNameEN_{0},
        //			      @AirportNameVN_{0},
        //			      @SuggestNormal_{0},
        //			      @SuggestNormalVN_{0},
        //			      @SuggestMobile_{0},
        //			      @IsAutoSuggest_{0},
        //			      @IsMainAirport_{0},
        //			      @Active_{0},
        //			      @OrderDisplay_{0}
        //				  
        //				  )
        //
        //				END 
        //				ELSE BEGIN
        //					update [Airport]  
        //					SET 
        //
        //					[Id]=@Id_{0},
        //				  [AirportCode]=@AirportCode_{0},
        //				  [CityCode]=@CityCode_{0},
        //				  [CityName]=@CityName_{0},
        //				  [CountryCode]=@CountryCode_{0},
        //				  [CountryName]=@CountryName_{0},
        //				  [CountryPriority]=@CountryPriority_{0},
        //				  [StateCode]=@StateCode_{0},
        //				  [AirportNameEN]=@AirportNameEN_{0},
        //				  [AirportNameVN]=@AirportNameVN_{0},
        //				  [SuggestNormal]=@SuggestNormal_{0},
        //				  [SuggestNormalVN]=@SuggestNormalVN_{0},
        //				  [SuggestMobile]=@SuggestMobile_{0},
        //				  [IsAutoSuggest]=@IsAutoSuggest_{0},
        //				  [IsMainAirport]=@IsMainAirport_{0},
        //				  [Active]=@Active_{0},
        //				  [OrderDisplay]=@OrderDisplay_{0}
        //							
        //					
        //					Where 
        //					[Id]=@Id_{0}	
        //				END;", i);



        //                    comm.AddParameter<string>(this.Factory, string.Format("AirportCode_{0}", i), item.AirportCode);
        //                    comm.AddParameter<string>(this.Factory, string.Format("CityCode_{0}", i), item.CityCode);
        //                    comm.AddParameter<string>(this.Factory, string.Format("CityName_{0}", i), item.CityName);
        //                    comm.AddParameter<string>(this.Factory, string.Format("CountryCode_{0}", i), item.CountryCode);
        //                    comm.AddParameter<string>(this.Factory, string.Format("CountryName_{0}", i), item.CountryName);
        //                    comm.AddParameter<int>(this.Factory, string.Format("CountryPriority_{0}", i), item.CountryPriority);
        //                    comm.AddParameter<string>(this.Factory, string.Format("StateCode_{0}", i), item.StateCode);
        //                    comm.AddParameter<string>(this.Factory, string.Format("AirportNameEN_{0}", i), item.AirportNameEN);
        //                    comm.AddParameter<string>(this.Factory, string.Format("AirportNameVN_{0}", i), item.AirportNameVN);
        //                    comm.AddParameter<string>(this.Factory, string.Format("SuggestNormal_{0}", i), item.SuggestNormal);
        //                    comm.AddParameter<string>(this.Factory, string.Format("SuggestNormalVN_{0}", i), item.SuggestNormalVN);
        //                    comm.AddParameter<string>(this.Factory, string.Format("SuggestMobile_{0}", i), item.SuggestMobile);
        //                    comm.AddParameter<bool>(this.Factory, string.Format("IsAutoSuggest_{0}", i), item.IsAutoSuggest);
        //                    comm.AddParameter<bool>(this.Factory, string.Format("IsMainAirport_{0}", i), item.IsMainAirport);
        //                    comm.AddParameter<bool>(this.Factory, string.Format("Active_{0}", i), item.Active);
        //                    comm.AddParameter<int>(this.Factory, string.Format("OrderDisplay_{0}", i), item.OrderDisplay);

        //                    i++;


        //                }



        //                comm.Connection.Open();
        //                opened = true;
        //                trans = comm.Connection.BeginTransaction();
        //                comm.Transaction = trans;

        //                comm.CommandText = sb.ToString();
        //                comm.ExecuteNonQuery();


        //                trans.Commit();


        //            }
        //            catch (Exception ex)
        //            {
        //                if (opened)
        //                    trans.Rollback();


        //                throw ex;
        //            }

        //            finally
        //            {
        //                if (opened)
        //                    comm.Connection.Close();
        //            }

        //        }
        #endregion

    }
}
