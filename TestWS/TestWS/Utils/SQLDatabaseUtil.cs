using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TestWS.Utils
{
    public class SQLDatabaseUtil
    {
        private string ConnectionString { get; } //строка подключения к серверу БД
        private IMapper Mapper { get; set; }

        public SQLDatabaseUtil(IMapper mapper)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["testWSDB"].ConnectionString;
            Mapper = mapper;
        }

        public IEnumerable<T> Execute<T>(string storedProcedureName, SqlParameter[] parameters = null, Func<SqlDataReader, List<T>, IMapper, List<T>> extendedReader = null)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            var results = new List<T>();

            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand(storedProcedureName, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (parameters != null && parameters.Any())
                    cmd.Parameters.AddRange(parameters);

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        results.Add(Mapper.Map<T>(reader));
                    }
                }
                if (extendedReader != null)
                {
                    results = extendedReader(reader, results, Mapper);
                }
            }
            finally
            {
                connection?.Close();
                reader?.Close();
            }

            return results;
        }

        public int ExecuteNonQuery(string storedProcedureName, SqlParameter[] parameters = null)
        {
            SqlConnection connection = null;

            int results;

            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand(storedProcedureName, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (parameters != null && parameters.Any())
                    cmd.Parameters.AddRange(parameters);

                results = cmd.ExecuteNonQuery();
            }
            finally
            {
                connection?.Close();
            }

            return results;
        }
    }
}