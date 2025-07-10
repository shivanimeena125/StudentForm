
using Formio.Models;

namespace Formio.Areas.Admin.Services
{
    public interface ISubmissionService
    {
        Task SubmitFormAsync(string submissionData, int formId, string userId);
        Task<IEnumerable<FormSubmission>> GetAllSubmissionsAsync();
        Task<FormPreviewModel> GetFormPreviewDataAsync(int formId, string submissionData);
        Task<string> GetFormSchemaAsync(int formId);
        Task<IEnumerable<FormSubmission>> GetSubmissionsByUserAsync(string userId);


    }
}