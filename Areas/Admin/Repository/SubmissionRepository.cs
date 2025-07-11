using Dapper;
using Formio.Areas.Admin.Connection;
using Formio.Areas.Identity.Data;
using Formio.Models;
using Microsoft.Data.SqlClient;

namespace Formio.Areas.Admin.Repository
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public SubmissionRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddSubmissionAsync(FormSubmission submit)
        {
            using var connection = _connectionFactory.CreateConnection();

            string Query = @"INSERT INTO FormSubmissions(SubmittedBy, SubmittedUtc, SubmissionData, FormGroupId )
                               VALUES(@SubmittedBy, @SubmittedUtc, @SubmissionData, @FormGroupId )";

            await connection.ExecuteAsync(Query, submit);
        }
        public async Task<IEnumerable<FormSubmission>> GetAllSubmissionsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            string sql = @"SELECT fs.*, f.Title as FormTitle, u.Name as SubmittedUserName
                   FROM FormSubmissions fs
                   JOIN Forms f ON fs.FormGroupId  = f.FormGroupId 
                   JOIN AspNetUsers u ON fs.SubmittedBy = u.Id
                   ORDER BY fs.SubmittedUtc DESC";

            var submissions = await connection.QueryAsync<FormSubmissionExtended>(sql);
            return submissions;
        }
        public async Task<IEnumerable<FormSubmission>> GetSubmissionsByUserAsync(string userId)
        {
            using var connection = _connectionFactory.CreateConnection();

            string sql = @"SELECT fs.*, f.Title as FormTitle, u.Name as SubmittedUserName
                   FROM FormSubmissions fs
                   JOIN Forms f ON fs.FormGroupId  = f.FormGroupId 
                   JOIN AspNetUsers u ON fs.SubmittedBy = u.Id
                   WHERE fs.SubmittedBy = @UserId
                   ORDER BY fs.SubmittedUtc DESC";

            return await connection.QueryAsync<FormSubmissionExtended>(sql, new { UserId = userId });
        }

    }
}