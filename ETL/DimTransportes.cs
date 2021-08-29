using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ETL
{
    class DimTransportes
    {
        private Dictionary<int, dynamic> transportistasExternos = new Dictionary<int, dynamic>();
        private Dictionary<int, dynamic> tipoMovil = new Dictionary<int, dynamic>();
        private Dictionary<int, dynamic> tipoTransportista = new Dictionary<int, dynamic>();

        private DataWarehouse dw;

        public DimTransportes(DataWarehouse dw)
        {
            this.dw = dw;

            foreach (var transportista in this.dw.Faverino("TransportistasExternos").Get())
            {
                this.transportistasExternos.Add(transportista.transportista_externo_id, transportista);
            }

            foreach (var tipoMovil in this.dw.Faverino("TiposMoviles").Get())
            {
                this.tipoMovil.Add(tipoMovil.tipo_movil_id, tipoMovil);
            }

            foreach (var tipoTransportista in this.dw.Faverino("TiposTransportistas").Get())
            {
                this.tipoTransportista.Add(tipoTransportista.tipo_transportista_id, tipoTransportista);
            }
        }

        public int GetId(int? idTransportistaExterno, int idTipoMovil, int idTipoTransportista)
        {
            var transporte = this.dw.DataMart("dim_transportes")
                .Where("transportista_externo_idext", idTransportistaExterno)
                .Where("tipo_movil_idext", idTipoMovil)
                .Where("tipo_transportista_idext", idTipoTransportista)
                .FirstOrDefault();

            if (transporte == null)
            {
                return this.InsertGetId(idTransportistaExterno, idTipoMovil, idTipoTransportista);
            }

            return (int) transporte.id;
        }

        public int InsertGetId(int? idTransportistaExterno, int idTipoMovil, int idTipoTransportista)
        {
            dynamic transportistaExterno = (idTransportistaExterno != null)
                    ? this.transportistasExternos[idTransportistaExterno.Value]
                    : null;

            dynamic tipoMovil = this.tipoMovil[idTipoMovil];

            dynamic tipoTransportista = this.tipoTransportista[idTipoTransportista];

            return this.dw.DataMart("dim_transportes")
                .InsertGetId<int>(new
                {
                    transportista_externo_idext = idTransportistaExterno,
                    tipo_transportista_idext = idTipoTransportista,
                    tipo_movil_idext = idTipoMovil,

                    tipo_transportista = tipoTransportista.descripcion,
                    tipo_movil = tipoMovil.descripcion,
                    es_movil_menor = tipoMovil.esMovilMenorSN,
                    nombre_transportista = transportistaExterno == null ? "Propio" : transportistaExterno.nombre,
                });
        }
    }
}
