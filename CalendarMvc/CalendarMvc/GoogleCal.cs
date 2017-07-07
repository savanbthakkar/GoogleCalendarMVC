using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace CalendarMvc


{
    public class GoogleCal : ICalendarAPI<Event>
    {
        public List<string> EditedIds = new List<string>();
        public List<string> Ids = new List<string>();
        public List<string> DeletedList = new List<string>();
        public bool loggedin;


        public CalendarService Service;

        public void Login()
        {
            const string clientId = "544981493845-gfu06a07tmtmnma4ml5ndda9623b00nv.apps.googleusercontent.com";
            const string clientSecret = "cwyU5_sohFiGvMtEeFOricwD";
            var userName = RandomString(); // A string used to identify a user. currently requests a login for every action... probably better solutions out there

            IEnumerable<string> scopes = new[]
            {
                CalendarService.Scope.Calendar,
                CalendarService.Scope.CalendarReadonly
            };

            var cred = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                }, scopes, userName, CancellationToken.None,
                new FileDataStore("ICalendar store")
            );

            var credential = cred.Result;

            Service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Calendar API Sample"
            });
            loggedin = true;
        }

        public Event CreateAppointment(Event e)
        {
            Service.Events.Insert(e, "primary").Execute();

            //Debug.WriteLine(EventDetails(e));
            EditedIds.Add(e.Id);
            Ids.Add(e.Id);
            return e;
        }

        public void EditAppointment(string ev)
        {
            var e = Service.Events.Get("primary", ev).Execute();
            Service.Events.Update(e, "primary", ev).Execute();
            EditedIds.Add(ev);
        }

        public bool Deleted(string eventId)
        {
            return DeletedList.Contains(eventId);

        }
        public List<string> CheckRange(DateTime startDateTime, DateTime endDateTime)
        {
            var items = Service.Events.List("primary");
            items.TimeMin = startDateTime;
            items.TimeMax = endDateTime;
            var events = items.Execute().Items;

            return events.Select(e => e.Id).ToList();
        }

        public List<string> CheckEntity(string entityId)
        {
            var now = DateTime.Now;
            var week = DateTime.Now.AddDays(7);
            var range = CheckRange(now, week);

            for (var i = 0; i < range.Count; i++)
            {
                if (Service.Events.Get("primary", range[i]).Execute().ExtendedProperties.Private__["entityId"] ==
                    entityId) continue;
                range.Remove(range[i]);
                i--;
            }

            return range;
        }

        public void DeleteAppointment(string eventId)
        {
            Service.Events.Delete("primary", eventId).Execute();
            EditedIds.Add(eventId);
            Ids.Remove(eventId);
            DeletedList.Add(eventId);
        }

        public Event CreateEvent(string guestemail, string summary, string location, string description,
            DateTime startTime,
            DateTime endTime, string zone, string entityId, string name)
        {
            IDictionary<string, string> d = new Dictionary<string, string>
                {{"entityId", entityId}, {"guestemail", guestemail}, {"guestname", name}};

            var e = new Event
            {
                Summary = summary,
                Location = location,

                ExtendedProperties = new Event.ExtendedPropertiesData
                {
                    Private__ = d
                },
                Start = new EventDateTime
                {
                    DateTime = startTime,
                    TimeZone = zone
                },

                End = new EventDateTime
                {
                    DateTime = endTime,
                    TimeZone = zone
                },
                Id = RandomString()
            };

            return e;
        }

        public Channel Watch()
        {
            var body = new Channel();
            return Service.Events.Watch(body, "primary").Execute();
        }

        public List<string> GetEdited()
        {
            var temp = EditedIds.ToList();
            EditedIds.Clear();
            return temp;
        }


        public string EventDetails(string ei)
        {
            var e = Service.Events.Get("primary", ei).Execute();

            var start = e.Start.DateTimeRaw;
            var end = e.End.DateTimeRaw;
            var detes =
                $"EventId: {e.Id} \nTitle: {e.Summary}\nLocation: {e.Location}\nStart Time: {start}\nEnd Time: {end}\n"; //Entity ID: " +
            //  $"{_service.Events.Get("primary",ei).Execute().ExtendedProperties.Private__["entityId"]}\n";

            return detes;
        }

        //used to generate Google.Apis.Calendar.v3.Data.EventIds
        public string RandomString()
        {
            var r = new Random();
            const string input = "abcdefghijklmnopqrstuv0123456789";
            var builder = new StringBuilder();
            for (var i = 0; i < 40; i++)
            {
                var ch = input[r.Next(0, input.Length)];
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}