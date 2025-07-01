using Formio.Data;
using Formio.Models;
using Formio.Servises;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Formio.Repository
{
    public class FormRepository: IFormServices
    {
        
        private readonly string _connectionString;

        public FormRepository(IConfiguration configuration)
        {
           _connectionString = configuration.GetConnectionString("FormConnection");

        }

       public void AddForm(Forms form)
        {
          using var connection = new SqlConnection(_connectionString);

            string query = @"INSERT INTO Forms (Title, CreatedBy, ModifiedBy, CreatedUtc, ModifiedUtc, VersionId, Latest, FormFields)
                VALUES (@Title, @CreatedBy, @ModifiedBy, @CreatedUtc, @ModifiedUtc, @VersionId, @Latest, @FormFields)";

            connection.Execute(query,form);

        }
    }
}
