using System.Text;
using StructureExplorer.Models;

namespace StructureExplorer.Services
{
    public class CSharpClassGenerator
    {
        private readonly StringBuilder _builder = new();
        private readonly HashSet<string> _generatedClasses = [];
        
        private bool _isFirstClass = true;

        public string Generate(JsonNodeInfo root)
        {
            _builder.Clear();
            _generatedClasses.Clear();
            
            _isFirstClass = true;

            GenerateClass(root, "Root");
            
            return _builder.ToString();
        }

        private void GenerateClass(JsonNodeInfo node, string className)
        {
            if (_generatedClasses.Contains(className))
            {
                return;
            }
            
            _generatedClasses.Add(className);
            
            if (!_isFirstClass)
            {
                _builder.AppendLine();
            }

            _isFirstClass = false;

            _builder.AppendLine($"public class {className}");
            _builder.AppendLine("{");

            List<JsonNodeInfo> nestedObjects = [];

            foreach (JsonNodeInfo child in node.Children)
            {
                _builder.AppendLine(
                    $"    public {child.SuggestedCSharpType} {child.SuggestedCSharpName} {{ get; set; }}"
                );

                if (child.NodeType == "Object")
                {
                    nestedObjects.Add(child);
                }

                if (child.NodeType == "Array" && child.Children.Any())
                {
                    JsonNodeInfo sample = child.Children.First();
                    
                    if (sample.NodeType == "Object")
                    {
                        nestedObjects.Add(sample);
                    }
                }
            }
            
            _builder.AppendLine("}");
            _builder.AppendLine();

            foreach (JsonNodeInfo child in nestedObjects)
            {
                string classNameToGenerate;

                if (child.NodeType == "Array")
                {
                    classNameToGenerate = child.SuggestedCSharpType.Replace("List<", "").Replace(">", "");
                }
                else
                {
                    classNameToGenerate = child.SuggestedCSharpName;
                }
                
                GenerateClass(child, classNameToGenerate);
            }
        }
    }
}