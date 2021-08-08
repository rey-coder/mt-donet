using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTApi.Services.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongService songService;
        public SongController(ISongService songService)
        {
            this.songService = songService;
        }

        [HttpPost("song/list")]
        public IActionResult GetSongs([FromBody] Paging paging)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var songs = songService.ListSongs(paging.length, paging.page);
                return Ok(songs); ;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = ex.GetBaseException().Message
                });

            }
        }
    }
}
