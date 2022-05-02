﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EF.Model;
using LostAndFoundAppBackend.DTOs;

namespace LostAndFoundAppBackend.Repository
{
    public interface IAdvertisementRepository
    {
        public Task<ActionResult<Account>> findAccById(int id);

        public Task<ActionResult<Advertisement>> findAdvById(int id);

        public Task<int> save(CreateAdvertisementDto adv);


    }
}