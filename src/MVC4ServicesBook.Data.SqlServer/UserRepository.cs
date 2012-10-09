using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MVC4ServicesBook.Common;
using MVC4ServicesBook.Data.Model;
using NHibernate;
using NHibernate.Linq;

namespace MVC4ServicesBook.Data.SqlServer
{
    public class UserRepository : IUserRepository
    {
        private readonly ISession _session;
        private readonly ISqlCommandFactory _sqlCommandFactory;
        private readonly IDatabaseValueParser _valueParser;

        public UserRepository(ISession session, ISqlCommandFactory sqlCommandFactory, IDatabaseValueParser valueParser)
        {
            _session = session;
            _sqlCommandFactory = sqlCommandFactory;
            _valueParser = valueParser;
        }

        public IQueryable<User> AllUsers()
        {
            return _session.Query<User>();
        }

        public User GetUser(Guid userId)
        {
            return _session.Get<User>(userId);
        }

        public void SaveUser(Guid userId, string firstname, string lastname)
        {
            using(var command = _sqlCommandFactory.GetCommand())
            {
                command.CommandText = "dbo.SaveUser";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@firstname", firstname);
                command.Parameters.AddWithValue("@lastname", lastname);

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<User> GetUsersForTask(long taskId)
        {
            using(var command = _sqlCommandFactory.GetCommand())
            {
                command.CommandText = "dbo.GetTaskUsers";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@taskId", taskId);

                using (var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        yield return new User
                                         {
                                             UserId = _valueParser.ParseGuid(reader["UserId"]),
                                             Username = _valueParser.ParseString(reader["Username"]),
                                             Firstname = _valueParser.ParseString(reader["Firstname"]),
                                             Lastname = _valueParser.ParseString(reader["Lastname"]),
                                             Email = _valueParser.ParseString(reader["Email"]),
                                             Timestamp = _valueParser.ParseByteArray(reader["ts"])
                                         };
                    }
                }
            }
        }
    }
}
