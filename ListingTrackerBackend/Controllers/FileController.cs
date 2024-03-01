using ListingTracker.Classes;
using ListingTracker.DbEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.HPSF;
using OfficeOpenXml;
using System.Xml;

namespace ListingTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly qDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public FileController(qDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("/uploadFile")]
        public async Task<IActionResult> UploadPersonData(FileUploadViewModel model)
        {
            var checkFiles = await _context.FileUploads.FirstOrDefaultAsync(s => s.FileName == model.FileName && !s.IsDeleted);
            if (checkFiles == null)
            {
                byte[] fileBytes = Convert.FromBase64String(model.file);
                string appRootPath = _hostingEnvironment.ContentRootPath;
                string fullPath = Path.Combine(appRootPath, "wwwroot\\UploadedFiles", model.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                System.IO.File.WriteAllBytes(fullPath, fileBytes);

                var fileUploadEntity = new FileUpload
                {
                    IsDeleted = false,
                    FileName = model.FileName,
                    CategoryId = model.CategoryId,
                    DateUploaded = model.DateUploaded,
                    FileContentType = model.FileContentType,
                    FileDescription = model.FileDescription,
                    FileExtension = model.FileExtension,
                    FilePath = "wwwroot\\UploadedFiles",
                    FileSize = model.FileSize,
                };
                await _context.FileUploads.AddAsync(fileUploadEntity);




                using (MemoryStream memoryStream = new MemoryStream(fileBytes))
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        var person = new Person
                        {
                            FirstName = worksheet.Cells[row, 1].Value?.ToString(),
                            LastName = worksheet.Cells[row, 2].Value?.ToString(),
                            Email = worksheet.Cells[row, 3].Value?.ToString(),
                            PhoneNumber = worksheet.Cells[row, 4].Value?.ToString(),
                            Address = worksheet.Cells[row, 5].Value?.ToString(),
                            City = worksheet.Cells[row, 6].Value?.ToString(),
                            State = worksheet.Cells[row, 7].Value?.ToString(),
                            ZipCode = worksheet.Cells[row, 8].Value?.ToString(),
                            Country = worksheet.Cells[row, 9].Value?.ToString(),
                            IsDeleted = false
                        };
                        _context.People.Add(person);



                        var personWithFile = new PersonWithFile
                        {
                            FileUploadId = fileUploadEntity.Id,
                            PersonId = person.Id,
                        };

                        _context.PersonWithFile.Add(personWithFile);


                        var existsInAccepted = await _context.AcceptedPeople.Where(
                            s => s.FirstName.Trim().ToLower().Contains(person.FirstName)
                            && s.LastName.Trim().ToLower().Contains(person.LastName)
                            && (s.Email.Trim().ToLower().Contains(person.Email) || s.PhoneNumber.Trim().ToLower().Contains(person.PhoneNumber))
                            )//.Include(s => s.Person).ThenInclude(s => s.PersonWithFileUploads).ThenInclude(s => s.FileUpload).ThenInclude(s => s.Category)
                            .FirstOrDefaultAsync();
                        var acPerson = new AcceptedPerson()
                        {
                            FirstName = worksheet.Cells[row, 1].Value?.ToString(),
                            LastName = worksheet.Cells[row, 2].Value?.ToString(),
                            Email = worksheet.Cells[row, 3].Value?.ToString(),
                            PhoneNumber = worksheet.Cells[row, 4].Value?.ToString(),
                            Address = worksheet.Cells[row, 5].Value?.ToString(),
                            City = worksheet.Cells[row, 6].Value?.ToString(),
                            State = worksheet.Cells[row, 7].Value?.ToString(),
                            ZipCode = worksheet.Cells[row, 8].Value?.ToString(),
                            Country = worksheet.Cells[row, 9].Value?.ToString(),
                            IsDeleted = false,
                            PersonId = person.Id
                        };

                        if (existsInAccepted == null)
                        {
                            await _context.AcceptedPeople.AddAsync(acPerson);
                            await _context.AcceptedPersonWithMatchingRecord.AddAsync(new AcceptedPersonWithMatchingRecord
                            {
                                AcceptedPersonId = acPerson.Id,
                                PersonId = person.Id,
                                IsDeletedAsMatch = false
                            });
                        }
                        else
                        {
                            var acceptedPersonWithMatchingRecord = await _context.PersonWithFile.Include(s=>s.FileUpload).ThenInclude(s=>s.Category).Where(s=>s.PersonId==existsInAccepted.PersonId).FirstOrDefaultAsync(); 
                            var sourceTracing = await _context.SourceTrackings.FirstOrDefaultAsync(s => s.AcceptedPersonId == existsInAccepted.Id);
                            var categoryData = await _context.Categories.FirstOrDefaultAsync(s => s.Id == model.CategoryId);
                            if (sourceTracing == null
                                && categoryData.Id != acceptedPersonWithMatchingRecord.FileUpload.CategoryId
                                && categoryData.CategoryLevel > acceptedPersonWithMatchingRecord.FileUpload.Category.CategoryLevel)
                            {
                                _context.Entry(existsInAccepted).State = EntityState.Detached;
                                acPerson.Id = existsInAccepted.Id;
                                _context.AcceptedPeople.Update(acPerson);
                                await _context.AcceptedPersonWithMatchingRecord.AddAsync(new AcceptedPersonWithMatchingRecord
                                {
                                    AcceptedPersonId = acPerson.Id,
                                    PersonId = person.Id,
                                    IsDeletedAsMatch = false
                                });
                            }
                            else
                            {
                                await _context.AcceptedPersonWithMatchingRecord.AddAsync(new AcceptedPersonWithMatchingRecord
                                {
                                    AcceptedPersonId = existsInAccepted.Id,
                                    PersonId = person.Id,
                                    IsDeletedAsMatch = false
                                });
                            }

                        }
                    }
                }
                var category = await _context.Categories.Where(s => s.Id == model.CategoryId).FirstOrDefaultAsync();
                await _context.ActivityLogs.AddAsync(new ActivityLog
                {
                    LogBy = "",
                    LogDate = DateTime.Now,
                    LogDescription = $"Uploaded file ''{model.FileName}'' to {category?.CategoryName} {category.CategoryLevel}",
                    LogDetails = "",
                    LogType = "Uploaded"
                });
                _context.SaveChanges();

                return Ok(new
                {
                    IsSuccessful = true,
                    Data = model
                });
            }
            else
            {
                return Ok(new
                {
                    IsSuccessful=false,
                    Message="Duplicate File Name."
                });
            }

        }
        [HttpPost("/getFiles")]
        public async Task<IActionResult> GetFiles(int catId)
        {
            var getFiles = await _context.FileUploads.Where(s => s.CategoryId == catId && !s.IsDeleted).ToListAsync();
            return Ok(new
            {
                IsSucccessful = true,
                Data = getFiles
            }
            );
        }
        [HttpPost("/downloadFile")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var getFile = await _context.FileUploads.FirstOrDefaultAsync(s => s.Id == id);

            if (getFile == null)
            {
                return NotFound(); // Return a 404 Not Found response if the file is not found
            }

            string appRootPath = _hostingEnvironment.ContentRootPath;
            string filePath = Path.Combine(appRootPath, "wwwroot\\UploadedFiles", getFile.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Return a 404 Not Found response if the file is not found on disk
            }

            // Return the file to the client
            return PhysicalFile(filePath, getFile.FileContentType, getFile.FileName);
        }

        [HttpDelete("/deleteFile")]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            var getFile = await _context.FileUploads.Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == id);

            if (getFile == null)
            {
                return NotFound(); // Return a 404 Not Found response if the file is not found
            }

            string appRootPath = _hostingEnvironment.ContentRootPath;
            string fullPath = Path.Combine(appRootPath, "wwwroot\\UploadedFiles", getFile.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            else
            {
                return NotFound();
            }

            getFile.IsDeleted = true;

            _context.FileUploads.Update(getFile);

            await _context.ActivityLogs.AddAsync(new ActivityLog
            {
                LogBy = "User",
                LogDate = DateTime.Now,
                LogDescription = $"Deleted file '{getFile.FileName}' with category '{getFile.Category.CategoryName}'",
                LogDetails = $"Deleted file '{getFile.FileName}' with category '{getFile.Category.CategoryName}'",
                LogType = "File Deleted"
            });

            await _context.SaveChangesAsync(); // Use SaveChangesAsync instead of SaveChanges

            return Ok(new
            {
                IsSuccessful = true,
            });
        }

    }



}

