using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.Entidades
{
    internal class Articulo
    {
        public int ArticuloNro { get; set; }
        public string NombreArticulo { get; set; }
        public double PrecioUnitario { get; set; }
        public Articulo()
        {
            ArticuloNro = 0;
            NombreArticulo = string.Empty;
            PrecioUnitario = 0;
        }

        public Articulo(int articuloNro, string nombreArticulo, double precioUnitario)
        {
            ArticuloNro = articuloNro;
            NombreArticulo = nombreArticulo;
            PrecioUnitario = precioUnitario;
        }
        public override string ToString()
        {
            return NombreArticulo;
        }
    }
}
