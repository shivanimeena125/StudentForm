using System.Data;

namespace Formio.Areas.Admin.Connection
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
