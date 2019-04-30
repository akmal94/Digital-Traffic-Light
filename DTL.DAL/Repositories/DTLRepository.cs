using Microsoft.Extensions.Options;
using System.Data.Common;
using Insight.Database;
using System.Data.SqlClient;
using DTL.Shared.Models;
using DTL.Shared.Interfaces;

namespace DTL.DAL.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private ConfigOptions _options;

        public BaseRepository(IOptions<ConfigOptions> options)
        {
            SqlInsightDbProvider.RegisterProvider();
            _options = options.Value;
        }

        public T Get<T>() where T : class => GetConnection(_options.DTLConnectionString).As<T>();

        private DbConnection GetConnection(string constr)
        {
            var conn = new SqlConnection(constr);
            return conn;
        }
    }
}
