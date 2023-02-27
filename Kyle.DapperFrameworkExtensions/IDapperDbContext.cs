using System.Data;

namespace Kyle.DapperFrameworkExtensions
{
    public interface IDapperDbContext
    {
        IDbConnection CreateConnection(string? name = null);
    }
}