using Dapper;
using Dapper.Oracle;
using FD.Infrastructure.Query.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Query
{
    public class SqlQuery : ISqlQuery
    {
        public SqlQuery() { }

        public SqlQuery(string dbAliase = "")
        {
            this.dbAliase = dbAliase;
        }

        protected virtual string dbAliase { get; set; } 

        private IDbConnection _dbConnection;
        /// <summary>
        /// IDbConnection
        /// </summary>
        public IDbConnection DBConnection
        {
            get
            {
                if (_dbConnection == null)
                {
                    _dbConnection = ConnectionFactory.GetDbConnection(dbAliase);
                }              
                return _dbConnection;
            }
            private set { this._dbConnection = value; }
        }        
     
        public int Execute(string sql, dynamic parms = null)
        {
            return DBConnection.Execute(sql, (object)parms, null);
        }

        public int ExecuteProc(string sp, dynamic parms = null)
        {
            return DBConnection.Execute(sp, (object)parms, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<dynamic> QueryList(string sql, object param = null)
        {
            return DBConnection.Query<dynamic>(sql, param);
        }

        public IPage<dynamic> QueryList(string sql, int pageNum, int pageSize, string whereString = null, object param = null, object order = null, bool asc = false)
        {            
            IPage<dynamic> paging = new Paging<dynamic>(pageNum, pageSize);
            var sqlBuilder = GetPageList(sql, pageNum, pageSize, whereString, param, order, asc);
            paging.data = DBConnection.Query<dynamic>(sqlBuilder.SQL, sqlBuilder.Arguments).ToList();
            paging.data_count = QueryCount(sql, whereString, param);
            return paging;
        }

        public int QueryCount(string sql,string whereString = null,object param = null)
        {
            SqlBuilder sbCount = new SqlBuilder();
            SqlBuilder sb = new SqlBuilder();
            sb.Append(sql);
            if (!string.IsNullOrEmpty(whereString))
            {
                sb.Where(whereString, param);
            }            
            sbCount.Select(args: "COUNT(*)");
            sbCount.Append("FROM (");
            sbCount.Append(sb.SQL, sb.Arguments);
            sbCount.Append(")");
            return DBConnection.QueryFirst<int>(sbCount.SQL, sbCount.Arguments);
        }

        public ISqlBuilder GetPageList(string sql, int pageNum = 0, int pageSize = 0, string whereString = null, object param = null, object order = null, bool asc = false)
        {
            SqlBuilder sqlBuilder = new SqlBuilder();

            SqlBuilder sqlBuilderRows = new SqlBuilder(sql);                       
           
            if (!string.IsNullOrEmpty(whereString))
            {             
                 sqlBuilderRows.Where(whereString, param);
            }

            if (order != null)
            {
                sqlBuilderRows.OrderBy(order);
                sqlBuilderRows.IsAse(asc);
            }

            if (pageSize > 0)
            {               
                if (pageNum <= 0)
                    pageNum = 1;
                int numMin = (pageNum - 1) * pageSize + 1,
                numMax = pageNum * pageSize;

                if (!string.IsNullOrEmpty(whereString) && whereString.Contains(":"))
                {
                    sqlBuilder.Append($"with y as (select rownum rid,tb.* from ({sqlBuilderRows.SQL}) tb) select * from y", sqlBuilderRows.Arguments);
                    sqlBuilder.Where("rid between :numMin and :numMax", new { numMin, numMax });
                }
                else
                {
                    sqlBuilder.Append($"select * from (select rownum rid,tb.* from ({sqlBuilderRows.SQL}) tb where ROWNUM<={numMax}) where rid>={numMin}", sqlBuilderRows.Arguments);
                }                
            }
            else
            {
                sqlBuilder.Append($"with y as (select rownum rid,tb.* from ({sqlBuilderRows.SQL}) tb) select * from y", sqlBuilderRows.Arguments);
            }
            return sqlBuilder;
        }

        private static Dictionary<string, object> EntityToDictionary(object ety)
        {
            Type type = ety.GetType();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo item in type.GetProperties())
            {
                dict.Add(item.Name, item.GetValue(ety, null));
            }
            return dict;
        }

        public dynamic Query(string sql, dynamic parms = null)
        {            
            return DBConnection.QueryFirstOrDefault<dynamic>(sql, (object)parms);
        }

        public IEnumerable<dynamic> QueryListProc(string sp, object param = null)
        {
            var dParams = new OracleDynamicParameters(param);
            dParams.Add("cur_result", null, OracleMappingType.RefCursor, ParameterDirection.Output);                           
            return DBConnection.Query(sp, dParams, commandType: CommandType.StoredProcedure).ToList();
        }



        public IPage<dynamic> QueryListProc(string sp, int pageNum = 0, int pageSize = 0, object param = null)
        {
            var dParams = new OracleDynamicParameters(param);
            dParams.Add("i_page_index", pageNum);
            dParams.Add("i_page_size", pageSize);
            dParams.Add("o_total", 0, OracleMappingType.Int32, ParameterDirection.Output);
            dParams.Add("cur_result", null, OracleMappingType.RefCursor, ParameterDirection.Output);

            var paging = new Paging<dynamic>(pageNum,pageSize);
            paging.data = DBConnection.Query(sp, dParams, commandType: CommandType.StoredProcedure).ToList();
            paging.data_count = (int)dParams.Get<decimal>("o_total");
            return paging;
        }
    }
}