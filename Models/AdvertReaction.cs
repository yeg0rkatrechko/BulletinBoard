using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AdvertReaction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AdvertId { get; set; }
        public Reaction Reaction { get; set; }
    }
    public enum Reaction
    {
         Like = 1,
         Dislike = -1
    }
}
