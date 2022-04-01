using PeliculasAPI.Entidades;
namespace PeliculasAPI.Repositorios
{
    public class RepositorioEnMemoria : IRepositorio
    {
        private List<Genero> _generos;

        public RepositorioEnMemoria()
        {
            _generos = new List<Genero>()
            {
                new Genero() { Id = 1, Nombre = "Accion" },
                new Genero() { Id = 2, Nombre = "Terror"}
            };

            _guid = Guid.NewGuid();//Guid: string con estruct --> 123546-FSAGR2AR-SDFERG5E5R-EAE5
                                   //Es aleatorio
                                   //Si se utiliza AddSingleton siempre va a ser el mismo string

        }

        public Guid _guid;

        public List<Genero> ObtenerTodosLosGeneros()
        {
            return _generos;
        }

        public async Task<Genero> ObtenerPorId(int Id)//Task es como una Promesa
        {
            await Task.Delay(1);//1 milisegundo
            //suspende la ejecución, libera el hilo actual para que pueda trabajar en otras cosas
            return _generos.FirstOrDefault(x => x.Id == Id);//Va a obtener un género o un nulo
        }

        public Guid ObtenerGUID()
        {
            return _guid;
        }

        public void CrearGenero(Genero genero)
        {
            genero.Id = _generos.Count() + 1;
            _generos.Add(genero);
        }

    }
}
