using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ETL
{
    class DataWarehouse
    {
        public DimDias DimDias { get; }
        public DimArticulos DimArticulos { get; }
        public DimMinutos DimMinutos { get; }
        public DimTipoComprobantes DimTipoComprobantes { get; }
        public DimSucursales DimSucursales { get; }
        public DimPlazosEntrega DimPlazosEntrega { get; }
        public DimTransportes DimTransporte { get; }

        private QueryFactory dataMart;
        private QueryFactory faverino;

        public DataWarehouse(QueryFactory faverino, QueryFactory dataMart)
        {
            this.faverino = faverino;
            this.dataMart = dataMart;

            this.DimDias = new DimDias(this);
            this.DimArticulos = new DimArticulos(this);
            this.DimMinutos = new DimMinutos(this);
            this.DimTipoComprobantes = new DimTipoComprobantes(this);
            this.DimSucursales = new DimSucursales(this);
            this.DimPlazosEntrega = new DimPlazosEntrega(this);
            this.DimTransporte = new DimTransportes(this);
        }

        public SqlKata.Query Faverino(string table)
        {
            return this.faverino.Query(table);
        }

        public SqlKata.Query DataMart(string table)
        {
            return this.dataMart.Query(table);
        }
    }
}
