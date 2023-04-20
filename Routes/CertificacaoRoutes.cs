using BumaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BumaAPI.Routes;
public static class CertificacaoRoutes
{
    static string modelName = "Certificacao";
    static string path = "/certificacaos";
    public static void MapCertificacaoRoutes(this WebApplication app)
    {
        app.MapGet(path, async (DataBaseContext db) => await db.CertificacaoItems.ToListAsync())
        .WithName($"Get{modelName}Items")
        .WithTags(modelName)
        .WithDescription($"Retornar todos {modelName}")
        .Produces<List<CertificacaoItem>>(StatusCodes.Status200OK);

        app.MapGet(path + "/{id:int}", async (DataBaseContext db, int id) =>
            await db.CertificacaoItems.FindAsync(id) is CertificacaoItem model ?
            Results.Ok(model) :
            Results.NotFound())
            .WithName($"Get{modelName}ById")
            .WithTags(modelName)
            .WithDescription($"Retornar um {modelName} pelo ID")
            .Produces<CertificacaoItem>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(path, async (CertificacaoItem model, DataBaseContext db) =>
        {
            if (model is null) return Results.BadRequest();
            db.CertificacaoItems.Add(model);
            await db.SaveChangesAsync();
            return Results.Created($"{path}/{model.Id}", model);
        }).WithTags(modelName)
          .WithName($"Post{modelName}")
          .WithDescription($"Criar novo {modelName}")
          .Produces<CertificacaoItem>(StatusCodes.Status201Created)
          .Produces(StatusCodes.Status400BadRequest);

        app.MapDelete(path + "/{id:int}", async (int id, DataBaseContext db) =>
        {
            var model = await db.CertificacaoItems.FindAsync(id);
            if (model is null) return Results.NotFound();
            db.CertificacaoItems.Remove(model);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithTags(modelName)
        .WithName($"Delete{modelName}")
        .WithDescription($"Deletar {modelName}")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPut(path + "/{id:int}", async (int id, CertificacaoItem model, DataBaseContext db) =>
        {
            var modelEdit = await db.CertificacaoItems.FindAsync(id);

            if (modelEdit is null) return Results.NotFound();

            modelEdit.Title = model.Title;
            modelEdit.Image = model.Image;
            modelEdit.Description= model.Description;

            await db.SaveChangesAsync();
            return Results.Ok(modelEdit);
        })
        .WithTags(modelName)
        .WithName($"Put{modelName}")
        .WithDescription($"Atualizar {modelName}")
        .Produces<CertificacaoItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
