using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.Entidades
{
    internal class Factura
    {
        public int nroFactura { get; set; }
        public DateTime fecha { get; set; }
        public string cliente { get; set; }
        public int formaPago { get; set; }
        public List<DetalleFactura> Detalles { get; set; }
        public Factura()
        {
            Detalles = new List<DetalleFactura>();
        }
        public void AgregarDetalle(DetalleFactura detalle)
        {
            Detalles.Add(detalle);
        }
        public void QuitarDetalle(int posicion)
        {
            Detalles.RemoveAt(posicion);
        }
        public double CalcularTotal()
        {
            double total = 0;


            foreach (DetalleFactura d in Detalles)
            {
                total += d.CalcularSubtotal();
            }
            return total;
        }

    }
}
