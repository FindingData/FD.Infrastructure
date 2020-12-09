using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Query
{
    public class FCJHelper:SqlQuery
    {
        protected override string dbAliase { get; set; } = "FCJDbContext";
    }
}
