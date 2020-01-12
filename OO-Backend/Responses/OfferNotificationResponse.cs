﻿using OO_Backend.Enums;
using OO_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Responses
{
    public class OfferNotificationResponse
    {
        public int Id { get; set; }
        public ShortUserResponse SendUser { get; set; }
        public ShortUserResponse ReceivedUser { get; set; }
        public RequestAdResponse RequestAd { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime PostDate { get; set; }
    }
}
