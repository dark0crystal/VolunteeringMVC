using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace volunteeringMVC.Models;

public partial class VolunteeringPost
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Category { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public int NumOfDays { get; set; }
    [Required]
    public string Location { get; set; } = null!;
    [Required]
    public string OrgName { get; set; } = null!;
    [Required]
    public DateOnly? StartDate { get; set; }
    [Required]
    public DateOnly? EndDate { get; set; }

    public string PostAdminEmail { get; set; } = null!;

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<VolunteeringUser> VolunteeringUsers { get; set; } = new List<VolunteeringUser>();
}
