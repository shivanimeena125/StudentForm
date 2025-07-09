using Formio.Models;

namespace Formio.Areas.Admin.Repository
{
    public interface ISubmissionRepository
    {
        Task FormSubmissionAsync(FormSubmission submit);

    }
}
