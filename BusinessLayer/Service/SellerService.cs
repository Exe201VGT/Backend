using BOs.Models;
using BusinessLayer.Service.Interface;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class SellerService : ISellerService
    {
        private readonly ISellerRepository _sellerRepository;

        public SellerService(ISellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }

        public async Task<IEnumerable<Seller>> GetAllSellersAsync()
        {
            return await _sellerRepository.GetAllSellersAsync();
        }

        public async Task<Seller> GetSellerByIdAsync(int sellerId)
        {
            return await _sellerRepository.GetSellerByIdAsync(sellerId);
        }

        public async Task<Seller> GetSellerByUserIdAsync(int userId)
        {
            return await _sellerRepository.GetSellerByUserIdAsync(userId);
        }

        public async Task<Seller> RegisterSellerAsync(Seller seller)
        {
            return await _sellerRepository.RegisterSellerAsync(seller);
        }

        public async Task<bool> UpdateSellerAsync(Seller seller)
        {
            return await _sellerRepository.UpdateSellerAsync(seller);
        }

        public async Task<bool> DeleteSellerAsync(int sellerId)
        {
            return await _sellerRepository.DeleteSellerAsync(sellerId);
        }

    }
}
