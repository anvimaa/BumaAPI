namespace BumaAPI.Routes;

public static class MapAlllRoutes
{
    public static void MapAllRoute(this WebApplication app)
    {
        app.MapAboutItemRoutes();
        app.MapPersonalInfoRoute();
        app.MapProgressBarRoutes();
        app.MapTimeLineItemRoutes();
        app.MapPortifolioRoutes();
        app.MapCertificacaoRoutes();
        app.MapContactRoutes();
        app.MapSocialRoutes();
    }
}
