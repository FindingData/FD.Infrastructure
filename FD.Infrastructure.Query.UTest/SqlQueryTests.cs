﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void QueryListTest()
        {
            var sql = new SqlQuery("LD_DBContext");
        }
    }
}