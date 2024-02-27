using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ListingTracker.DbEntities;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ListingTracker.Handlers
{
    public static class ExcelHandler
    {
        public static List<Person> GetPersonListFromExcelFile(string path)
        {
            FileStream fileStream = GetStreamFromExcelFile(path);
            XSSFWorkbook workbook = GetWorkbookFromStream(fileStream);
            ISheet worksheet = GetWorksheetFromWorkbook(workbook);
            List<Person> personList = GetPersonListFromWorksheet(worksheet);
            return personList;
        }

        public static FileStream GetStreamFromExcelFile(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            return fileStream;
        }

        public static XSSFWorkbook GetWorkbookFromStream(Stream stream)
        {
            XSSFWorkbook workbook = new XSSFWorkbook(stream);
            return workbook;
        }

        public static ISheet GetWorksheetFromWorkbook(IWorkbook workbook)
        {
            ISheet worksheet = workbook.GetSheetAt(0);
            return worksheet;
        }

        public static List<Person> GetPersonListFromWorksheet(ISheet worksheet)
        {

            List<Person> personList = new List<Person>();

            // Loop through all rows

            for (int row = 0; row <= worksheet.LastRowNum; row++)
            {
                var currentRow = worksheet.GetRow(row);

                if (row == 0)
                {
                    // header row
                    continue;
                }


                if (currentRow == null)
                {
                    continue;
                }

                Person person = new Person();

                // Loop through all cells in the row and get the values of each cell in the row and assign them to the person object

                personList.Add(person);
            }


            return personList;

        }


        public static void WriteToWorkbook(XSSFWorkbook workbook, string path)
        {
            using (FileStream outputStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(outputStream);
            }
        }

    }
}
