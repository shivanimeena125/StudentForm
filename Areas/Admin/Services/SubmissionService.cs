using Formio.Areas.Admin.Repository;
using Formio.Areas.Identity.Data;
using Formio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formio.Areas.Admin.Services
{
    public class SubmissionService: ISubmissionService
    {

        private readonly ISubmissionRepository _submissionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public SubmissionService(UserManager<ApplicationUser> userManager,ISubmissionRepository submissionRepository)
        {
            _userManager = userManager;
           _submissionRepository= submissionRepository;

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


    }
}
