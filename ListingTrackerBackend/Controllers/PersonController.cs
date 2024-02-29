using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using ListingTracker.Classes;
using NPOI.POIFS.Crypt.Dsig;

namespace ListingTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly qDbContext _qDbContext;

        public PersonController(qDbContext qDbContext)
        {
            _qDbContext = qDbContext;
        }

        [HttpPost("/getAcceptedPersonWithMatchingRecords")]
        public async Task<IActionResult> GetAcceptedPersonWithMatchingRecords()
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            var personList = await _qDbContext.AcceptedPersonWithMatchingRecord
                .Include(s => s.Person).ThenInclude(s => s.PersonWithFileUploads).ThenInclude(s => s.FileUpload).ThenInclude(s => s.Category)
                .Include(s => s.AcceptedPerson)
                .ToListAsync();
            var viewData = new List<PersonViewModel>();

            var groupedPeople = personList.GroupBy(p => p.AcceptedPersonId);

            foreach (var group in groupedPeople)
            {
                var viewModel = new PersonViewModel
                {
                    AcceptedPerson = new AcceptedPersonViewModel
                    {
                        Id = group.First().AcceptedPerson.Id,
                        PersonId = group.First().PersonId,
                        FirstName = group.First().AcceptedPerson.FirstName,
                        LastName = group.First().AcceptedPerson.LastName,
                        Email = group.First().AcceptedPerson.Email,
                        PhoneNumber = group.First().AcceptedPerson.PhoneNumber,
                        Address = group.First().AcceptedPerson.Address,
                        City = group.First().AcceptedPerson.City,
                        State = group.First().AcceptedPerson.State,
                        ZipCode = group.First().AcceptedPerson.ZipCode,
                        Country = group.First().AcceptedPerson.Country,
                        IsDeleted = group.First().AcceptedPerson.IsDeleted,
                    },
                    PersonList = group.Select(p => new PersonViewData
                    {
                        Id = p.Person.Id,
                        FirstName = p.Person.FirstName,
                        LastName = p.Person.LastName,
                        Email = p.Person.Email,
                        PhoneNumber = p.Person.PhoneNumber,
                        Address = p.Person.Address,
                        City = p.Person.City,
                        State = p.Person.State,
                        ZipCode = p.Person.ZipCode,
                        Country = p.Person.Country,
                        IsDeleted = p.Person.IsDeleted,
                        FileName = (
        from person in personList
        join personWithFile in p.Person.PersonWithFileUploads on person.PersonId equals personWithFile.PersonId
        select $"{personWithFile.FileUpload.Category.CategoryName} {personWithFile.FileUpload.FileName}"
    ).FirstOrDefault()
                    }).ToList()
                };

                viewData.Add(viewModel);
            }

            return Ok(viewData);
        }
    }
}
