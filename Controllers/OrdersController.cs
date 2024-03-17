﻿using eTicket.Data.Cart; using eTicket.Data.Services; using eTicket.Data.ViewModels; using eTicket.Models; using Microsoft.AspNetCore.Mvc;  namespace eTicket.Controllers {
	public class OrdersController : Controller
	{
		private readonly IMoviesService _moviesService;
		private readonly ShoppingCart _shoppingCart;
		private readonly IOrderService _orderService;
		public OrdersController(IMoviesService moviesService, ShoppingCart shoppingCart, IOrderService orderService)
		{
			_moviesService = moviesService;
			_shoppingCart = shoppingCart;
			_orderService = orderService;
		}

		public async Task<IActionResult> Index()
		{
			string userId = "";

			var orders = await _orderService.GetOrdersByUserIdAsync(userId);
			return View(orders);
		}


		public IActionResult ShoppingCart()
		{
			var items = _shoppingCart.GetShoppingCartItems();
			_shoppingCart.ShoppingCartItems = items;
			var response = new ShoppingCartVM()
			{
				ShoppingCart = _shoppingCart,
				ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(),
			};

			return View(response);
		}

		public async Task<IActionResult> AddItemToShoppingCart(int id)
		{
			var item = await _moviesService.GetMovieByIdAsync(id);
			if (item != null)
			{
				_shoppingCart.AddItemToCart(item);
			}
			return RedirectToAction(nameof(ShoppingCart));
		}

		public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
		{
			var item = await _moviesService.GetMovieByIdAsync(id);
			if (item != null)
			{
				_shoppingCart.RemoveItemFromCart(item);
			}
			return RedirectToAction(nameof(ShoppingCart));
		}
		[ActionName("OrderCompleted")]
		public async Task<IActionResult> CompleteOrder()
		{
			var items = _shoppingCart.GetShoppingCartItems();
			string userId = "";
			string userEmailAddress = "";

			await _orderService.StoreOrderAsync(items, userId, userEmailAddress);
			await _shoppingCart.ClearShoppingCartAsync();
			return View("OrderComplete");
		}
	} } 