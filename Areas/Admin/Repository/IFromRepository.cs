using Formio.Models;
using System.Security.Claims;

namespace Formio.Areas.Admin.Repository
{
    public interface IFromRepository
    {
        Task AddFormAsync(Forms form);

        Task<List<ViewFormModel>> ViewFormsAsync();

        Task<Forms> GetFormByIdAsync(int id);

        Task<int> DeleteFormAsync(int id);

        Task UpdateFormAsync(Forms form);
        

        Task<Forms> GetFormByFormGroupId(Guid formGroupId);

       
    }
}
