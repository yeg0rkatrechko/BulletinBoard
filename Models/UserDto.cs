using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserDto
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool Admin { get; } = false;
    }
}
