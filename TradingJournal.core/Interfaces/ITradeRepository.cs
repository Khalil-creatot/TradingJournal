using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingJournal.core.Models;

namespace TradingJournal.core.Interfaces
{
    // This interface defines the contract for a trade repository, which is responsible for managing trade data.
    public interface ITradeRepository
    {
        // Adds a new trade to the repository.
        void Add(Trade trade);
        // Retrieves all trades from the repository.
        IEnumerable<Trade> GetAll();
        // Retrieves a trade by its unique identifier.
        Trade? GetById(Guid id);
        // Updates an existing trade in the repository.
        void Update(Trade trade);
        void Delete(Guid id);
    }
}
