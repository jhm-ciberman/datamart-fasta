using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ETL
{
    class DimSucursales
    {
        private DataWarehouse dw;
        private int sucursalNulaId;

        public DimSucursales(DataWarehouse dw)
        {
            this.dw = dw;
            this.sucursalNulaId = this.GetSucursalNula();
        }

        public int GetId(int? idExt)
        {
            if (! idExt.HasValue)
                return this.sucursalNulaId;

            var tipoComprobante = this.dw.DataMart("dim_sucursales")
                .Where("sucursal_idext", idExt)
                .FirstOrDefault();

            if (tipoComprobante == null)
            {
                return this.InsertGetId(idExt.Value);
            }

            return (int) tipoComprobante.id;
        }

        public int InsertGetId(int idExt)
        {
            var sucursal = this.dw.Faverino("sucursales")
                .Where("sucursal_id", idExt)
                .First();

            return this.dw.DataMart("dim_sucursales")
                .InsertGetId<int>(new
                {
                    sucursal_idext = sucursal.sucursal_id,
                    descripcion = sucursal.descripcion,
                    domicilio = sucursal.domicilio,
                    es_deposito_central = sucursal.esDepositoCentralSN
                });
        }

        private int GetSucursalNula()
        {
            var sucursalNula = this.dw.DataMart("dim_sucursales")
                .Where("descripcion", "Sin sucursal")
                .FirstOrDefault();

            if (sucursalNula != null)
                return sucursalNula.id;

            return this.dw.DataMart("dim_sucursales")
                .InsertGetId<int>(new
                {
                    sucursal_idext = (int?) null,
                    descripcion = "Sin sucursal",
                    domicilio = "",
                    es_deposito_central = false,
                });
        }
    }
}
