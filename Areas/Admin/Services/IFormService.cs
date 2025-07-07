using Formio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formio.Areas.Admin.Services
{
    public interface IFormService
    {

        Task<JsonResult> AddFormAsync(Forms form, ClaimsPrincipal userPrincipal);


        Task<List<ViewFormModel>> ViewFormsAsync();

        Task<JsonResult> DeleteFormAsync(int id);
       
        Task<JsonResult> UpdateFormAsync(Forms model, ClaimsPrincipal userPrincipal);


        Task<Forms> GetFormByFormGroupId(Guid formGroupId);


       





    }
}
