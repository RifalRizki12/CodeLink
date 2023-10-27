using API.Models;

namespace API.Contracts
{
    public interface ICurriculumVitaeRepository : IGeneralRepository<CurriculumVitae>
    {
        CurriculumVitae GetByEmployeeGuid(Guid employeeGuid);
    }
}
