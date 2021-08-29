using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DataMartFasta.ETL
{
    class FactDespachosETL
    {
        private DataWarehouse dw;

        public int Total { get; private set; } = 0;

        public int Current { get; private set; } = 0;

        public event Action OnProgressChange;

        public FactDespachosETL(DataWarehouse dw)
        {
            this.dw = dw;
        }

        public Task Run()
        {
            int ultimoDespachoId = this.GetLatest()?.despacho_idext ?? 0;

            this.Total = this.GetTotal(ultimoDespachoId);
            this.Current = 0;
            this.OnProgressChange?.Invoke();

            return this.dw.Faverino("DespachosDetalle")
                .Select(
                    "Despachos.*",
                    "DespachosDetalle.cantidad as cantidad",
                    "DespachosDetalle.costoDespacho as costo",
                    "DespachosDetalle.articulo_id as articulo_id"
                )
                .Join("Despachos", "DespachosDetalle.despacho_id", "Despachos.despacho_id")
                .Where("DespachosDetalle.despacho_id", ">", ultimoDespachoId)
                .ChunkAsync(100, this.AddDespachos);
        }

        private dynamic GetLatest()
        {
            return this.dw.DataMart("despachos")
                .OrderByDesc("id")
                .FirstOrDefault();
        }

        private int GetTotal(int ultimoDespachoId)
        {
            return this.dw.Faverino("DespachosDetalle")
                .Where("DespachosDetalle.despacho_id", ">", ultimoDespachoId)
                .Count<int>();
        }

        private void AddDespachos(IEnumerable<dynamic> despachos, int page)
        {
            foreach (var despacho in despachos)
            {
                this.InsertOne(despacho);
                this.Current++;
            }

            this.OnProgressChange?.Invoke();
        }

        private void InsertOne(dynamic despacho)
        {
            this.dw.DataMart("stock").Insert(new
            {
                despacho_idext = (int)despacho.despacho_id,
                cantidad = despacho.cantidad,
                costo = despacho.costo,
                tipo_comprobante_id = this.dw.DimTipoComprobantes.GetId((int)despacho.tipo_comprobante_id),
                sucursal_origen_id = this.dw.DimSucursales.GetId((int?)despacho.LugarOrigen_id),
                sucursal_destino_id = this.dw.DimSucursales.GetId((int?)despacho.LugarDestino_id),
                articulo_id = this.dw.DimArticulos.GetId((long)despacho.articulo_id),
                dia_id = this.dw.DimDias.GetId((DateTime)despacho.FechaHora),
                minuto_id = this.dw.DimMinutos.GetId((DateTime)despacho.FechaHora),
                plazo_entrega_id = this.dw.DimPlazosEntrega.GetId((int)despacho.plazo_entrega_id),
                transporte_id = this.dw.DimTransporte.GetId(
                    (int?)despacho.transportista_externo_id,
                    (int)despacho.tipo_movil_id,
                    (int)despacho.tipo_transportista_id
                ),
            });
        }
    }
}
