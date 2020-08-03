using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RouletteWebApi.Models
{
    public interface IRouletteDatabaseSettings
    {
        string RoulettesCollectionName { get; set; }
        string BetsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}