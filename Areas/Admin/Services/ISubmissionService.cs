using Formio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formio.Areas.Admin.Services
{
    public interface ISubmissionService
    {
        Task<bool> FormSubmissionAsync(FormSubmission submit, string userId);  
    }
}
