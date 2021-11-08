﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITechArtBooking.Domain.Models;

namespace ITechArtBooking.Domain.Interfaces
{
    public interface IHotelRepository
    {
        public IEnumerable<Hotel> GetAll();
        public Hotel Get(long id);
        void Create(Hotel hotel);
        void Update(Hotel hotel);
        Hotel Delete(long id);
    }
}