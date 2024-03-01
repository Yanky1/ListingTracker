using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using ListingTracker.Classes;
using NPOI.POIFS.Crypt.Dsig;
using ListingTracker.LogicClasses;
using ListingTracker.DbEntities;
using System.Net.Http.Json;
using Newtonsoft.Json;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using System.Reflection.Emit;
using NPOI.SS.Formula.Eval;
using System.Diagnostics.Metrics;

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
                    PersonList = group.Where(s => !s.IsDeletedAsMatch).Select(p => new PersonViewData
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

        [HttpPost("/updateConflict")]
        public async Task<IActionResult> UpdateConflict([FromBody] UpdateDataModel updateData)
        {
            var updateDataList = JsonConvert.DeserializeObject<List<AcceptedSourceTracking>>(updateData.UpdateData);
            var acceptedPersonId = updateDataList.FirstOrDefault()?.AcceptedPersonId;
            var acceptedPerson = await _qDbContext.AcceptedPeople.FirstOrDefaultAsync(s => s.Id == acceptedPersonId);

            foreach (var item in updateDataList)
            {
                if (item.FieldName.ToLower() == "first_name")
                {
                    acceptedPerson.FirstName = item.FieldValue;
                }
                else if (item.FieldName.ToLower() == "last_name")
                {
                    acceptedPerson.LastName = item.FieldValue;
                }
                else if (item.FieldName.ToLower() == "email")
                {
                    acceptedPerson.Email = item.FieldValue;
                }
                else if (item.FieldName.ToLower() == "address")
                {
                    acceptedPerson.Address = item.FieldValue;
                }
                else if (item.FieldName.ToLower() == "phonenumber")
                {
                    acceptedPerson.PhoneNumber = item.FieldValue;
                }
                else if (item.FieldName.ToLower() == "city")
                {
                    acceptedPerson.City = item.FieldValue;
                }
                else if (item.FieldName.ToLower() == "state")
                {
                    acceptedPerson.State = item.FieldValue;
                }
                else if (item.FieldName.ToLower() == "zipcode")
                {
                    acceptedPerson.ZipCode = item.FieldValue;
                }
                else if (item.FieldName.ToLower() == "country")
                {
                    acceptedPerson.Country = item.FieldValue;
                }
            }
            _qDbContext.AcceptedPeople.Update(acceptedPerson);


            await _qDbContext.AddRangeAsync(updateDataList);
            await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
            {
                LogBy = "User",
                LogDate = DateTime.Now,
                LogDescription = $"Conflict Resolved for Accepted Person named ''{acceptedPerson.FirstName}' ' {acceptedPerson.LastName} ",
                LogDetails = $"Conflict Resolved for Accepted Person named ''{acceptedPerson.FirstName}' ' {acceptedPerson.LastName} ",
                LogType = "Conflict Resolved"
            });
            await _qDbContext.SaveChangesAsync();


            // Access updateData.UpdateData here
            return Ok(new
            {
                IsSuccessful = true,
            });
        }

        [HttpDelete("/deletePerson")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var person = await _qDbContext.People.FirstOrDefaultAsync(s => s.Id == id);
            var matching = await _qDbContext.AcceptedPersonWithMatchingRecord.FirstOrDefaultAsync(s => s.PersonId == id);

            if (person == null)
            {
                return StatusCode(403, "Can not delete!");
            }

            // Soft delete by setting IsDeleted to true
            person.IsDeleted = true;

            _qDbContext.People.Update(person);

            matching.IsDeletedAsMatch = true;
            _qDbContext.AcceptedPersonWithMatchingRecord.Update(matching);
            await _qDbContext.SaveChangesAsync();

            return Ok(new
            {
                IsSuccessful = true,
                Message = "Person deleted successfully"
            });
        }

        [HttpPost("/getAcceptedPersonWithMatchingRecordById")]
        public async Task<IActionResult> GetAcceptedPersonWithMatchingRecordsById(Guid id)
        {
            var personList = await _qDbContext.AcceptedPersonWithMatchingRecord
                .Include(s => s.Person).ThenInclude(s => s.PersonWithFileUploads).ThenInclude(s => s.FileUpload).ThenInclude(s => s.Category)
                .Where(s => s.AcceptedPersonId == id&&!s.IsDeletedAsMatch).ToListAsync();

            var data = personList.Select(p => new PersonViewData
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
            }).ToList();

            if ( data.Count()==1) {
                data.Add(new PersonViewData{
                Id = Guid.Empty,
                PersonId = Guid.Empty,
                FirstName = "",
                LastName = "",
                Email = "",
                PhoneNumber = "",
                Address = "",
                City = "",
                State = "",
                ZipCode = "",
                Country = "",
                IsDeleted = false,
                FileName = "",
            });
            }
            else if( data.Count()==0 )
            {
                data.Add(new PersonViewData
                {
                    Id = Guid.Empty,
                    PersonId = Guid.Empty,
                    FirstName = "",
                    LastName = "",
                    Email = "",
                    PhoneNumber = "",
                    Address = "",
                    City = "",
                    State = "",
                    ZipCode = "",
                    Country = "",
                    IsDeleted = false,
                    FileName = "",
                });
                
                data.Add(new PersonViewData
                {
                    Id = Guid.Empty,
                    PersonId = Guid.Empty,
                    FirstName = "",
                    LastName = "",
                    Email = "",
                    PhoneNumber = "",
                    Address = "",
                    City = "",
                    State = "",
                    ZipCode = "",
                    Country = "",
                    IsDeleted = false,
                    FileName = "",
                });
            }

            return Ok(data);
        }

        [HttpPost("/getAcceptedPerson")]
        public async Task<IActionResult> GetAcceptedPerson(Guid id)
        {
            var acceptedPerson = await _qDbContext.AcceptedPeople.FirstOrDefaultAsync(s => s.Id == id);
            return Ok(acceptedPerson);
        }    
        
        [HttpPost("/updateAcceptedPerson")]
        public async Task<IActionResult> UpdateAcceptedPerson([FromBody] UpdateDataModel updateData)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<AcceptedPerson>(updateData.UpdateData);

                var acceptedPerson = await _qDbContext.AcceptedPeople.FirstOrDefaultAsync(s => s.Id == model.Id);
                if (acceptedPerson != null)
                {
                    acceptedPerson.FirstName = model.FirstName;
                    acceptedPerson.LastName = model.LastName;
                    acceptedPerson.Email = model.Email;
                    acceptedPerson.PhoneNumber = model.PhoneNumber;
                    acceptedPerson.Address = model.Address;
                    acceptedPerson.City = model.City;
                    acceptedPerson.Country = model.Country;
                    acceptedPerson.ZipCode = model.ZipCode;
                    acceptedPerson.State = model.State;
                    _qDbContext.Entry(acceptedPerson).State = EntityState.Detached;
                    _qDbContext.Update(acceptedPerson);
                    await _qDbContext.SaveChangesAsync();
                }
                return Ok(new
                {
                    IsSuccessful = true,
                    Data = model
                });
            }
            catch(Exception ex)
            {
                return NoContent();
            }
            
        }

    }
}
