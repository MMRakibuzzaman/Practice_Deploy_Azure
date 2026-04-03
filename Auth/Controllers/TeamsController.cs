using Auth.Models;
using Auth.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public TeamsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        private async Task<string> UploadFile(IFormFile file)
        {
            string uploadFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "Images");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            string uniqueFileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";

            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        [HttpGet]
        public IActionResult GetTeam()
        {
            var teams = _db.Teams.Include(x => x.Players).ToList();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public IActionResult GetTeamById(int id)
        {
            var team = _db.Teams.Include(x => x.Players).FirstOrDefault(x => x.TeamId == id);

            if (team == null)
                return NotFound();

            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> PostTeam([FromForm] TeamVM tvm)
        {
            var team = new Team()
            {
                TeamName = tvm.TeamName,
                Established = tvm.Established,
                Revenue = tvm.Revenue,
                IsActive = tvm.IsActive,
                Players = tvm.Players.Select(name => new Player { PlayerName = name }).ToList(),
            };
            if (tvm.Image != null)
            {
                team.TeamLogo = await UploadFile(tvm.Image);
            }

            _db.Teams.Add(team);
            await _db.SaveChangesAsync();
            return Ok(team);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, [FromForm] TeamVM tvm)
        {
            var team = await _db.Teams.Include(x => x.Players).FirstOrDefaultAsync(x => x.TeamId == id);

            if (team == null)
                return NotFound();

            team.TeamName = tvm.TeamName;
            team.Established = tvm.Established;
            team.Revenue = tvm.Revenue;
            team.IsActive = tvm.IsActive;

            if (tvm.Image != null)
            {
                team.TeamLogo = await UploadFile(tvm.Image);
            }

            _db.Players.RemoveRange(team.Players);
            team.Players = tvm.Players.Select(name => new Player { PlayerName = name }).ToList();

            await _db.SaveChangesAsync();
            return Ok(team);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTeam(int id)
        {
            var team = _db.Teams.Include(x => x.Players).FirstOrDefault(x => x.TeamId == id);

            if (team == null)
                return NotFound();

            _db.Players.RemoveRange(team.Players);
            _db.Teams.Remove(team);
            _db.SaveChanges();
            return Ok("Team Deleted");
        }
    }
}