using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StructureExplorer.Models;
using StructureExplorer.Services;
using StructureExplorer.ViewModels;

namespace StructureExplorer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJsonAnalyzerService _analyzer;
        private readonly IJsonFetcherService _fetcher;

        public HomeController(IJsonAnalyzerService analyzer, IJsonFetcherService fetcher)
        {
            _analyzer = analyzer;
            _fetcher = fetcher;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View(new AnalyzeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(AnalyzeViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.JsonText) && string.IsNullOrWhiteSpace(model.JsonUrl))
            {
                ModelState.AddModelError("", "Please Provide JSON or a URL");
                
                return View(model);
            }

            try
            {
                string? json;

                if (!string.IsNullOrWhiteSpace(model.JsonUrl))
                {
                    json = await _fetcher.FetchFromUrlAsync(model.JsonUrl);
                }
                else
                {
                    json = model.JsonText;
                }

                JsonAnalysisResult result = _analyzer.Analyze(json);

                return View("Results", result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unabled to Process JSON: {ex.Message}");
                
                return View(model);
            }
        }
        
        public IActionResult Results()
        {
            return View();
        }
    }
}