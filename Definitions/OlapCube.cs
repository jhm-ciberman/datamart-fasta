using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMartFasta.Definitions
{
    class OlapCube
    {
        private List<FactTable> factTables = new List<FactTable>();
        public IReadOnlyList<FactTable> FactTables => this.factTables;

        public OlapCube()
        {
            // Icon names are font awesome 5 icon names in CammelCase (https://fontawesome.com/)

            var dimFecha = DimensionTable.Make("dim_dias", "Fecha", IconChar.Calendar)
                .WithAttribute("fecha")
                .WithAttribute("dia")
                .WithAttribute("mes")
                .WithAttribute("anio", "Año")
                .WithAttribute("dia_del_anio", "Día del año")
                .WithAttribute("semana_numero", "Semana número")
                .WithAttribute("cuatrimestre")
                .WithAttribute("trimestre")
                .WithAttribute("es_dia_de_semana");

            var dimArticulos = DimensionTable.Make("dim_articulos", "Artículos", IconChar.Dolly)
                .WithAttribute("categoria")
                .WithAttribute("marca")
                .WithAttribute("modelo")
                .WithAttribute("producto")
                .WithAttribute("volumen");

            var dimPlazosEntrega = DimensionTable.Make("dim_plazos_entrega", "Plazos de entrega", IconChar.HourglassHalf)
                .WithAttribute("descripcion")
                .WithAttribute("horas");

            var dimHora = DimensionTable.Make("dim_minutos", "Hora", IconChar.Clock)
                .WithAttribute("am_pm")
                .WithAttribute("hora")
                .WithAttribute("hora_numero")
                .WithAttribute("minuto_numero");

            var dimSucursales = DimensionTable.Make("dim_sucursales", "Sucursales", IconChar.MapMarkerAlt)
                .WithAttribute("descripcion")
                .WithAttribute("domicilio")
                .WithAttribute("es_deposito_central");

            var dimTiposComprobante = DimensionTable.Make("dim_tipos_comprobante", "Tipos Comprobante", IconChar.HandPaper)
                .WithAttribute("descripcion");

            var dimTransportes = DimensionTable.Make("dim_transportes", "Transportes", IconChar.Truck)
                .WithAttribute("nombre_transportista")
                .WithAttribute("tipo_movil")
                .WithAttribute("tipo_transportista")
                .WithAttribute("es_movil_menor");

            var factDespachos = FactTable.Make("despachos")
                .WithMeasure("cantidad")
                .WithMeasure("costo")
                .WithDimension(dimSucursales, "sucursal_origen_id", "Sucursal de Origen", true)
                .WithDimension(dimSucursales, "sucursal_destino_id", "Sucursal de Destino", true)
                .WithDimension(dimPlazosEntrega, "plazo_entrega_id")
                .WithDimension(dimHora, "minuto_id")
                .WithDimension(dimTransportes, "transporte_id")
                .WithDimension(dimFecha, "dia_id")
                .WithDimension(dimTiposComprobante, "tipo_comprobante_id")
                .WithDimension(dimArticulos, "articulo_id");

            var factStock = FactTable.Make("stock")
                .WithMeasure("cantidad")
                .WithDimension(dimFecha, "dia_id")
                .WithDimension(dimSucursales, "sucursal_id")
                .WithDimension(dimArticulos, "articulo_id");

            this.factTables.Add(factDespachos);
            this.factTables.Add(factStock);
        }
    }
}
