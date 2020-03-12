using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository.Interface
{
    public interface IQueryExecutor
    { 
        int Execute(string sql, dynamic parms = null);

        dynamic Query(string sql,dynamic parms=null);
 
        IEnumerable<dynamic> QueryList(string sql, object param = null);

        IPage<dynamic> QueryList(string sql, int pageNum, int pageSize, string whereString = null, object param = null, object order = null, bool asc = false);

        IEnumerable<dynamic> QueryListProc(string sp, object param = null);

        IPage<dynamic> QueryListProc(string sp, int pageNum = 0, int pageSize = 0, object param = null);

        ISqlBuilder GetPageList(string sql, int pageNum = 0, int pageSize = 0, string whereString = null, object param = null, object order = null, bool asc = false);
    }
}
