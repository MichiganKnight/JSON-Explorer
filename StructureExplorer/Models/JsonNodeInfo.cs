namespace StructureExplorer.Models
{
    public class JsonNodeInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string NodeType { get; set; } = string.Empty;
        
        public string? ValueType { get; set; }
        
        public int Depth { get; set; }
        
        public int? ChildCount { get; set; }

        public List<JsonNodeInfo> Children { get; set; } = new(0);
        
        public bool HasMoreChildren { get; set; }
        
        public int RemainingChildren { get; set; }
    }
}