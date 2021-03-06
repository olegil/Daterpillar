﻿using Gigobyte.Daterpillar.Transformation;
using Gigobyte.Daterpillar.Transformation.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Daterpillar.UnitTest
{
    [TestClass]
    [DeploymentItem(SampleData.DataTypesCSV)]
    public class SqlTypeNameResolverTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [Owner(Dev.Ackara)]
        [DataSource(SampleData.CsvDataProvider, SampleData.DataTypesConnStr, SampleData.DataTypesTable, DataAccessMethod.Sequential)]
        public void GetName_should_return_a_valid_mssql_type_when_a_data_type_is_passed()
        {
            // Arrange
            var dataType = new DataType(typeName: Convert.ToString(TestContext.DataRow["Type"]));
            var expected = Convert.ToString(TestContext.DataRow["T-SQL"]);
            var sut = new SqlTypeNameResolver();

            // Act
            var result = sut.GetName(dataType);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}