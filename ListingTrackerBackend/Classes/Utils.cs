using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace ListingTracker.Classes
{
    public static class Utils
    {
        public static void SaveFileFromBase64(byte[] fileBytes, string relativePath)
        {
            try
            {
              
                System.IO.File.WriteAllBytes(relativePath, fileBytes);

                Console.WriteLine("File saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }
        public static void CopyProperties(object source, object destination)
        {
            PropertyInfo[] sourceProperties = source.GetType().GetProperties();
            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                foreach (var destinationProperty in destinationProperties)
                {
                    if (sourceProperty.Name == destinationProperty.Name &&
                        sourceProperty.PropertyType == destinationProperty.PropertyType &&
                        destinationProperty.CanWrite)
                    {
                        destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                        break;
                    }
                }
            }
        }
    }
    
}
