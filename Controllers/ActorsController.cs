﻿using eTicket.Data;
using eTicket.Data.Services;
using eTicket.Data.Static;
using eTicket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTicket.Controllers
{
	//[Authorize(Roles = "Admin, User")]
	public class ActorsController : Controller
	{
		private readonly IActorService _service;

		public ActorsController(IActorService service)
		{
			_service = service;
		}
		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var data = await _service.GetAllAsync();
			return View(data);
		}
        //Get: Actors/Create 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create([Bind("ProfilePicture,FullName,Bio")] Actor actor)
		{
			if (!ModelState.IsValid)
			{
				return View(actor);
			}

			await _service.AddAsync(actor);
			return RedirectToAction(nameof(Index));
		}
		//Get: Actors/Details/1
		[AllowAnonymous]
		public async Task<IActionResult> Details(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);

			if (actorDetails == null) return View("NotFound");
			return View(actorDetails);
		}

        //Get: Actor/Edit/1
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);

			if (actorDetails == null) return View("NotFound");

			return View(actorDetails);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, [Bind("ProfilePicture,FullName,Bio")] Actor actor)
		{
			if (!ModelState.IsValid)
			{
				return View(actor);
			}

			await _service.UpdateAsync(id, actor);
			return RedirectToAction(nameof(Index));
		}

		//Get: Actor/Delete/1
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null) return View("NotFound");

			return View(actorDetails);
		}
		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null) return View("NotFound");

			await _service.DeleteAsync(id);


			return RedirectToAction(nameof(Index));
		}

	}
}
