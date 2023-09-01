using Facturacion.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturacion.Presentacion
{
    public partial class FrmFacturacion : Form
    {
        Factura nuevo;
        public FrmFacturacion()
        {
            InitializeComponent();
            nuevo = new Factura();
        }

        private void FrmFacturacion_Load(object sender, EventArgs e)
        {
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtCliente.Text = ("CONSUMIDOR FINAL");
            txtCantidad.Text = "1";
            CargarArticulos();
            FormaPago();
            ProximaFactura();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("Estas seguro de querer cancelar?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes) { this.Close(); }
        }
        private void CargarArticulos()
        {
            SqlConnection conexion = new SqlConnection();
            conexion.ConnectionString = @"Data Source=NACHO\SQLEXPRESS;Initial Catalog=Facturacion;Integrated Security=True";
            conexion.Open();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_consultar_articulos";
            DataTable tabla = new DataTable();
            tabla.Load(comando.ExecuteReader());
            comando.ExecuteNonQuery();
            conexion.Close();

            cboArticulo.DataSource = tabla;
            cboArticulo.ValueMember = tabla.Columns[0].ColumnName;
            cboArticulo.DisplayMember = tabla.Columns[2].ColumnName;

        }
        private void FormaPago()
        {
            SqlConnection conexion = new SqlConnection();
            conexion.ConnectionString = @"Data Source=NACHO\SQLEXPRESS;Initial Catalog=Facturacion;Integrated Security=True";
            conexion.Open();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_formaPago";
            DataTable tabla = new DataTable();
            tabla.Load(comando.ExecuteReader());
            comando.ExecuteNonQuery();
            conexion.Close();

            cboFormaPago.DataSource = tabla;
            cboFormaPago.ValueMember = tabla.Columns[0].ColumnName;
            cboFormaPago.DisplayMember = tabla.Columns[1].ColumnName;
        }
        private void ProximaFactura()
        {
            SqlConnection conexion = new SqlConnection();
            conexion.ConnectionString = @"Data Source=NACHO\SQLEXPRESS;Initial Catalog=Facturacion;Integrated Security=True";
            conexion.Open();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FacturaNro";
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = @"next";
            parametro.SqlDbType = SqlDbType.Int;
            parametro.Direction = ParameterDirection.Output;
            comando.Parameters.Add(parametro);
            comando.ExecuteNonQuery();
            conexion.Close();

            lblFacturaNro.Text = lblFacturaNro.Text + "  " + parametro.Value;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            validaciones();
            DataRowView item = (DataRowView)cboArticulo.SelectedItem;
            int nro = Convert.ToInt32(item.Row.ItemArray[0]);
            string nom = item.Row.ItemArray[1].ToString();
            double pre = Convert.ToDouble(item.Row.ItemArray[3]);
            Articulo a = new Articulo(nro, nom, pre);
            int cant = Convert.ToInt32(txtCantidad.Text);

            DetalleFactura detalleExistente = nuevo.Detalles.FirstOrDefault(detalle => detalle.Articulo.ArticuloNro == nro);

            if (detalleExistente != null)
            {
                detalleExistente.Cantidad += cant;
                dgvDetalles.Rows[detalleExistente.IndiceDataGridView].Cells["ColCantidad"].Value = detalleExistente.Cantidad;
            }
            else
            {
                DetalleFactura detalle = new DetalleFactura(a, cant);
                detalle.IndiceDataGridView = dgvDetalles.Rows.Add(detalle.Articulo.ArticuloNro, detalle.Articulo.NombreArticulo, pre, detalle.Cantidad, "Quitar");
                nuevo.AgregarDetalle(detalle);
            }

            CalcularTotales(); 
        }


        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 4)
            {
                nuevo.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.RemoveAt(dgvDetalles.CurrentRow.Index);
                CalcularTotales();
            }
        }
        private void CalcularTotales()
        {
            double total = nuevo.CalcularTotal();
            txtTotal.Text = total.ToString();
        }
        public void validaciones()
        {
            if (cboArticulo.SelectedIndex == -1)
            { MessageBox.Show("Debe seleccionar un articulo", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            if (string.IsNullOrEmpty(txtCliente.Text))
            { MessageBox.Show("El campo Cliente es requerido", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            if (string.IsNullOrEmpty(txtCantidad.Text) || !int.TryParse(txtCantidad.Text, out _))
            { MessageBox.Show("El campo Cantidad es requerido", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
     

        }
    }
    
}
