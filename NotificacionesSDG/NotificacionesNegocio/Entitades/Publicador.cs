using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificacionesNegocio.Entitades
{
    public class Publicador
    {
        public List<Lector> LectoresSuscritos { get; set; } = new List<Lector>();
    }
}
