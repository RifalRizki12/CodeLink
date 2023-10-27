using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class CurriculumVitaeRepository : GeneralRepository<CurriculumVitae>, ICurriculumVitaeRepository
    {
        public CurriculumVitaeRepository(CodeLinkDbContext context) : base(context) { }

        public CurriculumVitae GetByEmployeeGuid(Guid employeeGuid)
        {
            return _context.CurriculumVitaes.FirstOrDefault(cv => cv.Guid == employeeGuid);
        }
    }
}
