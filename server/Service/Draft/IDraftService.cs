using System.Security.Claims;

namespace Service.Draft;

public interface IDraftService
{
    Dto.DraftDetail GetById(ClaimsPrincipal principal, long id);
    IEnumerable<Dto.Draft> List(ClaimsPrincipal principal);
    Task<long> Create(ClaimsPrincipal principal, Dto.DraftFormData data);
    Task Update(ClaimsPrincipal principal, long id, Dto.DraftFormData data);
    Task Delete(ClaimsPrincipal principal, long id);
}
