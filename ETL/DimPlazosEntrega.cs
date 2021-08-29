using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMartFasta.ETL
{
    class DimPlazosEntrega
    {
        private DataWarehouse dw;

        public DimPlazosEntrega(DataWarehouse dw)
        {
            this.dw = dw;
        }

        public int GetId(int idExt)
        {
            var tipoComprobante = this.dw.DataMart("dim_plazos_entrega")
                .Where("plazo_entrega_idext", idExt)
                .FirstOrDefault();

            if (tipoComprobante == null)
            {
                return this.InsertGetId(idExt);
            }

            return (int)tipoComprobante.id;
        }

        public int InsertGetId(int idExt)
        {
            var plazoEntrega = this.dw.Faverino("PlazosEntregas")
                .Where("plazo_entrega_id", idExt)
                .First();

            return this.dw.DataMart("dim_plazos_entrega")
                .InsertGetId<int>(new
                {
                    plazo_entrega_idext = idExt,
                    descripcion = plazoEntrega.descripcion,
                    horas = this.GetNumeroDeHoras(plazoEntrega.descripcion),
                });
        }

        private int GetNumeroDeHoras(string descripcion)
        {
            string horasStr = new String(descripcion.Where(Char.IsDigit).ToArray());

            if (!int.TryParse(horasStr, out int horas))
            {
                horas = 0;
            }

            return horas;
        }
    }
}
