using System.ComponentModel.DataAnnotations;

namespace StructureExplorer.ViewModels
{
    public class AnalyzeViewModel
    {
        [Display(Name = "JSON URL")]
        [Url(ErrorMessage = "Please Enter a Valid URL")]
        public string? JsonUrl { get; set; }
        
        [Display(Name = "JSON")]
        public string? JsonText { get; set; }
    }
}