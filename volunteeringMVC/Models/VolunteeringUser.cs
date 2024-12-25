using System;
using System.Collections.Generic;

namespace volunteeringMVC.Models;

public partial class VolunteeringUser
{
    public int UserId { get; set; }

    public int PostId { get; set; }

    public DateTime VolunteeredAt { get; set; }

    public virtual VolunteeringPost Post { get; set; } = null!;

    public virtual Register User { get; set; } = null!;
}
