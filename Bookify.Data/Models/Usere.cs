using System;
using System.Collections.Generic;

namespace Bookify.Data.Models;

public partial class Usere
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Epassword { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? UserRole { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();
}
