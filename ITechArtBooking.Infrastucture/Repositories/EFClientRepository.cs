﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITechArtBooking.Domain.Interfaces;
using ITechArtBooking.Domain.Models;

namespace ITechArtBooking.Infrastucture.Repositories
{
    public class EFClientRepository : IClientRepository
    {
        private readonly EFBookingDBContext Context;

        public EFClientRepository(EFBookingDBContext context)
        {
            Context = context;
        }

        public IEnumerable<Client> GetAll()
        {
            return Context.Clients;
        }

        public Client Get(long id)
        {
            return Context.Clients.Find(id);
        }

        public void Create(Client client)
        {
            Context.Clients.Add(client);
            Context.SaveChanges();
        }

        public void Update(Client updatedClient)
        {
            Client currentClient = Get((int)updatedClient.Id);

            currentClient.LastName = updatedClient.LastName;
            currentClient.FirstName = updatedClient.FirstName;
            currentClient.MiddleName = updatedClient.MiddleName;
            currentClient.PhoneNumber = updatedClient.PhoneNumber;

            Context.Clients.Update(currentClient);
            Context.SaveChanges();
        }

        public Client Delete(long id)
        {
            Client client = Get(id);

            if(client != null) {
                Context.Clients.Remove(client);
                Context.SaveChanges();
            }

            return client;
        }
    }
}