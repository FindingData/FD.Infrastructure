using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository.Interface
{
    public interface IConnectionFactory
    {
        IDbConnection GetDbConnection();
    }
}
