using System;
using System.IO;


namespace Ecommerce.util
{
    public class PropertyUtil
    {
        public static string GetPropertyString(string connectionFile)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory,connectionFile);
            
         
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Property file '{connectionFile}' not found.");
            }
            string[] lines = File.ReadAllLines(filePath);
            string connectionString = "";
            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                {
                    connectionString += $"{parts[0].Trim()}={parts[1].Trim()};";
                }
            }
            return connectionString;
        }
    }
}
