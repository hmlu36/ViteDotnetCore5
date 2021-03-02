using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
/*
using Dapper;
using GeoUVP.Models.Enums;
using GeoUVP.Extensions;
*/
using System.IO;

namespace ViteDotnetCore5.Utils {
    public class SqlUtils {
        /*
        public static SqlConnection GetSqlConnection() {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                                       .SetBasePath(Directory.GetCurrentDirectory())
                                                       .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                                                       .Build();
            return new SqlConnection(configuration.GetConnectionString("GeoUVP"));
        }
       
        
        public static IEnumerable<dynamic> Query(string sql, object parameters = null) {
            using (var conn = GetSqlConnection()) {
                return conn.Query(sql, parameters);
            }
        }
        
        public static T QuerySingle<T>(string sql, object parameters = null) {
            using (var conn = GetSqlConnection()) {
                return conn.QuerySingle<T>(sql, parameters);
            }
        }

        public static T ExecuteScalar<T>(string sql, object parameters = null) {
            using (var conn = GetSqlConnection()) {
                return conn.ExecuteScalar<T>(sql, parameters);
            }
        }

        // 取得table所有欄位
        public static IEnumerable<string> GetTableColumns(string table) {
            using (var conn = GetSqlConnection()) {
                return conn.Query<string>($@"SELECT Column_Name
                                               FROM INFORMATION_SCHEMA.COLUMNS
                                              WHERE TABLE_NAME = N'{table}'");
            }

        }

        // 取得sequence值
        public static int GetNextSequenceValue(SequencePrefixEnums prefix) {
            using (var conn = GetSqlConnection()) {
                return conn.ExecuteScalar<int>($@"SELECT NEXT VALUE FOR {prefix.GetDescription()}");
            }
        }

        // 重設Sequence值
        public static void ResetSequenceValue(SequencePrefixEnums prefix) {
            using (var conn = GetSqlConnection()) {
                //conn.ExecuteScalar($@"ALTER SEQUENCE {prefix.GetDescription()} RESTART");
                conn.ExecuteScalar($@"ALTER SEQUENCE {prefix.GetDescription()} RESTART WITH 1");
            }
        }
         */
    }
}
