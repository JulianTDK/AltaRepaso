using System;
using AltaRepaso.Entidades;
using AltaRepaso.Servicios;
using AltaRepaso.Servicios.Interfaz;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AltaRepaso
{
    public partial class Form1 : Form
    {
        IServicio servicio = null;
        OrdenRetiro nuevaOrden = null;
        public Form1(FactoryServicioImp factory)
        {
            InitializeComponent();
            servicio = factory.GetServicio();
            nuevaOrden = new OrdenRetiro();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarMateriales();
        }
        private void CargarMateriales()
        {
            cboMateriales.DataSource = servicio.TraerMateriales();
            cboMateriales.ValueMember = "codigo";
            cboMateriales.DisplayMember = "nombre";
            cboMateriales.SelectedIndex = -1;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtResponsable.Text))
            {
                MessageBox.Show("Ingrese un Responsable...", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (nudCantidad.Value <= 0)
            {
                MessageBox.Show("Ingrese una Cantidad...", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            foreach (DataGridViewRow r in dgvDetalles.Rows)
            {

                if (r.Cells["Nombre"] != null && r.Cells["Nombre"].Value != null)
                {
                    if (r.Cells["Nombre"].Value.ToString() == cboMateriales.Text)
                    {
                        MessageBox.Show("Este Material ya está presupuestado...", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
            Material m = (Material)cboMateriales.SelectedItem;
            string responsable = txtResponsable.Text;
            int cantidad = Convert.ToInt32(nudCantidad.Value);

            DetalleOrden detalle = new DetalleOrden(m, cantidad);

            nuevaOrden.AddDetallle(detalle);

            dgvDetalles.Rows.Add(new object[] { m.Nombre, m.Stock, cantidad, "Quitar" });
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (nuevaOrden.lDetalles.Count <= 0)
            {
                MessageBox.Show("Debe agregar al menos un Material a la Orden...", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                foreach (DetalleOrden d in nuevaOrden.lDetalles)
                {
                    if (d.Material.Stock < d.Cantidad)
                    {
                        MessageBox.Show(d.Material.ToString() + " no tiene stock suficiente...", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
            GrabarOrden();
        }
        private void GrabarOrden()
        {
            nuevaOrden.Fecha = dtpFecha.Value;
            nuevaOrden.Responsable = txtResponsable.Text;
            int nroOrden = servicio.CrearOrdenRetiro(nuevaOrden);
            if (nroOrden != 0)
            {
                MessageBox.Show("Se registró con éxito la Orden de Retiro.\nNro: " + nroOrden.ToString(), "Informe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearControls();
                nuevaOrden = new OrdenRetiro();
            }
            else
            {
                MessageBox.Show("NO se pudo registrar la Orden de Retiro...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void ClearControls()
        {
            txtResponsable.Text = string.Empty;
            cboMateriales.SelectedIndex = -1;
            nudCantidad.Value = 0;
            dgvDetalles.Rows.Clear();
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 3)
            {
                nuevaOrden.RemoveDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.RemoveAt(dgvDetalles.CurrentRow.Index);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dgvDetalles_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
