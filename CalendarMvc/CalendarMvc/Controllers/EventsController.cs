using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CalendarMvc.Models;
using Google.Apis.Calendar.v3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarMvc.Controllers
{
    public class EventsController : Controller
    {
        public CalendarService Service;
        private readonly CalendarMvcContext _context;

        public GoogleCal Cal = new GoogleCal();

        public EventsController(CalendarMvcContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var e = from m in _context.Event
                select m;

            if (!string.IsNullOrEmpty(searchString))
                e = e.Where(s => s.Title.Contains(searchString));

            return View(await e.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!Cal.loggedin)
                Cal.Login();
            if (id == null)
                return NotFound();

            var e = await _context.Event
                .SingleOrDefaultAsync(m => m.ID == id);
            if (e == null)
                return NotFound();

            return View(e);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,Title,Date,Location,PrimaryAttendeeEmail,PrimaryAttendeeName,EntityId,EventId,Description")]
            Event e)
        {
            if (!Cal.loggedin)
                Cal.Login();
            var t = Cal.CreateEvent(e.PrimaryAttendeeEmail, e.Title, e.Location, "description", e.Date,
                e.Date.AddHours(2), "Etc/GMT", "entityId", e.PrimaryAttendeeName);
            var s = Cal.CreateAppointment(t);
            e.EventId = s.Id;
            e.EntityId = s.ExtendedProperties.Private__["entityId"];
            if (ModelState.IsValid)
            {
                _context.Add(e);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            

            return View(e);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var e = await _context.Event.SingleOrDefaultAsync(m => m.ID == id);
            if (e == null)
                return NotFound();
            return View(e);
        }

        // POST: Events/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ID,Title,Date,Location,PrimaryAttendeeEmail,PrimaryAttendeeName,EventId,EntityId,Description")]
            Event e)
        {
            if (!Cal.loggedin)
                Cal.Login();
            //Cal.EditAppointment(e.EventId);



           // var t = _context.Event.SingleOrDefault(m => m.ID == id);

            if (id != e.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(e);
                    var f = Cal.CreateEvent(e.PrimaryAttendeeEmail, e.Title, e.Location, e.Description, e.Date,
                        e.Date.AddHours(2), "Etc/GMT", "entityId", e.PrimaryAttendeeName);
                    var g = Cal.CreateAppointment(f);
                    try
                    {
                        Cal.DeleteAppointment(e.EventId);
                    }
                    catch (Exception exception)
                    {
                        
                    }
                    
                    g.Id = e.EventId;
                    Cal.EditAppointment(g.Id);
                    // Cal.EditAppointment(e.EventId, g);
                    /*if(e.EventId!=null)
                        Cal.DeleteAppointment(e.EventId);
                    e.EventId = g.Id;*/
                    //Cal.DeleteAppointment(e.EventId);
                    //t.EventId = g.Id;
                    await _context.SaveChangesAsync();
                    //Cal.EditAppointment(g.Id);
                    //Cal.DeleteAppointment(e.EventId);
                    //e.EventId = g.Id;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(e.ID))
                        return NotFound();
                    throw;
                }
                return RedirectToAction("Index");
            }
           
            
            return View(e);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var e = await _context.Event
                .SingleOrDefaultAsync(m => m.ID == id);
            if (e == null)
                return NotFound();

            return View(e);
        }

        // POST: Events/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!Cal.loggedin)
                Cal.Login();
            var e = await _context.Event.SingleOrDefaultAsync(m => m.ID == id);
            _context.Event.Remove(e);
            try
            {
                Cal.DeleteAppointment(e.EventId);
            }
            catch(Exception ex)
            {
            }
            //
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.ID == id);
        }
    }
}