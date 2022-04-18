namespace PeliculasAPI.Utilidades
{
    public class AlmacenadorArchivosLocales: IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AlmacenadorArchivosLocales(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task BorrarArchivo(string ruta, string contenedor)
        {

            if (string.IsNullOrEmpty(ruta))//Si es nulo
            {
                return Task.CompletedTask;
            }
            var nombreArchivo =Path.GetFileName(ruta);
            var directorioArchivo= Path.Combine(env.WebRootPath,contenedor, nombreArchivo);

            if (File.Exists(directorioArchivo))//Si existe
            {
                File.Delete(directorioArchivo);
            }

            return Task.CompletedTask;
        }

        public async Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta)
        {
            await BorrarArchivo(ruta, contenedor);//Borra el anterior
            return await GuardarArchivo(contenedor, archivo);//Guarda el 'archivo editado'
        }

        public async Task<string> GuardarArchivo(string contenedor, IFormFile archivo)
        {
            var extension= Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, contenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            using (var memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                await File.WriteAllBytesAsync(ruta, contenido); 
            }

            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            //ruta que guardamos en BD
            var rutaParaDB = Path.Combine(urlActual, contenedor, nombreArchivo).Replace("\\","/");
            return rutaParaDB;

        }
    }
}
