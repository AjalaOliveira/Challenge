using Challenge.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.Business.Interfaces
{
    public interface IPeopleService
    {
        Task<List<PersonDTO>> SortListOfPeople(List<PersonDTO> personDTO);
    }
}