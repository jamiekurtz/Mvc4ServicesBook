using System.Data.SqlClient;

namespace MVC4ServicesBook.Data.SqlServer
{
    public interface ISqlCommandFactory
    {
        SqlCommand GetCommand();
    }
}