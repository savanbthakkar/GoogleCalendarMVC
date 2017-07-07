
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
//using Newtonsoft.Json.Linq;


namespace CalendarMvc
{
    internal interface ICalendarAPI<T>
    {
        void Login();
        Event CreateAppointment(T obj);
        void EditAppointment(string eventId);
        List<string> CheckRange(DateTime startDateTime, DateTime endDateTime);
        List<string> CheckEntity(string entityId);
        void DeleteAppointment(string eventId);
    }
}