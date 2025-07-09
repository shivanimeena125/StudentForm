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

       
        public async Task<bool> FormSubmissionAsync(FormSubmission submit, string userId)
        {
            
            submit.SubmittedBy = userId;
            submit.SubmittedUtc = DateTime.UtcNow;
            await _submissionRepository.FormSubmissionAsync(submit);
            return true;
        }

        
    }
}
