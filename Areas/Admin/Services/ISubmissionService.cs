
using Formio.Models;

namespace Formio.Areas.Admin.Services
{
    public interface ISubmissionService
    {
        Task SubmitFormAsync(string submissionData, Guid formGroupId, string userId);
        Task<IEnumerable<FormSubmission>> GetAllSubmissionsAsync();
        Task<FormPreviewModel> GetFormPreviewDataAsync(Guid formGroupId, string submissionData);
        Task<string> GetFormSchemaAsync(Guid formGroupId);
        Task<IEnumerable<FormSubmission>> GetSubmissionsByUserAsync(string userId);


    }
}