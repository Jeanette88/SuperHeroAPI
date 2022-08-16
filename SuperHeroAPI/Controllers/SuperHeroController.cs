using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SuperHeroAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuperHeroController : ControllerBase
{
    private static List<SuperHero> heroes = new List<SuperHero>
    {
        /*  -------> Utan Databas
         
        new SuperHero {
            Id = 1,
            Name = "Spider Man",
            FirstName = "Peter",
            LastName = "Parker",
            Place = "New York City"
        },
        new SuperHero {
            Id = 2,
            Name = "Ironman",
            FirstName = "Tony",
            LastName = "Stark",
            Place = "Long Island"
        }
        */

    };

    private readonly DataContext _context;

    public SuperHeroController(DataContext context)
    {
        _context = context;
    }



    [HttpGet]
    public async Task<ActionResult<List<SuperHero>>> Get()
    {
        // return Ok(heroes);  -----> Utan Databas

        return Ok(await _context.SuperHeroes.ToListAsync()); // Med Databas
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SuperHero>> Get(int id)
    {
        // var hero = heroes.Find(h => h.Id == id); ------> utan Databas

        var hero = await _context.SuperHeroes.FindAsync(id); // Med Databas
        if (hero == null)
            return BadRequest("Hero not Found!");
        return Ok(hero);
    }

    [HttpPost]
    public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
    {
        // heroes.Add(hero); ---> Utan Databas
        // return Ok(heroes);

        _context.SuperHeroes.Add(hero); // Med Databas
        return Ok(await _context.SuperHeroes.ToListAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<SuperHero>>> UppdateHero(SuperHero request)
    {
        // var hero = heroes.Find(h => h.Id == request.Id);  ----> Utan Databas

        var dbhero = await _context.SuperHeroes.FindAsync(request.Id);  // Med Databas
        if (dbhero == null)
            return BadRequest("Hero not Found!");

        dbhero.Name = request.Name;
        dbhero.FirstName = request.FirstName;
        dbhero.LastName = request.LastName;
        dbhero.Place = request.Place;

        // return Ok(heroes); -----> Utan Databas

        await _context.SaveChangesAsync(); // Med Databas

        return Ok(await _context.SuperHeroes.ToListAsync());  // Med Databas
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SuperHero>> Delete(int id)
    {
        // var hero = heroes.Find(h => h.Id == id); ------> utan Databas

        var dbhero = await _context.SuperHeroes.FindAsync(id); // Med Databas
        if (dbhero == null)
            return BadRequest("Hero not Found!");

        // heroes.Remove(hero); ----> Utan Databas

        _context.SuperHeroes.Remove(dbhero);  // Med Databas
        await _context.SaveChangesAsync();   // Med Databas
        return Ok(heroes);
    }

}
