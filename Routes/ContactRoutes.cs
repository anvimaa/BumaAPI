using BumaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BumaAPI.Routes;
public static class ContactRoutes
{
    static string modelName = "Contact";
    static string path = "/contacts";
    public static void MapContactRoutes(this WebApplication app)
    {
        app.MapGet(path, async (DataBaseContext db) => await db.ContactItems.ToListAsync())
        .WithName($"Get{modelName}Items")
        .WithTags(modelName)
        .WithDescription($"Retornar todos {modelName}")
        .Produces<List<ContactItem>>(StatusCodes.Status200OK);

        app.MapGet(path + "/{id:int}", async (DataBaseContext db, int id) =>
            await db.ContactItems.FindAsync(id) is ContactItem model ?
            Results.Ok(model) :
            Results.NotFound())
            .WithName($"Get{modelName}ById")
            .WithTags(modelName)
            .WithDescription($"Retornar um {modelName} pelo ID")
            .Produces<ContactItem>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(path, async (ContactItem model, DataBaseContext db) =>
        {
            if (model is null) return Results.BadRequest();
            db.ContactItems.Add(model);
            await db.SaveChangesAsync();
            return Results.Created($"{path}/{model.Id}", model);
        }).WithTags(modelName)
          .WithName($"Post{modelName}")
          .WithDescription($"Criar novo {modelName}")
          .Produces<ContactItem>(StatusCodes.Status201Created)
          .Produces(StatusCodes.Status400BadRequest);

        app.MapDelete(path + "/{id:int}", async (int id, DataBaseContext db) =>
        {
            var model = await db.ContactItems.FindAsync(id);
            if (model is null) return Results.NotFound();
            db.ContactItems.Remove(model);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithTags(modelName)
        .WithName($"Delete{modelName}")
        .WithDescription($"Deletar {modelName}")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPut(path + "/{id:int}", async (int id, ContactItem model, DataBaseContext db) =>
        {
            var modelEdit = await db.ContactItems.FindAsync(id);

            if (modelEdit is null) return Results.NotFound();

            modelEdit.Title = model.Title;
            modelEdit.Icon = model.Icon;
            modelEdit.Description= model.Description;

            await db.SaveChangesAsync();
            return Results.Ok(modelEdit);
        })
        .WithTags(modelName)
        .WithName($"Put{modelName}")
        .WithDescription($"Atualizar {modelName}")
        .Produces<ContactItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
