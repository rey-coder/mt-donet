using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTApi.Services.Artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public partial class ArtistController : ControllerBase
    {
        private readonly IArtistService artistService;
        public ArtistController(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        [HttpPost("Add")]
        public IActionResult AddArtist([FromBody] ArtistVM artistVM)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try { 
                artistService.AddArtist(artistVM);
                return Ok(new {
                    data = artistVM,
                    Message = "Artist added successfully"
                }); ;
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                {
                    Message = ex.GetBaseException().Message
                });

            }
        }

        [HttpPost("Search")]
        public IActionResult SearchArtist([FromBody] SearchParams search)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var artists = artistService.SearchArtist(search.Name, search.page, search.length);
                return Ok(artists);
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
