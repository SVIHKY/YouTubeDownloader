using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace YouTubeDownloader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        [HttpPost]
        public IActionResult Download([FromBody] DownloadRequest request)
        {
            if (string.IsNullOrEmpty(request.Url))
                return BadRequest(new { success = false, error = "URL is required." });

            try
            {
                // Plná cesta k yt-dlp.exe
                string ytDlpPath = @"C:\yt-dlp\yt-dlp.exe"; // Změň na správnou cestu, kde je yt-dlp.exe uložen
                if (!System.IO.File.Exists(ytDlpPath))
                {
                    return StatusCode(500, new { success = false, error = "yt-dlp.exe was not found. Check the path." });
                }

                string fileExtension = request.Format == "mp3" ? "mp3" : "mp4";
                string fileName = $"downloaded_video.{fileExtension}";
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

                // Příkaz pro yt-dlp
                string arguments = $"\"{request.Url}\" -f {request.Quality} -o \"{outputPath}\"";
                if (request.Format == "mp3")
                    arguments += " --extract-audio --audio-format mp3";

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ytDlpPath,
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                process.WaitForExit();

                if (System.IO.File.Exists(outputPath))
                {
                    return Ok(new { success = true, fileName, filePath = $"/{fileName}" });
                }
                else
                {
                    return BadRequest(new { success = false, error = "Failed to download the video." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }

    public class DownloadRequest
    {
        public string Url { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string Quality { get; set; } = string.Empty;
    }
}
