using System.Text.Json.Nodes;
using StructureExplorer.Models;

namespace StructureExplorer.Services
{
    public class JsonAnalyzerService : IJsonAnalyzerService
    {
        private readonly CSharpNamingService _namingService;
        
        public JsonAnalyzerService(CSharpNamingService namingService)
        {
            _namingService = namingService;
        }
        
        public JsonAnalysisResult Analyze(string json)
        {
            JsonAnalysisResult result = new();
            
            JsonNode? root = JsonNode.Parse(json);

            if (root == null)
            {
                return result;
            }
            
            JsonNodeInfo rootNode = Traverse(root, result, "$", "$", -1);
            
            result.Root = rootNode;
            
            return result;
        }

        private JsonNodeInfo Traverse(JsonNode node, JsonAnalysisResult result, string name, string path, int depth)
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
                        Depth = depth,
                        
                        SuggestedCSharpName = _namingService.ConvertToPropertyName(name),
                        SuggestedCSharpType = _namingService.ConvertToPropertyName(name)
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
                        ChildCount = array.Count,
                        
                        SuggestedCSharpType = $"List<{_namingService.ConvertToPropertyName(_namingService.Singularize(name))}>"
                    };

                    if (array.Count > 0)
                    {
                        JsonNode? firstItem = array[0];

                        if (firstItem != null)
                        {
                            JsonNodeInfo sample = Traverse(firstItem, result, "Item Structure", $"{path}[0]", depth + 1);
                            
                            sample.IsSample = true;
                            
                            info.Children.Add(sample);
                        }
                    }
                    
                    break;
                
                case JsonValue value:
                    result.PrimitiveCount++;
                    
                    string type = value.GetValueKind().ToString();
                    
                    info = new JsonNodeInfo
                    {
                        Name = name,
                        Path = path,
                        NodeType = "Value",
                        ValueType = type,
                        ValuePreview = value.ToString(),
                        Depth = depth,
                        
                        SuggestedCSharpName = _namingService.ConvertToPropertyName(name),
                        SuggestedCSharpType = type switch
                        {
                            "String" => "string",
                            "Number" => "int",
                            "True" => "bool",
                            "False" => "bool",
                            _ => "object"
                        }
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

            return info;
        }
    }
}