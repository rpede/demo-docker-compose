using Microsoft.AspNetCore.Mvc;
using Service.Draft;
using Service.Draft.Dto;

namespace Api.Controllers;

[ApiController]
[Route("api/draft")]
public class DraftController(IDraftService service) : ControllerBase
{
    private readonly IDraftService service = service;

    [HttpGet]
    [Route("")]
    public IEnumerable<Draft> List() => service.List(HttpContext.User);

    [HttpPost]
    [Route("")]
    public async Task<long> Create(DraftFormData data) =>
        await service.Create(HttpContext.User, data);

    [HttpGet]
    [Route("{id}")]
    public DraftDetail Get(long id) => service.GetById(HttpContext.User, id);

    [HttpPut]
    [Route("{id}")]
    public async Task Update(long id, DraftFormData data) =>
        await service.Update(HttpContext.User, id, data);

    [HttpDelete]
    [Route("{id}")]
    public async Task Delete(long id) => await service.Delete(HttpContext.User, id);
}
