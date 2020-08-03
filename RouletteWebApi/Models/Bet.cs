using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace RouletteWebApi.Models
{
    public class Bet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RouletteId { get; set; }
        public int BetAmmount { get; set; }
        public int UserId { get; set; }
        public string BetBox { get; set; }
        public string DateMade { get; set; }
    }
}