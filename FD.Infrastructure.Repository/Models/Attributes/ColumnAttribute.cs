using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 别名|column name
        /// </summary>
        /// <param name="columnName"></param>
        public ColumnAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }
        /// <summary>
        /// ColumnName
        /// </summary>
        public string ColumnName { get; set; }
    }
}
