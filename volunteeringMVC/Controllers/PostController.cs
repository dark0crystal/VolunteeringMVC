using Microsoft.AspNetCore.Mvc;
using volunteeringMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Hosting;
namespace volunteeringMVC.Controllers
{
    public class PostController : Controller
    {
        MasterContext context  = new MasterContext();
        public IActionResult AllPosts()
        {
            var posts = context.VolunteeringPosts.ToList(); // Fetch all posts

            var user = (from s in context.Registers
                        where s.Id == 1
                        select s).FirstOrDefault();

            if (user != null)
            {
                int userId = user.Id;
            }
            return View(posts);
        }

        [HttpPost]
        public IActionResult UpdatePost(int id, string title, string description, string category)
        {
            var post = context.VolunteeringPosts.FirstOrDefault(p => p.Id == id);
            if (post != null)
            {
                post.Title = title;
                post.Description = description;
                post.Category = category;

                context.SaveChanges();
                TempData["Message"] = "Post updated successfully.";
            }
            else
            {
                TempData["Message"] = "Post not found.";
            }
            return RedirectToAction("Dashboard");
        }
       [HttpPost]
public IActionResult Volunteer(int PostId)
{
    var email = HttpContext.Session.GetString("UserEmail");
    if (string.IsNullOrEmpty(email))
    {
        return RedirectToAction("Login", "Auth"); // Redirect to login if the admin is not authenticated
    }

    // Searching for userId using the user email
    var user = (from s in context.Registers
                where s.Email == email
                select s).FirstOrDefault();

    if (user == null)
    {
        TempData["ErrorMessage"] = "User not found. Please log in again.";
        return RedirectToAction("Login", "Auth");
    }

    var userId = user.Id;

    // Check if the user has already volunteered for the post.
    var existingRecord = context.VolunteeringUsers
        .FirstOrDefault(v => v.UserId == userId && v.PostId == PostId);

    if (existingRecord == null)
    {
        // Add the new volunteer record.
        var newVolunteering = new VolunteeringUser
        {
            UserId = userId,
            PostId = PostId,
            VolunteeredAt = DateTime.Now
        };

        context.VolunteeringUsers.Add(newVolunteering);
        context.SaveChanges();

        // Set a success message.
        TempData["SuccessMessage"] = "You have successfully volunteered for this post!";
    }
    else
    {
        TempData["InfoMessage"] = "You have already volunteered for this post.";
    }

    return RedirectToAction("AllPosts"); // Redirect back to the posts page or another appropriate page.
}


        public IActionResult Dashboard()
        {
            // Get the admin's email from the session or authentication context
            var Email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(Email))
            {
                return RedirectToAction("Login", "Auth"); // Redirect to login if the admin is not authenticated
            }
            // Searching for userId using the user email
            var user = (from s in context.Registers
                        where s.Email == Email
                        select s).FirstOrDefault();

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please log in again.";
                return RedirectToAction("Login", "Auth");
            }

            var userId = user.Id;

            // Fetch posts created by the admin and their complaints
            var createdPosts = context.VolunteeringPosts
                .Where(post => post.PostAdminEmail == Email)
                .Select(post => new
                {
                    Post = post,
                    Complaints = context.Complaints.Where(c => c.PostId == post.Id).ToList()
                })
                .ToList();

            // Fetch complaints
            var complaints = context.VolunteeringPosts
                .Where(post => post.Complaints.Any(c => c.PostId == post.Id)) 
                .ToList();

      
            var postsUserVolunteeredIn = context.VolunteeringPosts
                .Where(post => post.VolunteeringUsers.Any(user1 => user1.UserId == user.Id))
                .ToList();


            ViewBag.VolunteerIn = postsUserVolunteeredIn;
            ViewBag.CreatedPosts = createdPosts;
            ViewBag.ComplaintsPosts = complaints;

            return View();
        }




        public IActionResult DeletePost(int postId)
        {
            var post = context.VolunteeringPosts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                // Fetch and delete complaints related to the post
                var complaints = context.Complaints.Where(c => c.PostId == postId).ToList();
                context.Complaints.RemoveRange(complaints);

                // Delete the post
                context.VolunteeringPosts.Remove(post);
                context.SaveChanges();

                TempData["Message"] = "Post and related complaints deleted successfully.";
            }
            else
            {
                TempData["Message"] = "Post not found.";
            }

            return RedirectToAction("Dashboard");
        }

        public enum ComplaintType
        {
            InappropriateContent,
            MisleadingInformation,
            Spam,
            Other
        }
        [HttpPost]
        public IActionResult AddComplaint(int PostId, ComplaintType ComplaintType, string ComplaintText)
        {
            if (ModelState.IsValid)
            {
                var complaint = new Complaint
                {
                    PostId = PostId,
                    ComplaintType = ComplaintType.ToString(),
                    ComplaintText = ComplaintText
                };

                context.Complaints.Add(complaint);
                context.SaveChanges();

                TempData["Message"] = "Your complaint has been submitted successfully.";
                return RedirectToAction("AllPosts");
            }

            TempData["Error"] = "There was an issue submitting your complaint. Please try again.";
            return RedirectToAction("AllPosts");
        }



        public IActionResult AddPost()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        [HttpPost]
      
        public IActionResult AddPost(VolunteeringPost post)
        {
            // Get the admin email from the session
            var adminEmail = HttpContext.Session.GetString("UserEmail");
            if (adminEmail == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                post.PostAdminEmail = adminEmail; // Set the admin email
            }

            if (post != null)
            {
                context.VolunteeringPosts.Add(post);
                context.SaveChanges();
                return RedirectToAction("AddPost");
            }

            return View(post);
        }

    }
}
