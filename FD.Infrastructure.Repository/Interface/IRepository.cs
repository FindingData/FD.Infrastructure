using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository.Interface
{
    public interface IRepository<T> where T:class,IEntity
    {
        /// <summary>
        /// 表名|
        /// To get the name of the table
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// DBConnection
        /// </summary>
        IDbConnection DBConnection { get; }

        IDbTransaction DBTransaction { get; set; }

        /// <summary>
        /// 开启事务|
        /// Open transaction
        /// </summary>
        IDbTransaction OpenTransaction();

        #region Sync
        /// <summary>
        /// 插入实体|
        /// Inserts an entity into table "Ts" and returns identity id or number of inserted rows if inserting a list.
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回自增Id|Identity of inserted entity.</returns>
        long Insert(T entity);

        /// <summary>
        /// 插入实体列表
        /// |Inserts an entity into table "Ts" and returns identity id or number of inserted rows if inserting a list.
        /// </summary>
        /// <param name="entityList">entity list</param>
        /// <returns>返回受影响行数|number of inserted rows if inserting a list.</returns>
        long Insert(IEnumerable<T> entityList);

        /// <summary>
        /// 更新|
        /// Updates entity in table "Ts", checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        bool Update(T entity);

        /// <summary>
        /// 更新部分|
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        bool UpdatePart(dynamic data);

        /// <summary>
        /// 删除实体|
        /// Delete entity in table "Ts".
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>true if deleted, false if not found</returns>
        bool Delete(T entity);

        /// <summary>
        /// 删除|
        /// Delete data in table "Ts".
        /// </summary>
        /// <param name="whereString">parameterized sql of "where",(example:whereString:name like @name)</param>
        /// <param name="param">whereString's param，(example:new { name = "google%" })</param>
        /// <returns>受影响的行数|The number of rows affected.</returns>
        bool Delete(string whereString, object param);

        /// <summary>
        /// 删除全部|
        /// Delete all data
        /// </summary>
        bool DeleteAll();

        /// <summary>
        /// 执行单条语句
        /// |Execute parameterized SQL.
        /// </summary>
        /// <param name="sql">parameterized SQL</param>
        /// <param name="parms">The parameters to use for this query.</param>
        /// <returns>受影响的行数|The number of rows affected.</returns>
        int Execute(string sql, dynamic parms = null);

        /// <summary>
        /// 查询单个实体|
        /// Returns a single entity by a single id from table "Ts".  
        /// Id must be marked with [Key]/[ExplicitKey] attribute.
        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
        /// for optimal performance. 
        /// </summary>
        /// <param name="id">Id of the entity to get, must be marked with [Key]/[ExplicitKey] attribute</param>
        /// <returns>Entity of T</returns>
        T Query(object id);

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereString"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QueryFirst(string whereString = null, object param = null);

        /// <summary>
        /// 查询总数|
        /// Returns the number of rows
        /// </summary>
        /// <param name="whereString">where sql</param>
        /// <param name="param">param</param>
        /// <returns>number of rows</returns>
        int QueryCount(string whereString = null, object param = null);

        /// <summary>
        /// 查询列表|
        /// Executes a query, returning the data typed as T.
        /// </summary>
        /// <param name="whereString">whereString,(example:whereString:name like @name)</param>
        /// <param name="param">whereString's param，(example:new { name = "google%" })</param> 
        /// <param name="order">order param,(example:order:"createTime")</param>
        /// <param name="asc">Is ascending</param>
        /// <returns>returning the data list typed as T.</returns>
        List<T> QueryList(string whereString = null, object param = null, string order = null, bool asc = false);

        /// <summary>
        /// 分页查询|
        /// Executes a query, returning the paging data typed as T.
        /// </summary>
        /// <param name="pageNum">页码|page number</param>
        /// <param name="pageSize">页大小|page size</param>
        /// <param name="whereString">parameterized sql of "where",(example:whereString:name like @name)</param>
        /// <param name="param">whereString's param，(example:new { name = "google%" })</param>
        /// <param name="order">order param,(example:order:"createTime")</param>
        /// <param name="asc">Is ascending</param>
        /// <returns>返回分页数据|returning the paging data typed as T</returns>
        IPage<T> QueryList(int pageNum, int pageSize, string whereString = null, object param = null, string order = null, bool asc = false);

        #endregion
    }
}
