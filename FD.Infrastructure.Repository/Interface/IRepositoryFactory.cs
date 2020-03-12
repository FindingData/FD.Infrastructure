using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository.Interface
{
    /// <summary>
    /// 定义IRepository接口|The interface for repository's factory  
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Get Repository
        /// </summary>
        IRepository<T> GetRepository<T>() where T : class, IEntity;
    }
}
