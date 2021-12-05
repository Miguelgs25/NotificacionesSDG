namespace NotificacionesNegocio.Entitades
{
    public class Publicador
    {
        public List<Lector> LectoresSuscritos { get; set; } = new List<Lector>();
    }
}
