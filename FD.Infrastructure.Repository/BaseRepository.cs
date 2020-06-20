using Dapper;
using FD.Infrastructure.Repository.Extension;
using FD.Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {

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

      

        public string TableName
        {
            get
            { 
                return SqlMapperExtensions.GetTableName(typeof(T));
            }
        }
       

        private IDbTransaction _dbTransaction;

        public IDbTransaction DBTransaction
        {
            get { return _dbTransaction; }
            set { _dbTransaction = value; }
        }

          /// <summary>
        /// 事务状态|
        /// transaction's state
        /// </summary>
       // public ETrancationState TrancationState { get; private set; } = ETrancationState.Closed;
        /// <summary>
        /// 开启事务|
        /// Open transaction
        /// </summary>
        public IDbTransaction OpenTransaction()
        {
            _dbTransaction = DBConnection.BeginTransaction();
            //TrancationState = ETrancationState.Opened;
            return _dbTransaction;
        }

        public BaseRepository()
        {            
        }

        /// <summary>
        /// 仓储基类| Base Repository
        /// </summary>
        public BaseRepository(string dbAliase = "")
        {
            this.dbAliase = dbAliase;
        }

        protected virtual string dbAliase { get; set; }

        public BaseRepository(IDbConnection dbConnection, IDbTransaction dbTransaction = null)
        {
            this.DBConnection = dbConnection;
            this.DBTransaction = dbTransaction;
        }


        public bool Delete(T entity)
        {            
            return DBConnection.Delete(entity);
        }

        public bool Delete(string whereString, object param)
        {
            SqlBuilder sb = new SqlBuilder();
            sb.Append("DELETE FROM " + TableName);
            sb.Where(whereString, param);
            return Execute(sb.SQL, sb.Arguments) > 0;
        }

        public bool DeleteAll()
        {
            return DBConnection.DeleteAll<T>(_dbTransaction);
        }

        public int Execute(string sql, dynamic parms = null)
        {
            return DBConnection.Execute(sql, (object)parms, _dbTransaction);
        }

        public long Insert(T entity)
        {
            return DBConnection.Insert(entity, _dbTransaction);
        }

        public long Insert(IEnumerable<T> entityList)
        {
            return DBConnection.Insert(entityList, _dbTransaction);
        }

        public T Query(object id)
        {
            return DBConnection.Get<T>(id);
        }

        public T QueryFirst(string whereString = null, object param = null)
        {
            SqlBuilder sb = new SqlBuilder();
            sb.Select(args: "*");
            sb.From(TableName);
            if (!string.IsNullOrEmpty(whereString))
            {
                sb.Where(whereString, param);
            }
            return DBConnection.QueryFirstOrDefault<T>(sb.SQL, sb.Arguments);
        }

        public int QueryCount(string whereString = null, object param = null)
        {
            SqlBuilder sb = new SqlBuilder();
            sb.Select(args: "Count(*)");
            sb.From(TableName);
            if (!string.IsNullOrEmpty(whereString))
            {
                sb.Where(whereString, param);
            }
            return DBConnection.QueryFirst<int>(sb.SQL, sb.Arguments);
        }

        public List<T> QueryList(string whereString = null, object param = null, string order = null, bool asc = false)
        {
            if (string.IsNullOrEmpty(whereString) && string.IsNullOrEmpty(order))
                return DBConnection.GetAll<T>().ToList();
            else
            {
                ISqlAdapter adapter = SqlMapperExtensions.GetFormatter(DBConnection);
                var sqlBuilder = adapter.GetPageList(this, whereString: whereString, param: param, order: order, asc: asc);
                return DBConnection.Query<T>(sqlBuilder.SQL, sqlBuilder.Arguments).ToList();
            }
           
        }

        public IPage<T> QueryList(int pageNum, int pageSize, string whereString = null, object param = null, string order = null, bool asc = false)
        {
            IPage<T> paging = new Paging<T>(pageNum, pageSize);
            ISqlAdapter adapter = SqlMapperExtensions.GetFormatter(DBConnection);
            var sqlBuilder = adapter.GetPageList(this, pageNum, pageSize, whereString, param, order, asc);
            paging.data = DBConnection.Query<T>(sqlBuilder.SQL, sqlBuilder.Arguments).ToList();
            return paging;
        }

        public bool Update(T entity)
        {            
            return DBConnection.Update(entity, _dbTransaction);
        }

        

        /// <summary>
        /// Update a record in the database.
        /// </summary>
        /// <param name="id">The primary key of the row to update.</param>
        /// <param name="data">The new object data.</param>
        /// <returns>The number of rows affected.</returns>
        public bool UpdatePart(dynamic data)
        {
            Dictionary<string,object> dir = EntityToDictionary(data);

            var type = typeof(T);
            var key = SqlMapperExtensions.KeyPropertiesCache(type).FirstOrDefault();

            var builder = new StringBuilder();
            builder.Append("update ").Append(TableName).Append(" set ");
            builder.AppendLine(string.Join(",", dir.Where(n => n.Key != key.Name && dir[n.Key] != null).Select(p => p.Key + "= :" + p.Key)));
            builder.Append($" where ");
            var adapter = SqlMapperExtensions.GetFormatter(DBConnection);
            adapter.AppendColumnNameEqualsValue(builder, SqlMapperExtensions.GetColumnName(key), key.Name); //fix for issue #336

            DynamicParameters parameters = new DynamicParameters(data);
            parameters.Add(key.Name, dir[key.Name]);

            var updated = DBConnection.Execute(builder.ToString(), parameters,_dbTransaction);
            return updated > 0;
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



       
    }
}
