﻿using BlogASP.Models;
using BlogASP.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlogASP.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AdminPanelController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            List<UserModel> users = _userRepository.GetAll();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            UserModel user = _userRepository.ListById(id);
            return View(user);
        }

        public IActionResult Show()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            UserModel user = _userRepository.ListById(id);
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserModel user) 
        {
            _userRepository.Create(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(UserModel user)
        {
            _userRepository.Edit(user);
            return RedirectToAction("Index");
        }

        public IActionResult Erase(int id)
        {
            _userRepository.Erase(id);
            return RedirectToAction("Index");
        }

        public IActionResult PendingUsers()
        {
            List<UserModel> endUsers = _userRepository.GetAll().Where(u => u.Role == "EndUser").ToList();
            return View(endUsers);
        }

    }
}
