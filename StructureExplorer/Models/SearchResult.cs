namespace StructureExplorer.Models
{
    public class SearchResult
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string NodeType { get; set; } = string.Empty;
        
        public string? ValueType { get; set; }
    }
}