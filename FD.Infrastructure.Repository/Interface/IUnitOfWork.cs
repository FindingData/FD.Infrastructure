using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commit
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback
        /// </summary>
        void Rollback();

        /// <summary>
        /// Get Repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> GetRepository<T>() where T : class, IEntity;
        
    }
}
