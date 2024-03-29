﻿using Microsoft.AspNetCore.Http;
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
                var stl = await _qDbContext.SourceTrackings.Where(s => s.AcceptedPersonId == group.First().AcceptedPerson.Id).ToListAsync();
                var viewModel = new PersonViewModel
                {
                    SourceTrackers = stl.Count()==0?new List<AcceptedSourceTrackingVM>():stl.Select(s=>new AcceptedSourceTrackingVM
                    {
                        AcceptedPersonId=s.AcceptedPersonId,
                        FieldName = s.FieldName,
                        FieldValue = s.FieldValue,
                        Id= s.Id,
                        PersonId = s.PersonId,
                    }).ToList(),
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
            try
            {
                var updateDataList = JsonConvert.DeserializeObject<List<AcceptedSourceTracking>>(updateData.UpdateData);
                var acceptedPersonId = updateDataList.FirstOrDefault()?.AcceptedPersonId;
                var acceptedPerson = await _qDbContext.AcceptedPeople.Include(s => s.AcceptedPersonWithMatchingRecords).FirstOrDefaultAsync(s => s.Id == acceptedPersonId);

                var file = await _qDbContext.People.Include(s => s.PersonWithFileUploads).ThenInclude(s => s.FileUpload).Where(s => s.Id == acceptedPerson.PersonId).FirstOrDefaultAsync();

                var filename = file.PersonWithFileUploads.FirstOrDefault().FileUpload.FileName;
                foreach (var item in updateDataList)
                {
                    if (item.FieldName.ToLower() == "first_name")
                    {
                        acceptedPerson.FirstName = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct first name.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "last_name")
                    {
                        acceptedPerson.LastName = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct last name.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "email")
                    {
                        acceptedPerson.Email = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct email.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "address")
                    {
                        acceptedPerson.Address = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct address.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });

                    }
                    else if (item.FieldName.ToLower() == "phonenumber")
                    {
                        acceptedPerson.PhoneNumber = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct phone no.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "city")
                    {
                        acceptedPerson.City = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct city.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "state")
                    {
                        acceptedPerson.State = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct state.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "zipcode")
                    {
                        acceptedPerson.ZipCode = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct zip code.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "country")
                    {
                        acceptedPerson.Country = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct country.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                }
                _qDbContext.AcceptedPeople.Update(acceptedPerson);

                var records = await _qDbContext.AcceptedPersonWithMatchingRecord.Where(s => s.AcceptedPersonId == acceptedPersonId).ToListAsync();


                await _qDbContext.AddRangeAsync(updateDataList);
                foreach (var item in records)
                {
                    item.IsDeletedAsMatch = true;
                }
                _qDbContext.AcceptedPersonWithMatchingRecord.UpdateRange(records);
                await _qDbContext.SaveChangesAsync();


                // Access updateData.UpdateData here
                return Ok(new
                {
                    IsSuccessful = true,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("/updateConflictAsExisting")]
        public async Task<IActionResult> UpdateConflictExisting([FromBody] UpdateDataModel updateData)
        {
            try
            {
                var updateDataList = JsonConvert.DeserializeObject<List<AcceptedSourceTracking>>(updateData.UpdateData);
                var acceptedPersonId = updateDataList.FirstOrDefault()?.AcceptedPersonId;
                var acceptedPerson = await _qDbContext.AcceptedPeople.Include(s => s.AcceptedPersonWithMatchingRecords).FirstOrDefaultAsync(s => s.Id == acceptedPersonId);

                var file = await _qDbContext.People.Include(s => s.PersonWithFileUploads).ThenInclude(s => s.FileUpload).Where(s => s.Id == acceptedPerson.PersonId).FirstOrDefaultAsync();

                var filename = file.PersonWithFileUploads.FirstOrDefault().FileUpload.FileName;
                foreach (var item in updateDataList)
                {
                    if (item.FieldName.ToLower() == "first_name")
                    {
                        acceptedPerson.FirstName = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct first name.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "last_name")
                    {
                        acceptedPerson.LastName = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct last name.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "email")
                    {
                        acceptedPerson.Email = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct email.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "address")
                    {
                        acceptedPerson.Address = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct address.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });

                    }
                    else if (item.FieldName.ToLower() == "phonenumber")
                    {
                        acceptedPerson.PhoneNumber = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct phone no.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "city")
                    {
                        acceptedPerson.City = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct city.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "state")
                    {
                        acceptedPerson.State = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct state.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "zipcode")
                    {
                        acceptedPerson.ZipCode = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct zip code.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                    else if (item.FieldName.ToLower() == "country")
                    {
                        acceptedPerson.Country = item.FieldValue;
                        await _qDbContext.ActivityLogs.AddAsync(new ActivityLog
                        {
                            LogBy = "User",
                            LogDate = DateTime.Now,
                            LogDescription = $"from '{filename}' was accepted as correct country.",
                            LogDetails = acceptedPersonId.ToString(),
                            LogType = item.FieldValue
                        });
                    }
                }
                _qDbContext.AcceptedPeople.Update(acceptedPerson);

                var records = await _qDbContext.AcceptedPersonWithMatchingRecord.Where(s => s.AcceptedPersonId == acceptedPersonId).ToListAsync();

                var existingST = await _qDbContext.SourceTrackings.Where(s => s.AcceptedPersonId == acceptedPersonId).ToListAsync();

                if (existingST.Any())
                {
                    _qDbContext.RemoveRange(existingST);
                }

                await _qDbContext.AddRangeAsync(updateDataList);


                await _qDbContext.SaveChangesAsync();


                // Access updateData.UpdateData here
                return Ok(new
                {
                    IsSuccessful = true,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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
                .Where(s => s.AcceptedPersonId == id && !s.IsDeletedAsMatch).ToListAsync();

            var list = new List<PersonViewData>();
            var a= _qDbContext.AcceptedPeople.First(s=>s.Id==id);
            var accepted = await _qDbContext.People
              .Include(s => s.PersonWithFileUploads)
                  .ThenInclude(s => s.FileUpload)
                      .ThenInclude(s => s.Category)
      .FirstOrDefaultAsync(s=>s.Id== a.PersonId);

            var matchingRecord = accepted.PersonWithFileUploads
                .FirstOrDefault();

            var filename = matchingRecord.FileUpload?.FileName;
            var category = matchingRecord.FileUpload?.Category?.CategoryName;
            var acPer = new PersonViewData
            {
                Id = accepted.Id,
                FirstName = accepted.FirstName,
                LastName = accepted.LastName,
                Email = accepted.Email,
                PhoneNumber = accepted.PhoneNumber,
                Address = accepted.Address,
                City = accepted.City,
                State = accepted.State,
                ZipCode = accepted.ZipCode,
                Country = accepted.Country,
                IsDeleted = accepted.IsDeleted,
                FileName = category + " " + filename
            };

            list.Add(acPer);



            var data = personList.Where(s => s.PersonId != id).Select(p => new PersonViewData
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

            if( data .Count()>0)
            {
                list.AddRange(data);

                list.Remove(acPer);
            }

            return Ok(list);
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
            catch (Exception ex)
            {
                return NoContent();
            }

        }

    }
}
