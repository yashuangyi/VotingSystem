// Copyright (c) PlaceholderCompany. All rights reserved.

using SqlSugar;

namespace VotingSystem.DB
{
    /// <summary>
    /// 数据库.
    /// </summary>
    public class DataBase
    {
        private static SqlSugarClient _db = null;

        private static string connectionString = "Data Source=localhost;Initial Catalog=votingsystem;User id=root;Password=asd123456";

        /// <summary>
        /// 创建SqlSugar实体.
        /// </summary>
        /// <returns>SqlSugar实体.</returns>
        public static SqlSugarClient CreateClient()
        {
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.MySql, // 设置数据库类型
                IsAutoCloseConnection = false, // 是否自动释放数据事务
                InitKeyType = InitKeyType.Attribute, // 从实体特性中读取主键自增列信息
            });
            return _db;
        }
    }
}