using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository.Interface
{
    /// <summary>
    /// The interface for all SqlBuilder operations.
    /// </summary>
    public interface ISqlBuilder
    {
        /// <summary>
        /// SQL
        /// </summary>
        string SQL { get; }

        /// <summary>
        /// args
        /// </summary>
        object Arguments { get; }
    }
}
