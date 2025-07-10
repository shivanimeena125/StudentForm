using Formio.Areas.Admin.Repository;
using Formio.Areas.Identity.Data;
using Formio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formio.Areas.Admin.Services
{
    public class SubmissionService : ISubmissionService
    {

        private readonly ISubmissionRepository _submissionRepository;
        private readonly IFromRepository _formRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public SubmissionService(UserManager<ApplicationUser> userManager, IFromRepository fromRepository, ISubmissionRepository submissionRepository)
        {
            _userManager = userManager;
            _submissionRepository = submissionRepository;
            _formRepository = fromRepository;

        }


        public async Task SubmitFormAsync(string submissionData, int formId, string userId)
        {
            var submission = new FormSubmission
            {
                FormId = formId,
                SubmittedBy = userId,
                SubmissionData = submissionData,
                SubmittedUtc = DateTime.UtcNow
            };

            await _submissionRepository.AddSubmissionAsync(submission);
        }
        public async Task<IEnumerable<FormSubmission>> GetAllSubmissionsAsync()
        {
            return await _submissionRepository.GetAllSubmissionsAsync();
        }
        public async Task<FormPreviewModel> GetFormPreviewDataAsync(int formId, string submissionData)
        {
            var form = await _formRepository.GetFormByIdAsync(formId);
            if (form == null) return null;

            return new FormPreviewModel
            {
                FormJson = form.FormFields,
                SubmissionData = submissionData
            };
        }
        public async Task<string> GetFormSchemaAsync(int formId)
        {
            var form = await _formRepository.GetFormByIdAsync(formId);
            return form?.FormFields;
        }
        public async Task<IEnumerable<FormSubmission>> GetSubmissionsByUserAsync(string userId)
        {
            return await _submissionRepository.GetSubmissionsByUserAsync(userId);
        }



    }
}