using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicineStorage.Data;
using MedicineStorage.Models;
using Microsoft.AspNetCore.Authorization;
using MedicineStorage.Services.Interfaces;
using MedicineStorage.DTOs;
using AutoMapper;

namespace MedicineStorage.Controllers
{
    
    public class UsersController(IUserRepository _userRepository, IMapper _mapper) : BaseApiController
    {
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserKnownDTO>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();

            var usersToReturn = _mapper.Map<IEnumerable<UserKnownDTO>>(users);

            return Ok(usersToReturn); 
        }

        // GET: api/Users/5
        [HttpGet("{username}")]
        public async Task<ActionResult<UserKnownDTO>> GetUser(string username)
        {
            var user = await _userRepository.GetByUserNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            var userToReturn = _mapper.Map<UserKnownDTO>(user);

            return Ok(userToReturn);
        }
    }
}
