using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

    namespace wedding_planner.Models
    {
        public class Wedding
        {
            [Key]
            public int WeddingId {get;set;}
            public int UserId {get;set;}
            public string Bride {get;set;}
            public List<RSVP> Guests { get; set;}
            public User User {get;set;}
            public string Groom {get;set;}
            public System.DateTime Date {get;set;}
            public string Address {get;set;}
            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;
        }
    }