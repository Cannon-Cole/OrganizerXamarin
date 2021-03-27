using Organizer.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Data
{
    public class OrganizerDatabase
    {
        SQLiteAsyncConnection _database;

        public OrganizerDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath, SQLite.SQLiteOpenFlags.ReadWrite, false);
        }

        public Task<List<Event>> GetEventsAsync()
        {
            return _database.Table<Event>().ToListAsync();
        }

        public Task<Event> GetEventAsync(int id)
        {
            return _database.Table<Event>()
                            .Where(i => i.EventID == id)
                            .FirstOrDefaultAsync();
        }
        public async Task<List<Event>> GetDatesWithIncompleteEventsAsync()
        {
            return await _database.QueryAsync<Event>("SELECT DISTINCT StartDate FROM Event WHERE Complete = 0");
        }
        public async Task<List<Event>> GetSingleEventForASpecificDay(string date)
        {
            return await _database.QueryAsync<Event>("SELECT * FROM Event WHERE StartDate = " + date);
        }
        public async Task<List<Event>> GetEventsForASpecificDay(string date)
        {
            return await _database.QueryAsync<Event>("SELECT * FROM Event WHERE StartDate = " + date + "ORDER BY Name ASC");
        }
        public async Task<List<Event>> GetIncompleteEventsForASpecificDay(string date)
        {
            return await _database.QueryAsync<Event>("SELECT * FROM Event WHERE StartDate = " + date + " AND Complete = 0");
        }
        public Task<int> SaveEventAsync(Event Event)
        {
            if (Event.EventID != 0)
            {
                return _database.UpdateAsync(Event);
            }
            else
            {
                return _database.InsertAsync(Event);
            }
        }
        public Task<int> DeleteEventAsync(Event Event)
        {
            return _database.DeleteAsync(Event);
        }
        public async Task<List<Event>> GetLastInsertedEvent()
        {
            return await _database.QueryAsync<Event>("SELECT EventId FROM Event ORDER BY EventId DESC LIMIT 1");
        }

        //Chunk==================================================
        public Task<List<Chunk>> GetChunksAsync()
        {
            return _database.Table<Chunk>().ToListAsync();
        }

        //public async Task<List<Event>> GetEventsForASpecificDay(string date)
        //{
        //    return await _database.QueryAsync<Event>("SELECT * FROM Event WHERE StartDate = " + date + "ORDER BY Name ASC");
        //}

        public Task<Chunk> GetChunkAsync(int id)
        {
            return _database.Table<Chunk>()
                            .Where(i => i.ChunkID == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<List<Chunk>> GetChunksByEvent(Event Event)
        {
            return await _database.QueryAsync<Chunk>("SELECT c.ChunkId FROM Event e " +
                "JOIN ChunkEvent ce ON ce.EventID = e.EventID " +
                "JOIN Chunk c ON ce.ChunkID = c.ChunkID WHERE e.EventID = " + Event.EventID + " ORDER BY c.Name ASC");
        }
        public Task<int> SaveChunkAsync(Chunk Chunk)
        {
            if (Chunk.ChunkID != 0)
            {
                return _database.UpdateAsync(Chunk);
            }
            else
            {
                return _database.InsertAsync(Chunk);
            }
        }
        public Task<int> DeleteChunkAsync(Chunk Chunk)
        {
            return _database.DeleteAsync(Chunk);
        }

        public Task<int> SaveChunkEventAsync(ChunkEvent ChunkEvent)
        {
                return _database.InsertAsync(ChunkEvent);
        }
    }
}