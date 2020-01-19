using System.ComponentModel.DataAnnotations;

namespace Jobsity.Bots.Requests
{
    public class HandshakeRequest
    {
        [Required]
        public string Challenge { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
