﻿using BlogASP.Models;
using BlogASP.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogASP.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IArticleRepository _articleRepository;

        public AdminPanelController(IUserRepository userRepository, IArticleRepository articleRepository)
        {
            _userRepository = userRepository;
            _articleRepository = articleRepository;
        }

        public IActionResult Index()
        {
            var viewModel = new AdminPanelViewModel
            {
                Users = _userRepository.GetAll(),
                Articles = _articleRepository.GetAllArticles()
            };
            return View(viewModel);
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

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_userRepository.Delete(id))
                {
                    return Ok(); 
                }
                else
                {
                    return BadRequest("Deleting Error:");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Deleting Error: " + ex.Message); 
            }
        }

        [HttpPost]
        public IActionResult Create(UserModel user) 
        {
            //if (ModelState.IsValid)
            //{
            //    _userRepository.Create(user);
            //    return RedirectToAction("Index");
            //}
            //return View(user);
            //
            _userRepository.Create(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(UserModel user)
        {
            _userRepository.Edit(user);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult SetAsPrivileged(int id)
        {
            UserModel user = _userRepository.ListById(id);

            if (user != null && (user.Role == "EndUser" || user.Role == "Administrator"))
            {
                user.Role = "Privileged";
                _userRepository.Edit(user);

                return RedirectToAction("Index");
            }

            return View("Error");
        }

        [HttpPost]
        public IActionResult SetAsEndUser(int id)
        {
            UserModel user = _userRepository.ListById(id);

            if (user != null && (user.Role == "Privileged" || user.Role == "Administrator"))
            {
                user.Role = "EndUser";
                _userRepository.Edit(user);

                return RedirectToAction("Index");
            }

            return View("Error");
        }

        [HttpPost]
        public IActionResult SetAsAdministrator(int id)
        {
            UserModel user = _userRepository.ListById(id);

            if (user != null && user.Role == "Privileged")
            {
                user.Role = "Administrator";
                _userRepository.Edit(user);

                return RedirectToAction("Index");
            }

            return View("Error");
        }
    }
}
