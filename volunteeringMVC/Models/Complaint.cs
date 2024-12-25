using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace volunteeringMVC.Models;

public partial class Complaint
{
    public int Id { get; set; }

    public int PostId { get; set; }
    [Required]
    public string ComplaintType { get; set; } = null!;
    [Required]
    public string ComplaintText { get; set; } = null!;

    public virtual VolunteeringPost Post { get; set; } = null!;
}
