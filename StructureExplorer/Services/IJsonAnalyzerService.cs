using StructureExplorer.Models;

namespace StructureExplorer.Services
{
    public interface IJsonAnalyzerService
    {
        JsonAnalysisResult Analyze(string json);
    }
}