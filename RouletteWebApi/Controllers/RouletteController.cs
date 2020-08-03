using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using RouletteWebApi.Models;
using RouletteWebApi.Services;
namespace RouletteWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly RouletteService _rouleteService;
        public RouletteController(RouletteService rouleteService)
        {
            _rouleteService = rouleteService;
        }
        [HttpGet]
        public IEnumerable<Roulette> Get()
        {
            return _rouleteService.Get();
        }
        [HttpGet("create/")]
        public IActionResult Create()
        {
            Roulette _roulette = new Roulette()
            {
                Open = "false"
            };
            try
            {
                var result = _rouleteService.Create(_roulette);
                return Created($"{this.Request.Scheme}://{this.Request.Host}{this.Request.Path}/{result.Id}", result.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpPost("bet/{userId}")]
        public IActionResult Bet(string userId, [FromBody] Dictionary<string, object> bet)
        {
            Bet newBet = new Bet()
            {
                RouletteId = bet["rouletteId"].ToString(),
                UserId = Int32.Parse(userId),
                BetAmmount = Int32.Parse(bet["ammount"].ToString()),
                BetBox = bet["box"].ToString(),
                DateMade = bet["dateMade"].ToString()

            };
            if (IsValidBet(newBet))
            {
                var result = _rouleteService.CreateBet(newBet);
                return Created($"{this.Request.Scheme}://{this.Request.Host}{this.Request.Path}/{userId}", result.Id);
            }
            else
            {
                return StatusCode(400);
            }
        }
        [HttpGet("open/{id}")]
        public IActionResult Open(string id)
        {
            Roulette _roulette = new Roulette()
            {
                Id = id,
                Open = "true",
                OpenDate = DateTime.Now.ToString()
            };
            try
            {
                _rouleteService.Update(id,_roulette);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpGet("close/{id}")]
        public IEnumerable<Bet> Close(string id)
        {
            Roulette _roulette = new Roulette()
            {
                Id = id,
                Open = "false",
                CloseDate = DateTime.Now.ToString()
            };
            
            return _rouleteService.CloseRoulette(_roulette);
        }
        private bool IsValidBet(Bet bet)
        {
            bool isValid = false;
            int box;
            if(bet.BetAmmount > 0 && bet.BetAmmount <= 10000 )
            {
                isValid = true;
            }
            if(Int32.TryParse(bet.BetBox, out box))
            {
                if(box >= 0 && box <= 36)
                {
                    isValid = true;
                }
            }
            else
            {
                if(bet.BetBox.ToLower().Equals("black") || bet.BetBox.ToLower().Equals("red"))
                {
                    isValid = true;
                }
            }

            return isValid;
        }
    }
}