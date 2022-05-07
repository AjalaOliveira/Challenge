using AutoMapper;
using Challenge.API.Controllers;
using Challenge.API.ViewModels;
using Challenge.Business.Interfaces;
using Challenge.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.API.V1
{
    [ApiVersion("1.0")]
    [Route("[controller]/v{version:apiVersion}/[action]")]
    public class PeopleController : MainController
    {
        private readonly IPeopleService _peopleService;
        private readonly IMapper _mapper;

        public PeopleController(INotificator notificator,
                                IPeopleService peopleService,
                                IMapper mapper) : base(notificator)
        {
            _peopleService = peopleService;
            _mapper = mapper;
        }

        [ActionName("SortListOfPeople")]
        [HttpPost]
        public async Task<IActionResult> SortListOfPeople(List<PersonViewModel> personViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            try
            {
                var result = await _peopleService.SortListOfPeople(_mapper.Map<List<PersonDTO>>(personViewModel));
                return CustomResponse(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    data = ex.Message
                });
            }
        }
    }
}