using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.ETL
{
    class DimDias
    {
        private DataWarehouse dw;

        private Dictionary<DateTime, int> cache = new Dictionary<DateTime, int>();

        public DimDias(DataWarehouse dw)
        {
            this.dw = dw;
        }

        public int GetId(DateTime fecha)
        {
            if (this.cache.TryGetValue(fecha, out int id))
            {
                return id;
            }

            var dia = this.dw.DataMart("dim_dias")
                .Where("fecha", fecha.Date)
                .FirstOrDefault();

            if (dia == null)
            {
                id = this.InsertGetId(fecha);
                this.cache.Add(fecha, id);
                return id;
            }

            return (int) dia.id;
        }

        public int InsertGetId(DateTime fecha)
        {
            return this.dw.DataMart("dim_dias")
                .InsertGetId<int>(new
                {
                    fecha = fecha,
                    dia = fecha.Day,
                    mes = fecha.Month,
                    anio = fecha.Year,
                    dia_del_anio = fecha.DayOfYear,
                    dia_de_la_semana = fecha.DayOfWeek,
                    mes_nombre = fecha.ToString("MMMM"),
                    semana_numero = (int)(fecha.DayOfYear / 7),
                    cuatrimestre = (int)(fecha.Month / 4),
                    trimestre = (int)(fecha.Month / 3),
                    bimestre = (int)(fecha.Month / 2),
                    es_dia_de_semana = !(fecha.DayOfWeek == DayOfWeek.Sunday || fecha.DayOfWeek == DayOfWeek.Saturday),
                });;
        }
    }
}
