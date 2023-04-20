using BumaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BumaAPI.Routes;
public static class SocialRoutes
{
    static string modelName = "Social";
    static string path = "/socials";
    public static void MapSocialRoutes(this WebApplication app)
    {
        app.MapGet(path, async (DataBaseContext db) => await db.SocialItems.ToListAsync())
        .WithName($"Get{modelName}Items")
        .WithTags(modelName)
        .WithDescription($"Retornar todos {modelName}")
        .Produces<List<SocialItem>>(StatusCodes.Status200OK);

        app.MapGet(path + "/{id:int}", async (DataBaseContext db, int id) =>
            await db.SocialItems.FindAsync(id) is SocialItem model ?
            Results.Ok(model) :
            Results.NotFound())
            .WithName($"Get{modelName}ById")
            .WithTags(modelName)
            .WithDescription($"Retornar um {modelName} pelo ID")
            .Produces<SocialItem>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(path, async (SocialItem model, DataBaseContext db) =>
        {
            if (model is null) return Results.BadRequest();
            db.SocialItems.Add(model);
            await db.SaveChangesAsync();
            return Results.Created($"{path}/{model.Id}", model);
        }).WithTags(modelName)
          .WithName($"Post{modelName}")
          .WithDescription($"Criar novo {modelName}")
          .Produces<SocialItem>(StatusCodes.Status201Created)
          .Produces(StatusCodes.Status400BadRequest);

        app.MapDelete(path + "/{id:int}", async (int id, DataBaseContext db) =>
        {
            var model = await db.SocialItems.FindAsync(id);
            if (model is null) return Results.NotFound();
            db.SocialItems.Remove(model);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithTags(modelName)
        .WithName($"Delete{modelName}")
        .WithDescription($"Deletar {modelName}")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPut(path + "/{id:int}", async (int id, SocialItem model, DataBaseContext db) =>
        {
            var modelEdit = await db.SocialItems.FindAsync(id);

            if (modelEdit is null) return Results.NotFound();

            modelEdit.Url = model.Url;
            modelEdit.Icon = model.Icon;

            await db.SaveChangesAsync();
            return Results.Ok(modelEdit);
        })
        .WithTags(modelName)
        .WithName($"Put{modelName}")
        .WithDescription($"Atualizar {modelName}")
        .Produces<SocialItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
