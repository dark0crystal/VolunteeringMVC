using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace volunteeringMVC.Models;

public partial class Register
{
   
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } 
    [Required]
    public string FamilyName { get; set; } 

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } 

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(15,MinimumLength =8,ErrorMessage ="Invalid 555")]
    public string Password { get; set; }

    public virtual ICollection<VolunteeringUser> VolunteeringUsers { get; set; } = new List<VolunteeringUser>();
}
