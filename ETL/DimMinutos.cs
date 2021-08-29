using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ETL
{
    class DimMinutos
    {
        private DataWarehouse dw;

        public DimMinutos(DataWarehouse dw)
        {
            this.dw = dw;
        }

        public int GetId(DateTime fecha)
        {
            var dia = this.dw.DataMart("dim_minutos")
                .Where("hora", fecha.ToString("HH:mm"))
                .FirstOrDefault();

            if (dia == null)
            {
                return this.InsertGetId(fecha);
            }

            return (int) dia.id;
        }

        public int InsertGetId(DateTime fecha)
        {
            return this.dw.DataMart("dim_minutos")
                .InsertGetId<int>(new
                {
                    hora = fecha.ToString("HH:mm"),
                    hora_numero = fecha.Hour,
                    minuto_numero = fecha.Minute,
                    am_pm = (fecha.Hour >= 12) ? "pm" : "am"
                });
        }
    }
}
