using Formio.Models;

namespace Formio.Areas.Admin.Servises
{
    public interface IFormServices
    {
        Task AddFormAsync(Forms form);
        Task<List<ViewFormModel>> ViewFormsAsync();

        Task<Forms> GetFormByIdAsync(int id);

        Task<int> DeleteFormAsync(int id);

        Task UpdateFormAsync(Forms form);
    }
}
