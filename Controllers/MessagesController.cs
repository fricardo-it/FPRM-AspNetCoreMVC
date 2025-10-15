using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPRMAspNetCoreMVC.Data;
using FPRMAspNetCoreMVC.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FPRMAspNetCoreMVC.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;


        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Messages1
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentUser = new Guid();
 
            if (userId == null)
            {
                return NotFound();
            }
            if (Guid.TryParse(userId, out Guid userIdGuid))
            {
                currentUser = userIdGuid;
            }
            else
            {
                return NotFound();

            }

            var userMessages = await _context.Message
                .Include(m => m.Building)
                .Include(m => m.SenderMsg)
                .Where(m => m.SenderMsgId == currentUser)
                .ToListAsync();

            return View(userMessages);



            //var applicationDbContext = _context.Message.Include(m => m.Building)
            //    .Include(m => m.SenderMsg);
            ////var administrators = await _context.User.ToListAsync();

            ////administrators = administrators.Where(u => u.UserRole.ToString() != "Manager" && u.UserRole.ToString() != "Tenant").ToList();


            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Messages1/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Building)
                .Include(m => m.SenderMsg)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            bool alignRight = message.LastReplySenderId == message.BuildingManagerId;

            ViewBag.AlignRight = alignRight;

            return View(message);
        }


        // GET: Messages1/Create
        public IActionResult Create()
        {
            ViewData["BuildingId"] = new SelectList(_context.Building, "Id", "Name");
            ViewData["SenderMsgId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: Messages1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("BuildingId,SenderMsgId,Content,Title,Timestamp,Replies,Id")]
            Message message)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        return NotFound();
                    }
                    if (Guid.TryParse(userId, out Guid userIdGuid))
                    {
                        message.SenderMsgId = userIdGuid;
                    }
                    else
                    {
                        return NotFound();

                    }
                    
                    var buildingId = message.BuildingId;

                    var managerId = _context.Building
                        .Where(b => b.Id == buildingId)
                        .Select(b => b.ManagerId)
                        .FirstOrDefault();
                }
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BuildingId"] = new SelectList(_context.Building, "Id", "Name", message.BuildingId);
            ViewData["SenderMsgId"] = new SelectList(_context.User, "Id", "Email", message.SenderMsgId);
            return View(message);
        }

        // GET: Messages1/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            ViewData["BuildingId"] = new SelectList(_context.Building, "Id", "Name", message.BuildingId);
            ViewData["SenderMsgId"] = new SelectList(_context.User, "Id", "Email", message.SenderMsgId);
            return View(message);
        }

        // POST: Messages1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("BuildingId,SenderMsgId,Content,Title,Timestamp,Replies,Id")]
            Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["BuildingId"] = new SelectList(_context.Building, "Id", "Name", message.BuildingId);
            ViewData["SenderMsgId"] = new SelectList(_context.User, "Id", "Email", message.SenderMsgId);
            return View(message);
        }

        // GET: Messages1/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Building)
                .Include(m => m.SenderMsg)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var message = await _context.Message.FindAsync(id);
            if (message != null)
            {
                _context.Message.Remove(message);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(Guid id)
        {
            return _context.Message.Any(e => e.Id == id);
        }

        // GET: Messages/Reply/5
        public async Task<IActionResult> Reply(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            var viewModel = new ReplyViewModel();
            return View(viewModel);
        }



        // POST: Messages/Reply/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(Guid id, [Bind("ReplyContent")] ReplyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var message = await _context.Message.FindAsync(id);
                if (message == null)
                {
                    return NotFound();
                }


                string fullReply = $"{User.Identity.Name}: {viewModel.ReplyContent}";


                message.Replies.Add(fullReply);

                _context.Update(message);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = id });
            }

            return View(viewModel);
        }


        // GET: Messages/Forward/5
        public async Task<IActionResult> Forward(Guid? id)
        {

            var administrators = await _context.User.ToListAsync(); 
            
            administrators = administrators.Where(u => u.UserRole.ToString() != "Manager" && u.UserRole.ToString() != "Tenant").ToList();


            if (administrators.Any())
            {
                ViewBag.Administrators = new SelectList(administrators, "Id", "Name");
            }
            else
            {
                ViewBag.Administrators = new List<SelectListItem>(); 
            }

            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            var viewModel = new ForwardViewModel
            {
                OriginalMessageId = message.Id,
                ForwardedMessageContent = message.Content,
            };

            return View(viewModel);
        }

        // POST: Messages/Forward/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Forward(Guid id, [Bind("ForwardedToId, ForwardedMessageContent")] ForwardViewModel viewModel)
        {
            var originalMessage = await _context.Message.FindAsync(id);
            if (originalMessage == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var forwardedMessage = new Message
                {
                    BuildingId = originalMessage.BuildingId,
                    SenderMsgId = originalMessage.SenderMsgId,
                    Content = viewModel.ForwardedMessageContent,
                    Title = "Forwarded: " + originalMessage.Title, 
                    Timestamp = DateTime.Now,
                    BuildingManagerId = originalMessage.SenderMsgId,
                    LastReplySenderId = originalMessage.SenderMsgId
                };

                _context.Add(forwardedMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.OriginalMessage = originalMessage;
            return View(viewModel);
        }
    }
}
