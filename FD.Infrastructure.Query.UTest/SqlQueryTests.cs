using Microsoft.VisualStudio.TestTools.UnitTesting;
using FD.Infrastructure.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Query.Tests
{
    [TestClass()]
    public class SqlQueryTests
    {
        [TestMethod()]
        public void QueryAliaseTest()
        {
            ConnectionFactory.ConfigRegister("DbContext");
            ConnectionFactory.ConfigRegister("FCJDbContext");

            var query1 = new SqlQuery("DbContext");
            query1.Query("select count(*) from t_il_land");
            var query2 = new SqlQuery("FCJDbContext");
            query2.Query("select count(*) from m_room");

        }


        [TestMethod()]
        public void QueryAliaseHelperTest()
        {
            ConnectionFactory.ConfigRegister("DbContext");
            ConnectionFactory.ConfigRegister("FCJDbContext");

            var query1 = new LocalHelper();
            query1.Query("select count(*) from t_il_land");
            var query2 = new FCJHelper();
            query2.Query("select count(*) from m_room");

        }

        [TestMethod()]
        public void GetPageListTest()
        {
            ConnectionFactory.ConfigRegister("DbContext");
            var query  = new SqlQuery("DbContext");
            var sql = @"select * from T_FCJ_HOUSE_SALE";
            var data = query.QueryList(sql, 1, 10);
            Assert.AreEqual(data.data.Count, 10);
            //Console.WriteLine(qb.SQL);

        }
    }
}