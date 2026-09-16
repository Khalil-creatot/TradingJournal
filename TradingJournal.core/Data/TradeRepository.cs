using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingJournal.core.Models;
using TradingJournal.core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TradingJournal.core.Data
{
    public class TradeRepository : ITradeRepository
    {
        private readonly AppDbContext _context;

        public TradeRepository(AppDbContext context) {
            _context = context;
        }

        public void Add(Trade trade)
        {
            _context.Trades.Add(trade);
            _context.SaveChanges();
        }

        public IEnumerable<Trade> GetAll()
        {
            return _context.Trades
                .OrderByDescending(t => t.CloseDate)
                .ToList();
        }

        public Trade? GetById(Guid id)
        {
            return _context.Trades.Find(id);
        }

        public void Update(Trade trade) 
        {
            _context.Trades.Update(trade);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var trade = GetById(id);
            if (trade != null)
            {
                _context.Trades.Remove(trade);
                _context.SaveChanges();
            }
        }
    }
}
