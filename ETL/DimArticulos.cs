using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMartFasta.ETL
{
    class DimArticulos
    {
        private DataWarehouse dw;

        private Dictionary<long, int> cache = new Dictionary<long, int>();

        public DimArticulos(DataWarehouse dw)
        {
            this.dw = dw;
        }

        public int GetId(long articuloIdExt)
        {
            if (this.cache.TryGetValue(articuloIdExt, out int id))
            {
                return id;
            }

            var articulo = this.dw.DataMart("dim_articulos")
                .Where("articulo_idext", articuloIdExt)
                .FirstOrDefault();

            if (articulo == null)
            {
                id = this.InsertGetId(articuloIdExt);
                this.cache.Add(articuloIdExt, id);
                return id;
            }

            return (int)articulo.id;
        }

        public int InsertGetId(long articuloIdExt)
        {
            var articulo = this.dw.Faverino("articulos")
                .Select(
                    "articulos.articulo_id as articulo_idext",
                    "articulos.marca_id as marca_idext",
                    "categorias.categoria_id as categoria_idext",
                    "articulos.producto_id as producto_idext",
                    "productos.descripcion as producto", 
                    "articulos.modelo as modelo",
                    "articulos.volumenOcupadoXUnidad as volumen",
                    "marcas.descripcion as marca",
                    "categorias.descripcion as categoria",
                    "productos.descripcion as producto"
                )
                .Where("articulo_id", articuloIdExt)
                .LeftJoin("marcas", "marcas.marca_id", "articulos.marca_id")
                .LeftJoin("productos", "productos.producto_id", "articulos.producto_id")
                .LeftJoin("categorias", "categorias.categoria_id", "productos.categoria_id")
                .First();

            return this.dw.DataMart("dim_articulos")
                .InsertGetId<int>(new
                {
                    articulo_idext = articulo.articulo_idext,
                    marca_idext = articulo.marca_idext,
                    categoria_idext = articulo.categoria_idext,
                    producto_idext = articulo.producto_idext,
                    modelo = articulo.modelo,
                    volumen = articulo.volumen,
                    marca = articulo.marca,
                    producto = articulo.producto,
                    categoria = articulo.categoria,
                }); ;
        }
    }
}
