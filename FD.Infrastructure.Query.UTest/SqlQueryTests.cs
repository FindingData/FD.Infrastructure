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

        [TestInitialize]
        public void Initialize()
        {
            ConnectionFactory.ConfigRegister("DbContext");
            ConnectionFactory.ConfigRegister("OaDbContext");
        }

        [TestMethod()]
        public void MultiTest()
        {
            var sql = new SqlQuery("OaDbContext");
            bool result;
            try
            {
               sql.DBConnection.Open();
               
               result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            Assert.IsTrue(result);
        }
    }
}