using FD.Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository
{
    /// <summary>
    /// 工作单元基类|
    /// Base UnitOfWork
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IRepositoryFactory
    {
        private Dictionary<Type, object> repositories;
        private readonly IDbConnection context;
        private readonly IDbTransaction transaction;

        #region Disposed
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //clear repo
                    if (repositories != null)
                    {
                        repositories.Clear();
                    }
                    if (context.State != ConnectionState.Closed)
                        context.Close();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// 工作单元基类|
        /// Base UnitOfWork
        /// </summary>
        public UnitOfWork(IDbConnection context)
        {
            this.context = context;
            if (this.context.State == ConnectionState.Closed && this.context.State != ConnectionState.Connecting)
            {
                this.context.Open();
            }
            transaction = this.context.BeginTransaction();
        }

        /// <summary>
        /// 工作单元基类|
        /// Base UnitOfWork
        /// </summary>
        public UnitOfWork()
        {
            this.context = ConnectionFactory.GetDbConnection();
            if (this.context.State == ConnectionState.Closed && this.context.State != ConnectionState.Connecting)
            {
                this.context.Open();
            }
            this.transaction = this.context.BeginTransaction();
        }

        /// <summary>
        /// 提交事务|
        /// transaction commit
        /// </summary>
        public void Commit()
        {
            this.transaction.Commit();
        }

        /// <summary>
        /// 事务回滚|
        /// transaction rollback
        /// </summary>
        public void Rollback()
        {
            this.transaction.Rollback();
        }
       

        /// <summary>
        /// 获取仓储|
        /// Get a repository
        /// </summary> 
        public IRepository<T> GetRepository<T>() where T : class, IEntity
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }
            var type = typeof(T);
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context, transaction);
                repositories.Add(type, repositoryInstance);
            }
            return (BaseRepository<T>)repositories[type];
        }

    }
}
