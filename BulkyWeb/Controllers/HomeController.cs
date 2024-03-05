using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulkyWeb.Models;

namespace BulkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly StackOverflow2010Context _context;

        public HomeController(StackOverflow2010Context context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index(string searchTerm, int? page)
        {
            const int pageSize = 10;
            int pageNumber = page ?? 1;

            try
            {

                var query = from p in _context.Posts
                            join u in _context.Users on p.OwnerUserId equals u.Id into userJoin
                            from user in userJoin.DefaultIfEmpty()
                            join pa in _context.Posts on p.ParentId equals pa.Id into parentJoin
                            from parent in parentJoin.DefaultIfEmpty()
                            join pu in _context.Users on parent.OwnerUserId equals pu.Id into parentUserJoin
                            from parentUser in parentUserJoin.DefaultIfEmpty()
                            join b in _context.Badges on (p.PostTypeId == 2 ? parent.OwnerUserId : user.Id) equals b.UserId into badgeJoin

                            select new PostViewModel
                            {
                                Id = p.Id,
                                PostTypeId = p.PostTypeId,
                                Title = p.PostTypeId == 2 ? parent.Title : p.Title,
                                Body = p.Body,
                                AnswerCount = (int)(p.PostTypeId == 2 ? parent.AnswerCount : p.AnswerCount),
                                VoteCount = _context.Votes.Count(v => v.PostId == p.Id),
                                OwnerDisplayName = p.PostTypeId == 2 ? parentUser.DisplayName : user.DisplayName,
                                OwnerReputation = p.PostTypeId == 2 ? parentUser.Reputation : user.Reputation,
                                Badges = string.Join(", ", badgeJoin.Select(b => b.Name).Distinct())
                            };

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(p => p.Title.Contains(searchTerm));
                    ViewData["searchTerm"] = searchTerm;
                }

                var posts = await PaginatedList<PostViewModel>.CreateAsync(query, pageNumber, pageSize);
                return View(posts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return View("Error");
            }
        }
    }
}
