namespace EMS.API;

public static class ConfigureMiddleware
{
    public static WebApplication? ConfigureFrameworkMiddlewares(this WebApplication? app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseCors(x => x
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
