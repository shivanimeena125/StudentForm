
using Formio.Models;
using Microsoft.Data.SqlClient;
using Dapper;

using Formio.Areas.Admin.Services;
using Formio.Areas.Admin.Connection;

namespace Formio.Areas.Admin.Repository
{
    public class FormRepository : IFromRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public FormRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddFormAsync(Forms form)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = @"INSERT INTO Forms (Title, CreatedBy, ModifiedBy, CreatedUtc, ModifiedUtc, VersionId, Latest, FormFields,FormGroupId)
                VALUES (@Title, @CreatedBy, @ModifiedBy, @CreatedUtc, @ModifiedUtc, @VersionId, @Latest, @FormFields, @FormGroupId)";
            await connection.ExecuteAsync(query, form);
        }

        public async Task<List<ViewFormModel>> ViewFormsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = @"
        SELECT 
            Forms.Id, Forms.Title, Forms.CreatedUtc, Forms.ModifiedUtc,Forms.FormGroupId,Forms.VersionId,
            AspNetUsers.Name AS CreatedByName
            FROM  Forms INNER JOIN 
            AspNetUsers  ON Forms.CreatedBy = AspNetUsers.Id where Forms.Latest=1  Order By Forms.ModifiedUtc DESC";
            return (await connection.QueryAsync<ViewFormModel>(query)).ToList();
        }
        public async Task<Forms> GetFormByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = "SELECT * FROM Forms WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Forms>(query, new { Id = id });
        }

        public async Task<int> DeleteFormAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = "DELETE FROM Forms WHERE Id = @Id";
            return await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task UpdateFormAsync(Forms form)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = @"
                UPDATE Forms 
                SET Title = @Title, 
                Latest = @Latest,
                ModifiedBy = @ModifiedBy, 
                ModifiedUtc = @ModifiedUtc, 
                FormFields = @FormFields
                WHERE FormGroupId= @FormGroupId";

            await connection.ExecuteAsync(query, form);
        }

        public async Task<Forms> GetFormByFormGroupId(Guid formGroupId)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = "SELECT * FROM Forms WHERE FormGroupId = @FormGroupId And Latest=1";
            return await connection.QueryFirstOrDefaultAsync<Forms>(query, new { FormGroupId = formGroupId });
        }

        public async Task<(List<ViewFormModel> Data, int TotalCount, int filtered)> GetPaginatedFormsAsync(
    string title, string startDate, string endDate, int start, int length)
        {
            using var connection = _connectionFactory.CreateConnection();

            var baseQuery = @"
    SELECT 
    Forms.Id,
    Forms.Title,
    AspNetUsers.Name AS CreatedByName,
    Forms.CreatedUtc,
    Forms.ModifiedUtc,
    Forms.FormGroupId 
    FROM Forms
    INNER JOIN AspNetUsers ON Forms.CreatedBy = AspNetUsers.Id
    WHERE Forms.Latest=1 ";

            var countQuery = @"SELECT COUNT(*) FROM Forms WHERE Latest=1 ";

            var filters = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(title))
            {
                filters.Add("Title LIKE @Title");
                parameters.Add("Title", $"%{title}%");
            }


            if (!string.IsNullOrEmpty(startDate))
            {
                filters.Add("CreatedUtc >= @StartDate");
                parameters.Add("StartDate", DateTime.Parse(startDate));
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                filters.Add("CreatedUtc <= @EndDate");
                parameters.Add("EndDate", DateTime.Parse(endDate));
            }

            if (filters.Any())
            {
                var where = " AND " + string.Join(" AND ", filters);
                baseQuery += where;
                countQuery += where;
            }

            var total = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Forms WHERE Latest=1");
            var filtered = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

            baseQuery += " ORDER BY CreatedUtc DESC OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY";
            parameters.Add("Start", start);
            parameters.Add("Length", length);

            var data = (await connection.QueryAsync<ViewFormModel>(baseQuery, parameters)).ToList();

            return (data, total, filtered);
        }
    }
}