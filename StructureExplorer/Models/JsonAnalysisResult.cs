namespace StructureExplorer.Models
{
    public class JsonAnalysisResult
    {
        public List<JsonNodeInfo> RootNodes { get; set; } = [];
        public List<SearchResult> SearchResults { get; set; } = [];
        
        public int ObjectCount { get; set; }
        public int ArrayCount { get; set; }
        public int PrimitiveCount { get; set; }
        public int MaxDepth { get; set; }
    }
}