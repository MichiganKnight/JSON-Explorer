using System.Text.RegularExpressions;

namespace StructureExplorer.Services
{
    public class CSharpNamingService
    {
        public string ConvertToPropertyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "Unknown";
            }

            string[] parts = Regex.Split(name, @"[_\-\s]");

            return string.Join("", parts.Select(p => char.ToUpper(p[0]) + p.Substring(1)));
        }
        
        public string Singularize(string name)
        {
            if (name.EndsWith("ies"))
            {
                return name[..^3] + "y";
            }
            
            if (name.EndsWith("s"))
            {
                return name[..^1];
            }
            
            return name;
        }
    }
}