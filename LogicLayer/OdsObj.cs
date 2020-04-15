using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validate.LogicLayer
{
    // created with http://json2csharp.com/ from json file
    public class Coordinates
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Od
    {
        public string id { get; set; }
        public Coordinates coordinates { get; set; }
    }

    public class RootObject
    {
        public List<Od> ods { get; set; }
    }

    public class Inicio
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Fin
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Relacion
    {
        public int origen { get; set; }
        public int destino { get; set; }
        public string rel_principal { get; set; }
        public string rel_secundaria { get; set; }
        public Inicio inicio { get; set; }
        public Fin fin { get; set; }
    }

    public class RootObject_0
    {
        public List<Relacion> relaciones { get; set; }
    }

    public class WrapperObject
    {
        public RootObject ListOds { get; set; }
        public RootObject_0 ListRelations { get; set; }
    }
}