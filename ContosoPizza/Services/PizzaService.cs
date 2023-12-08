using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService
{
    private readonly PizzaContext pizzaContext;

    public PizzaService(PizzaContext pizzaContext)
    {
        this.pizzaContext = pizzaContext;
    }

    public IEnumerable<Pizza> GetAll()
    {
        return pizzaContext.Pizzas
            .AsNoTracking()
            .ToList();
    }

    public Pizza? GetById(int id)
    {
        return pizzaContext.Pizzas
            .Include(p => p.Toppings)
            .Include(p => p.Sauce)
            .AsNoTracking()
            .SingleOrDefault(pizza => pizza.Id == id);
    }

    public Pizza Create(Pizza newPizza)
    {
        pizzaContext.Add(newPizza);
        pizzaContext.SaveChanges();
        return newPizza;
    }

    public void AddTopping(int pizzaId, int toppingId)
    {
        // var pizza = GetById(pizzaId);
        // if (pizza == null)
        // {
        //     return;
        // }
        // var topping = pizzaContext.Toppings
        // .AsNoTracking() // mistake - we NEED tracking here!
        // .SingleOrDefault(t => t.Id == toppingId);
        // if (topping == null)
        // {
        //     return;
        // }

        var pizza = pizzaContext.Pizzas.Find(pizzaId);
        var topping = pizzaContext.Toppings.Find(toppingId);

        if (pizza is null || topping is null)
        {
            throw new InvalidOperationException("Pizza or topping does not exist");
        }

        pizza.Toppings ??= new List<Topping>();
        pizza.Toppings.Add(topping);

        pizzaContext.SaveChanges();
    }

    public void UpdateSauce(int pizzaId, int sauceId)
    {
        var pizza = pizzaContext.Pizzas.Find(pizzaId);
        var sauce = pizzaContext.Sauces.Find(sauceId);

        if (pizza is null || sauce is null)
        {
            throw new InvalidOperationException("Pizza or sauce does not exist.");
        }

        pizza.Sauce = sauce;
        pizzaContext.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var pizza = pizzaContext.Pizzas.Find(id) ?? throw new InvalidOperationException("Pizza does not exist.");
        pizzaContext.Pizzas.Remove(pizza);
        pizzaContext.SaveChanges();
    }
}
