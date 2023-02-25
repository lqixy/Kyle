using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions
{
    //public interface IDbContextProvider<out TDatabase> where TDatabase : IDatabase
    public interface IDbContextProvider
    {
        IDatabase GetDbConnection();
    }

    //public class DapperDbContextProvider<TDatabase> : IDbContextProvider<TDatabase> where TDatabase : IDatabase
    public class DapperDbContextProvider : IDbContextProvider
    {
        public IDbProviderFactory _dbProviderFactory { get; private set; }

        public string ConnectionString { get; private set; }

        public IDatabase DbContext { get; private set; }

        //private readonly ILogger logger = ILoggerFactor;

        public DapperDbContextProvider(IDatabase dbContext, IDbProviderFactory factory)
        {
            DbContext = dbContext;
            _dbProviderFactory = factory ?? throw new ArgumentException(null, nameof(factory));
            ConnectionString = _dbProviderFactory.ConnectionString;
        }

        public IDatabase GetDbConnection()
        {
            var connection = GetNewOpenConnection();
            DbContext.SetConnection(connection);
            return DbContext;
        }

        internal DbConnection GetNewOpenConnection(bool isOpen = false)
        {
            DbConnection connection = null;
            try
            {
                connection = CreateConnection();
                if (isOpen) connection.Open();
            }
            catch (Exception ex)
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
                //logger.LogError("open database is error", ex);
                throw new Exception("open database is error");
            }
            return connection;
        }
        private DbConnection CreateConnection()
        {
            DbConnection newConnection = _dbProviderFactory.DbFactory.CreateConnection();
            newConnection.ConnectionString = _dbProviderFactory.ConnectionString;
            return newConnection;
        }
    }

}
