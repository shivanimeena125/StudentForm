
using Formio.Models;
using Microsoft.Data.SqlClient;
using Dapper;

using Formio.Areas.Admin.Servises;

namespace Formio.Areas.Admin.Repository
{
    public class FormRepository : IFormServices
    {

        private readonly string _connectionString;

        public FormRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FormConnection");

        }

        public async Task AddForm(Forms form)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"INSERT INTO Forms (Title, CreatedBy, ModifiedBy, CreatedUtc, ModifiedUtc, VersionId, Latest, FormFields)
                VALUES (@Title, @CreatedBy, @ModifiedBy, @CreatedUtc, @ModifiedUtc, @VersionId, @Latest, @FormFields)";

            await connection.ExecuteAsync(query, form);

        }

        public async Task<List<ViewFormModel>> AllForms()
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"
        SELECT 
            Forms.Id,
            Forms.Title, 
            Forms.CreatedUtc, 
            AspNetUsers.Name AS CreatedByName
        FROM 
            Forms 
        INNER JOIN 
            AspNetUsers  ON Forms.CreatedBy = AspNetUsers.Id Order By Forms.CreatedUtc DESC";
            return (await connection.QueryAsync<ViewFormModel>(query)).ToList();

        }

        public async Task<Forms?> GetFormById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Forms WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Forms>(query, new { Id = id });
        }

        public async Task<int> DeleteForm(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "DELETE FROM Forms WHERE Id = @Id";
           return await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task UpdateForm(Forms form)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"
                UPDATE Forms 
                SET Title = @Title, 
                    ModifiedBy = @ModifiedBy, 
                    ModifiedUtc = @ModifiedUtc, 
                    FormFields = @FormFields
                WHERE Id = @Id";
            await connection.ExecuteAsync(query, form);
        }
    }
}
