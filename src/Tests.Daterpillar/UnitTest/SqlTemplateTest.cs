﻿using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using Gigobyte.Daterpillar.Transformation;
using Gigobyte.Daterpillar.Transformation.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace Tests.Daterpillar.UnitTest
{
    [TestClass]
    [UseApprovalSubdirectory(nameof(ApprovalTests))]
    [UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
    public class SqlTemplateTest
    {
        [ClassCleanup]
        public static void Cleanup()
        {
            ApprovalTests.Maintenance.ApprovalMaintenance.CleanUpAbandonedFiles();
        }

        [TestMethod]
        [Owner(Dev.Ackara)]
        public void Transform_should_generate_a_mssql_schema_when_all_template_settings_are_enabled()
        {
            // Arrange
            var settings = new SqlTemplateSettings()
            {
                AddScript = true,
                UseDatabase = true,
                CreateSchema = true,
                CommentsEnabled = true,
                DropDatabaseIfExist = true
            };

            RunTemplateTest(settings);
        }

        [TestMethod]
        [Owner(Dev.Ackara)]
        public void Transform_should_generate_a_mssql_schema_when_all_template_settings_are_disabled()
        {
            // Arrange
            var settings = new SqlTemplateSettings()
            {
                AddScript = false,
                UseDatabase = false,
                CreateSchema = false,
                CommentsEnabled = false,
                DropDatabaseIfExist = false,
            };

            RunTemplateTest(settings);
        }

        [TestMethod]
        [Owner(Dev.Ackara)]
        public void Transform_should_assign_the_tsql_schema_index_tableName_property_when_null()
        {
            // Arrange
            var sut = new SqlTemplate(SqlTemplateSettings.Default, Mock.Create<ITypeNameResolver>());
            var schema = SampleData.CreateSchema();
            schema.Tables[0].Indexes[0].Table = "";

            // Act
            var script = sut.Transform(schema);

            // Assert
            Approvals.Verify(script);
        }

        private void RunTemplateTest(SqlTemplateSettings settings, [CallerMemberName]string name = null)
        {
            // Arrange
            var schema = SampleData.CreateSchema(name);
            var managerTable = SampleData.CreateTableSchema("Manager");
            managerTable.Comment = "this is a comment";
            schema.Tables.Add(managerTable);

            var mockTypeResolver = Mock.Create<ITypeNameResolver>();
            mockTypeResolver.Arrange(x => x.GetName(Arg.IsAny<DataType>()))
                .Returns("int")
                .OccursAtLeast(1);

            var sut = new SqlTemplate(settings, mockTypeResolver);

            // Act
            var script = sut.Transform(schema);

            // Assert
            mockTypeResolver.AssertAll();
            Approvals.Verify(script);
        }
    }
}