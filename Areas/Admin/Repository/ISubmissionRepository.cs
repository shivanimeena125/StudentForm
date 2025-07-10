using Formio.Models;

namespace Formio.Areas.Admin.Repository
{
    public interface ISubmissionRepository
    {
        Task AddSubmissionAsync(FormSubmission submit);
        Task<IEnumerable<FormSubmission>> GetAllSubmissionsAsync();

    }
}
