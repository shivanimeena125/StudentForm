using Formio.Models;

namespace Formio.Areas.Admin.Helpers
{
    public static class FormHelper
    {
        public static void FillNewVersionFields(Forms newForm, Forms existingForm, string userId)
        {
            newForm.CreatedBy = existingForm.CreatedBy;
            newForm.VersionId = GuidHelper.NewGuid();
            newForm.CreatedUtc = existingForm.CreatedUtc;
            newForm.FormGroupId = existingForm.FormGroupId;
            newForm.ModifiedBy = userId;
            newForm.Latest = true;
            newForm.ModifiedUtc = DateTime.UtcNow;
        }

    }
}
