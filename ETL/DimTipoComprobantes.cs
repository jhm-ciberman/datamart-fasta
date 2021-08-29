using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ETL
{
    class DimTipoComprobantes
    {
        private DataWarehouse dw;

        public DimTipoComprobantes(DataWarehouse dw)
        {
            this.dw = dw;
        }

        public int GetId(int idExt)
        {
            var tipoComprobante = this.dw.DataMart("dim_tipos_comprobante")
                .Where("tipo_comprobante_idext", idExt)
                .FirstOrDefault();

            if (tipoComprobante == null)
            {
                return this.InsertGetId(idExt);
            }

            return (int) tipoComprobante.id;
        }

        public int InsertGetId(int idExt)
        {
            var comprobante = this.dw.Faverino("TiposComprobantes")
                .Where("tipo_comprobante_id", idExt)
                .First();

            return this.dw.DataMart("dim_tipos_comprobante")
                .InsertGetId<int>(new
                {
                    tipo_comprobante_idext = comprobante.tipo_comprobante_id,
                    descripcion = comprobante.descripcion,
                });
        }
    }
}
