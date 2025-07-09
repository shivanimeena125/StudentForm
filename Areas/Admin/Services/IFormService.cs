using Formio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formio.Areas.Admin.Services
{
    public interface IFormService
    {
        Task<bool> AddFormAsync(Forms form, string userId);
        Task<List<ViewFormModel>> ViewFormsAsync();
        Task<JsonResult> DeleteFormAsync(int id);
        Task<JsonResult> UpdateFormAsync(Forms model, ClaimsPrincipal userPrincipal);
        Task<Forms> GetFormByFormGroupId(Guid formGroupId);
        Task<object> GetPaginatedFormsAsync(DataTableRequest request, string title, string startDate, string endDate);

    }
}
