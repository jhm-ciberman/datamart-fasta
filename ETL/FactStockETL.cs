using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DataMartFasta.ETL
{
    class FactStockETL
    {
        private DataWarehouse dw;

        public int Total { get; private set; } = 0;

        public int Current { get; private set; } = 0;

        public event Action OnProgressChange;

        public FactStockETL(DataWarehouse dw)
        {
            this.dw = dw;
        }

        public Task Run()
        {
            DateTime? ultimaFecha = this.GetLatest()?.fecha;

            this.Total = this.GetTotal(ultimaFecha);
            this.Current = 0;
            this.OnProgressChange?.Invoke();

            return this.dw.Faverino("InventarioXFecha")
                .When(ultimaFecha != null, q => q.Where("InventarioXFecha.Fecha", ">", ultimaFecha.Value.Date))
                .ChunkAsync(100, this.AddChunk);
        }

        private dynamic GetLatest()
        {
            return this.dw.DataMart("stock")
                .Join("dim_dias", "stock.dia_id", "dim_dias.id")
                .OrderByDesc("stock.id")
                .FirstOrDefault();
        }

        private int GetTotal(DateTime? ultimaFecha)
        {
            return this.dw.Faverino("InventarioXFecha")
                .When(ultimaFecha != null, q => q.Where("InventarioXFecha.Fecha", ">", ultimaFecha.Value.Date))
                .Count<int>();
        }

        private void AddChunk(IEnumerable<dynamic> inventariosPorDia, int page)
        {
            foreach (var inventarioPorDia in inventariosPorDia)
            {
                this.InsertOne(inventarioPorDia));
                this.Current++;
            }
            this.OnProgressChange?.Invoke();
        }

        private void InsertOne(dynamic inventarioPorDia)
        {
            this.dw.DataMart("stock").Insert(new
            {
                sucursal_id = this.dw.DimSucursales.GetId((int?)inventarioPorDia.sucursal_id),
                articulo_id = this.dw.DimArticulos.GetId((long)inventarioPorDia.articulo_id),
                dia_id = this.dw.DimDias.GetId((DateTime)inventarioPorDia.fecha),
                cantidad = inventarioPorDia.cantidad,
            });
        }
    }
}
