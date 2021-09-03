using DataMartFasta.Definitions;
using DataMartFasta.ETL;
using FontAwesome.Sharp;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DataMartFasta.ViewModels
{
    public class OlapCubeViewModel : ObservableRecipient
    {
        public List<FactTableViewModel> FactTablesList { get;  } = new List<FactTableViewModel>();

        private FactTableViewModel selectedfactTable;
        public FactTableViewModel SelectedFactTable { get => this.selectedfactTable; set => this.SetProperty(ref this.selectedfactTable, value, true); }

        private List<dynamic> queryResults = new List<dynamic>();
        public List<dynamic> QueryResults { get => this.queryResults; set => this.SetProperty(ref this.queryResults, value); }

        public IRelayCommand PerformQueryCommand { get; }
        
        public IRelayCommand AddAttributeCommand { get; }

        public IRelayCommand RemoveAttributeCommand { get; }

        private QueryFactory dw;

        internal OlapCubeViewModel(QueryFactory dw)
        {
            this.dw = dw;

            // Icon names are font awesome 5 icon names in CammelCase (https://fontawesome.com/)

            var dimFecha = DimensionDefinition.Make("dim_dias", "Fecha", IconChar.Calendar)
                .WithAttribute("fecha")
                .WithAttribute("dia")
                .WithAttribute("mes")
                .WithAttribute("anio", "Año")
                .WithAttribute("dia_del_anio", "Día del año")
                .WithAttribute("semana_numero", "Semana número")
                .WithAttribute("cuatrimestre")
                .WithAttribute("trimestre")
                .WithAttribute("es_dia_de_semana");

            var dimArticulos = DimensionDefinition.Make("dim_articulos", "Artículos", IconChar.Dolly)
                .WithAttribute("categoria")
                .WithAttribute("marca")
                .WithAttribute("modelo")
                .WithAttribute("producto")
                .WithAttribute("volumen");

            var dimPlazosEntrega = DimensionDefinition.Make("dim_plazos_entrega", "Plazos de entrega", IconChar.HourglassHalf)
                .WithAttribute("descripcion")
                .WithAttribute("horas");

            var dimHora = DimensionDefinition.Make("dim_minutos", "Hora", IconChar.Clock)
                .WithAttribute("am_pm")
                .WithAttribute("hora")
                .WithAttribute("hora_numero")
                .WithAttribute("minuto_numero");

            var dimSucursales = DimensionDefinition.Make("dim_sucursales", "Sucursales", IconChar.MapMarkerAlt)
                .WithAttribute("descripcion")
                .WithAttribute("domicilio")
                .WithAttribute("es_deposito_central");

            var dimTiposComprobante = DimensionDefinition.Make("dim_tipos_comprobante", "Tipos Comprobante", IconChar.HandPaper)
                .WithAttribute("descripcion");

            var dimTransportes = DimensionDefinition.Make("dim_transportes", "Transportes", IconChar.Truck)
                .WithAttribute("nombre_transportista")
                .WithAttribute("tipo_movil")
                .WithAttribute("tipo_transportista")
                .WithAttribute("es_movil_menor");

            var factDespachos = this.AddFactTable("despachos")
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

            var factStock = this.AddFactTable("stock")
                .WithMeasure("cantidad")
                .WithDimension(dimFecha, "dia_id")
                .WithDimension(dimSucursales, "sucursal_id")
                .WithDimension(dimArticulos, "articulo_id");

            this.PerformQueryCommand = new RelayCommand(this.PerformQuery, () => this.SelectedFactTable != null);
        }

        public FactTableViewModel AddFactTable(string tableName, string displayName = null)
        {
            var fact = new FactTableViewModel(tableName, displayName);
            this.FactTablesList.Add(fact);
            return fact;
        }

        protected void PerformQuery()
        {
            var fact = this.SelectedFactTable;

            var query = this.dw.Query(fact.TableName);

            
            var visibleDimensions = fact.DimensionsList.Where(dim => dim.Attributes.Any(attr => attr.IsVisible));
            var visibleMeasures = fact.MeasuresList.Where(measure => measure.IsVisible);

            if (! visibleDimensions.Any() && !visibleMeasures.Any())
            {
                this.QueryResults = new List<dynamic>();
                return;
            }

            foreach (FactDimensionViewModel dimension in visibleDimensions)
            {
                var tableAlias = dimension.UniqueAliasName;
                var foreignKey = fact.TableName + "." + dimension.ForeignKeyColumnName;
                var primaryKey = tableAlias + "." + dimension.PrimaryKeyColumnName;

                query.Join(dimension.TableName + " as " + tableAlias, foreignKey, primaryKey);
                
                foreach (var attribute in dimension.Attributes)
                {
                    if (attribute.IsVisible)
                    {
                        query.GroupBy(tableAlias + "." + attribute.ColumnName);
                        query.Select(tableAlias + "." + attribute.ColumnName + " as " + attribute.UniqueAliasName);
                    }
                }
            }

            foreach (var measure in visibleMeasures)
            {
                query.SelectRaw(measure.Aggregate + "(" + fact.TableName + "." + measure.ColumnName + ") as " + measure.UniqueAliasName);
            }

            this.QueryResults = query.Get().ToList();
        }
    }
}
