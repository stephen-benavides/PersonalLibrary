using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using PersonalLibrary.Data;
using PersonalLibrary.Models;

namespace PersonalLibrary.Tests.TempDataBase
{
    [TestClass]
    [Ignore("SqlServerHelper SERVER HELPER TEST CHECK IS UPDATED")]
    public class SqlServerHelperTests
    {
        private SqlServerHelper sqlServerHelper = new SqlServerHelper();

        [TestMethod]
        public void ExecuteQuery_ShouldReturnAStringWithInformationFromDB()
        {
            Hashtable parameterList = new Hashtable();
            parameterList.Add("@GenreId", 4);
            var dataSet = sqlServerHelper.ExecuteQuery("SELECT * FROM Customers WHERE GenreId = @GenreId", parameterList);

            Debug.WriteLine(dataSet.Tables[0].Rows[0][1].ToString());

        }
    }
}
