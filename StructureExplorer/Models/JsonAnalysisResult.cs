namespace StructureExplorer.Models
{
    public class JsonAnalysisResult
    {
        public JsonNodeInfo? Root { get; set; }
        
        public int ObjectCount { get; set; }
        public int ArrayCount { get; set; }
        public int PrimitiveCount { get; set; }
        public int MaxDepth { get; set; }
    }
}