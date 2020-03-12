using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository
{
    public class Paging<T> : IPage<T>
    {
        /// <summary>
        /// All data rows
        /// </summary>
        public int data_count { get; set; }
        /// <summary>
        /// 总页数|pageCount
        /// </summary>
        public int page_count => (int)System.Math.Ceiling((decimal)this.data_count / page_size);

        /// <summary>
        /// 当前页码|page number
        /// </summary>
        public int page_no { get; set; }

        /// <summary>
        /// 每页显示记录数|page size
        /// </summary>
        public int page_size { get; set; }

        /// <summary>
        /// 当前分页数据|data list
        /// </summary>
        public List<T> data { get; set; }

        /// <summary>
        /// 分页数据|
        /// paging data
        /// </summary>
        public Paging()
        {
            data = new List<T>();
        }

        /// <summary>
        /// 分页数据|
        /// paging data
        /// </summary>
        /// <param name="pageNo">page number</param>
        /// <param name="pageSize">page size</param>
        public Paging(int pageNo, int pageSize) : this()
        {
            this.page_no = pageNo;
            this.page_size = pageSize;
        }
    }
}
