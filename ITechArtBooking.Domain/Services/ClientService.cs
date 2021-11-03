﻿using ITechArtBooking.Domain.Interfaces;
using ITechArtBooking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITechArtBooking.Domain.Services
{
    public class ClientService
    {
        private readonly IClientRepository clientRepository;
        public ClientService(IClientRepository _clientsRepository)
        {
            clientRepository = _clientsRepository ?? throw new ArgumentNullException(nameof(clientRepository));
        }
        public IEnumerable<Client> GetAll()
        {
            return clientRepository.GetAll();
        }
    }
}
