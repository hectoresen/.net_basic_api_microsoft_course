using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_contoso_pizza.Controllers;

[ApiController]
[Route("[Controller]")]
public class PizzaController : ControllerBase
{
  private readonly ILogger<PizzaController> _logger;

  public PizzaController(ILogger<PizzaController> logger)
  {
    _logger = logger;
  }

  [HttpGet(Name = "GetPizzas")]
  public ActionResult<IEnumerable<Pizza>> GetPizzas()
  {
    var pizzas = PizzaService.GetAll();
    _logger.LogInformation("Retrieved {Count} pizzas.", pizzas.Count);
    return Ok(pizzas);
  }

  [HttpGet("{id:int}")]
  public ActionResult<Pizza> Get(int id)
  {
    var pizza = PizzaService.Get(id);

    if (pizza == null)
      return NotFound();

    return pizza;
  }

  [HttpPost(Name = "CreatePizza")]
  public ActionResult<Pizza> CreatePizza([FromBody] Pizza newPizza)
  {
    if (newPizza == null)
    {
      return BadRequest("Pizza data is required.");
    }

    PizzaService.Add(newPizza);
    _logger.LogInformation("Pizza created with Id {Id} and Name {Name}.", newPizza.Id, newPizza.Name);
    return CreatedAtRoute("GetPizzas", new { id = newPizza.Id }, newPizza);
  }

  [HttpPut("{id:int}", Name = "UpdatePizza")]
  public IActionResult UpdatePizza(int id, [FromBody] Pizza updatedPizza)
  {
    if (updatedPizza == null)
    {
      return BadRequest("Pizza data is required.");
    }

    var existingPizza = PizzaService.Get(id);
    if (existingPizza == null)
    {
      return NotFound($"Pizza with Id {id} not found.");
    }

    existingPizza.Name = updatedPizza.Name;
    existingPizza.IsGlutenFree = updatedPizza.IsGlutenFree;

    PizzaService.Update(existingPizza);
    _logger.LogInformation("Pizza with Id {Id} updated.", id);

    return Ok(existingPizza);
  }
}
