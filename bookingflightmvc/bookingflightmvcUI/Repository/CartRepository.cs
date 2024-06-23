﻿using bookingflightmvcUI.Data;
using bookingflightmvcUI.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bookingflightmvcUI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> AddItem(int bookId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };
                    _db.Carts.Add(cart);
                }
                _db.SaveChanges();
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.CartId == cart.Id && a.FlightId == bookId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var book = _db.Flights.Find(bookId);
                    cartItem = new CartDetail
                    {
                        FlightId = bookId,
                        CartId = cart.Id,
                        Quantity = qty,
                        UnitPrice = book.ticketPrice  // it is a new line after update
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }


        public async Task<int> RemoveItem(int bookId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.CartId == cart.Id && a.FlightId == bookId);
                if (cartItem is null)
                    throw new InvalidOperationException("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<Cart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new InvalidOperationException("Invalid userid");
            var shoppingCart = await _db.Carts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Flight)
                                  .ThenInclude(a => a.Stock)
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Flight)
                                  .ThenInclude(a => a.Airport)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;

        }
        public async Task<Cart> GetCart(string userId)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId)) // updated line
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.Carts
                              join cartDetail in _db.CartDetails
                              on cart.Id equals cartDetail.CartId
                              where cart.UserId==userId // updated line
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }

        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
                var cartDetail = _db.CartDetails
                                    .Where(a => a.CartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new InvalidOperationException("Cart is empty");
                var pendingRecord = _db.BookingStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                if (pendingRecord is null)
                    throw new InvalidOperationException("Order status does not have Pending status");
                var order = new Booking
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    Name=model.Name,
                    Email=model.Email,
                    MobileNumber=model.MobileNumber,
                    PaymentMethod=model.PaymentMethod,
                    IsPaid=false,
                    BookingStatusId = pendingRecord.Id
                };
                _db.Bookings.Add(order);
                _db.SaveChanges();
                foreach(var item in cartDetail)
                {
                    var orderDetail = new BookingDetail
                    {
                        FlightId = item.FlightId,
                        BookingId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.BookingDetails.Add(orderDetail);

                    // update stock here

                    var stock = await _db.Stocks.FirstOrDefaultAsync(a => a.FlightId == item.FlightId);
                    if (stock == null)
                    {
                        throw new InvalidOperationException("Stock is null");
                    }

                    if (item.Quantity > stock.Quantity)
                    {
                        throw new InvalidOperationException($"Only {stock.Quantity} items(s) are available in the stock");
                    }
                    // decrease the number of quantity from the stock table
                    stock.Quantity -= item.Quantity;
                }
                //_db.SaveChanges();

                // removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }


    }
}
