using Formio.Models;

namespace Formio.Areas.Admin.Servises
{
    public interface IFormServices
    {
        Task AddForm(Forms form);
        Task<List<ViewFormModel>> AllForms();

        Task<Forms?> GetFormById(int id);

        Task<int> DeleteForm(int id);

        Task UpdateForm(Forms form);
    }
}
