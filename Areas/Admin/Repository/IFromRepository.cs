using Formio.Models;

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

        //Task<List<ViewFormModel>> GetFilteredFormsAsync(string title);
        Task<(List<ViewFormModel> Data, int TotalCount, int filtered)> GetPaginatedFormsAsync(
    string title, string startDate, string endDate, int start, int length);


    }
}