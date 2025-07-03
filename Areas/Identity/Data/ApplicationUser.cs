using Microsoft.AspNetCore.Identity;
using Formio.Models;
namespace Formio.Areas.Identity.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }    
    }
}
