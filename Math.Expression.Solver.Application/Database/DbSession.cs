using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Math.Expression.Solver.Application.Database
{
    public sealed class DbSession : IDisposable
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbSession(IConfiguration config)
        {
            Connection = new NpgsqlConnection(config.GetConnectionString("postgres"));
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}