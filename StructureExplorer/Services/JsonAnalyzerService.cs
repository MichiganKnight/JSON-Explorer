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

            JsonNodeInfo rootNode = Traverse(root, result, "$", "$", 0);
            
            result.RootNodes.Add(rootNode);
            
            return result;
        }

        private static JsonNodeInfo Traverse(JsonNode node, JsonAnalysisResult result, string name, string path, int depth)
        {
            result.MaxDepth = Math.Max(result.MaxDepth, depth);

            JsonNodeInfo info;

            switch (node)
            {
                case JsonObject obj:
                    result.ObjectCount++;
                    
                    info = new JsonNodeInfo
                    {
                        Name = name,
                        Path = path,
                        NodeType = "Object",
                        Depth = depth
                    };

                    foreach (KeyValuePair<string, JsonNode?> property in obj)
                    {
                        info.Children.Add(Traverse(property.Value!, result, property.Key, $"{path}.{property.Key}", depth + 1));
                    }
                    
                    break;
                
                case JsonArray array:
                    result.ArrayCount++;
                    
                    info = new JsonNodeInfo
                    {
                        Name = name,
                        Path = path,
                        NodeType = "Array",
                        Depth = depth,
                        ChildCount = array.Count
                    };

                    int index = 0;

                    const int maxArrayItems = 5;
                    
                    IEnumerable<JsonNode?> itemsToProcess = array.Take(maxArrayItems);

                    foreach (JsonNode? item in itemsToProcess)
                    {
                        info.Children.Add(Traverse(item!, result, $"[{index}]", $"{path}[{index}]", depth + 1));
                        
                        index++;
                    }

                    if (array.Count > maxArrayItems)
                    {
                        info.HasMoreChildren = true;
                        info.RemainingChildren = array.Count - maxArrayItems;
                    }
                    
                    break;
                
                case JsonValue value:
                    result.PrimitiveCount++;
                    
                    info = new JsonNodeInfo
                    {
                        Name = name,
                        Path = path,
                        NodeType = "Value",
                        ValueType = value.GetValueKind().ToString(),
                        Depth = depth
                    };
                    
                    break;
                
                default:
                    info = new JsonNodeInfo
                    {
                        Name = name,
                        Path = path,
                        NodeType = "Unknown",
                    };
                    
                    break;
            }
            
            result.SearchResults.Add(new SearchResult
            {
                Name = info.Name,
                Path = info.Path,
                NodeType = info.NodeType,
                ValueType = info.ValueType
            });

            return info;
        }
    }
}