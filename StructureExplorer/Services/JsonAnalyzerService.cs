using System.Text.Json.Nodes;
using StructureExplorer.Models;

namespace StructureExplorer.Services
{
    public class JsonAnalyzerService : IJsonAnalyzerService
    {
        public JsonAnalysisResult Analyze(string json)
        {
            JsonAnalysisResult result = new();
            
            JsonNode? root = JsonNode.Parse(json);

            if (root == null)
            {
                return result;
            }

            Traverse(root, result, "$", "$", 0);
            
            return result;
        }

        private void Traverse(JsonNode node, JsonAnalysisResult result, String name, string path, int depth)
        {
            result.MaxDepth = Math.Max(result.MaxDepth, depth);

            switch (node)
            {
                case JsonObject obj:
                    result.ObjectCount++;
                    
                    result.Nodes.Add(new JsonNodeInfo
                    {
                        Name = name,
                        Path = path,
                        NodeType = "Object",
                        Depth = depth
                    });

                    foreach (KeyValuePair<string, JsonNode?> property in obj)
                    {
                        Traverse(property.Value!, result, property.Key, $"{path}.{property.Key}", depth + 1);
                    }
                    
                    break;
                
                case JsonArray array:
                    result.ArrayCount++;
                    
                    result.Nodes.Add(new JsonNodeInfo
                    {
                        Name = name,
                        Path = path,
                        NodeType = "Array",
                        Depth = depth
                    });

                    int index = 0;

                    foreach (JsonNode item in array)
                    {
                        Traverse(item!, result, $"[{index}]", $"{path}[{index}]", depth + 1);
                        
                        index++;
                    }
                    
                    break;
                
                case JsonValue value:
                    result.PrimitiveCount++;
                    
                    result.Nodes.Add(new JsonNodeInfo
                    {
                        Name = name,
                        Path = path,
                        NodeType = "Value",
                        ValueType = value.GetValueKind().ToString(),
                        Depth = depth
                    });
                    
                    break;
            }
        }
    }
}