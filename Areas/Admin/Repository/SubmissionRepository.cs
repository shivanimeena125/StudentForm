using Dapper;
using Formio.Areas.Admin.Connection;
using Formio.Areas.Identity.Data;
using Formio.Models;
using Microsoft.Data.SqlClient;

namespace Formio.Areas.Admin.Repository
{
    public class SubmissionRepository: ISubmissionRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public SubmissionRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task FormSubmissionAsync(FormSubmission submit)
        {
            using var connection = _connectionFactory.CreateConnection();
            
                string Query=@"INSERT INTO FormSubmissions(SubmittedBy, SubmittedUtc, SubmissionData, FormId)
                               VALUES(@SubmittedBy, @SubmittedUtc, @SubmissionData, @FormId)";

            await connection.ExecuteAsync(Query,submit);
        }
       
    }
}
