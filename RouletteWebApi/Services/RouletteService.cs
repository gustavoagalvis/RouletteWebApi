using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RouletteWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RouletteWebApi.Services
{
    public class RouletteService
    {
        private readonly IMongoCollection<Roulette> _roulettes;
        private readonly IMongoCollection<Bet> _bets;
        public RouletteService(IRouletteDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _roulettes = database.GetCollection<Roulette>(settings.RoulettesCollectionName);
            _bets = database.GetCollection<Bet>(settings.BetsCollectionName);
        }
        public List<Roulette> Get() =>
            _roulettes.Find(roulette => true).ToList();
        public Roulette Get(string id) =>
            _roulettes.Find<Roulette>(roulette => roulette.Id == id).FirstOrDefault();
        public List<Bet> GetBet() =>
           _bets.Find(bet => true).ToList();
        public Roulette Create(Roulette roulette)
        {
            _roulettes.InsertOne(roulette);
            return roulette;
        }
        public void Update(string id, Roulette rouletteIn) =>
            _roulettes.ReplaceOne(roulette => roulette.Id == id, rouletteIn);
        public void Remove(Roulette rouletteIn) =>
            _roulettes.DeleteOne(roulette => roulette.Id == rouletteIn.Id);
        public void Remove(string id) =>
            _roulettes.DeleteOne(roulette => roulette.Id == id);
        public List<Bet> CloseRoulette(Roulette roulette)
        {
            Roulette db_roulette = Get(roulette.Id);
            db_roulette.CloseDate = roulette.CloseDate;
            db_roulette.Open = roulette.Open;
            Update(db_roulette.Id, db_roulette);
            return GetRouleteBets(db_roulette);
        }
        public Bet CreateBet(Bet bet)
        {
            _bets.InsertOne(bet);
            return bet;
        }
        private List<Bet> GetRouleteBets(Roulette roulette)
        {
            var open = DateTime.Parse(roulette.OpenDate);
            var close = DateTime.Parse(roulette.CloseDate);
            List<Bet> bets =  _bets.Find(bet => bet.RouletteId == roulette.Id).ToList();

            return bets.FindAll(bet => GetDate(bet.DateMade) <= close && GetDate(bet.DateMade) >= open);
        }
        private DateTime GetDate(string dateString)
        {
            return DateTime.Parse(dateString);
        }
    }
}