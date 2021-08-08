using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MTApi.Services.Artist
{
    // View model for artist post requests
    public class ArtistVM
    {
        [Required]
        public string ArtistTitle { get; set; }
        [Required]
        public string Bio { get; set; }
        [Required]
        public string ImageHyperLink { get; set; }
        [Required]
        public string heroHyperLink { get; set; }
    }
}
