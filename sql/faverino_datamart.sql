CREATE TABLE [dim_sucursales] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [sucursal_idext] int,
  [descripcion] nvarchar(255) NOT NULL,
  [domicilio] nvarchar(255) NOT NULL,
  [es_deposito_central] bit NOT NULL
)
GO

CREATE TABLE [dim_articulos] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [articulo_idext] bigint NOT NULL,
  [marca_idext] int NOT NULL,
  [categoria_idext] int NOT NULL,
  [producto_idext] int NOT NULL,
  [modelo] nvarchar(255) NOT NULL,
  [volumen] float NOT NULL,
  [marca] nvarchar(255) NOT NULL,
  [categoria] nvarchar(255) NOT NULL,
  [producto] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [dim_dias] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [fecha] date NOT NULL,
  [dia] int NOT NULL,
  [mes] int NOT NULL,
  [anio] int NOT NULL,
  [dia_del_anio] int NOT NULL,
  [mes_nombre] nvarchar(255) NOT NULL,
  [dia_de_la_semana] nvarchar(255) NOT NULL,
  [semana_numero] int NOT NULL,
  [cuatrimestre] nvarchar(255) NOT NULL,
  [trimestre] nvarchar(255) NOT NULL,
  [bimestre] nvarchar(255) NOT NULL,
  [es_dia_de_semana] bit
)
GO

CREATE TABLE [dim_minutos] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [hora] nvarchar(255) NOT NULL,
  [hora_numero] int NOT NULL,
  [minuto_numero] int NOT NULL,
  [am_pm] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [dim_transportes] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [tipo_transportista_idext] int NOT NULL,
  [tipo_movil_idext] int NOT NULL,
  [transportista_externo_idext] int,
  [tipo_transportista] nvarchar(255) NOT NULL,
  [tipo_movil] nvarchar(255) NOT NULL,
  [es_movil_menor] bit NOT NULL,
  [nombre_transportista] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [dim_plazos_entrega] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [plazo_entrega_idext] int NOT NULL,
  [horas] int NOT NULL,
  [descripcion] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [dim_tipos_comprobante] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [tipo_comprobante_idext] int NOT NULL,
  [descripcion] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [despachos] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [despacho_idext] int NOT NULL,
  [tipo_comprobante_id] int NOT NULL,
  [sucursal_origen_id] int,
  [sucursal_destino_id] int,
  [articulo_id] int NOT NULL,
  [dia_id] int NOT NULL,
  [minuto_id] int NOT NULL,
  [plazo_entrega_id] int NOT NULL,
  [transporte_id] int NOT NULL,
  [cantidad] int NOT NULL,
  [costo] decimal(9,2)
)
GO

CREATE TABLE [stock] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [sucursal_id] int NOT NULL,
  [articulo_id] int NOT NULL,
  [dia_id] int NOT NULL,
  [cantidad] int NOT NULL
)
GO

CREATE TABLE [articulos_sin_movimientos] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [articulo_id] int NOT NULL,
  [dia_id] int NOT NULL
)
GO

ALTER TABLE [despachos] ADD FOREIGN KEY ([tipo_comprobante_id]) REFERENCES [dim_tipos_comprobante] ([id])
GO

ALTER TABLE [despachos] ADD FOREIGN KEY ([sucursal_origen_id]) REFERENCES [dim_sucursales] ([id])
GO

ALTER TABLE [despachos] ADD FOREIGN KEY ([sucursal_destino_id]) REFERENCES [dim_sucursales] ([id])
GO

ALTER TABLE [despachos] ADD FOREIGN KEY ([articulo_id]) REFERENCES [dim_articulos] ([id])
GO

ALTER TABLE [despachos] ADD FOREIGN KEY ([dia_id]) REFERENCES [dim_dias] ([id])
GO

ALTER TABLE [despachos] ADD FOREIGN KEY ([minuto_id]) REFERENCES [dim_minutos] ([id])
GO

ALTER TABLE [despachos] ADD FOREIGN KEY ([plazo_entrega_id]) REFERENCES [dim_plazos_entrega] ([id])
GO

ALTER TABLE [despachos] ADD FOREIGN KEY ([transporte_id]) REFERENCES [dim_transportes] ([id])
GO

ALTER TABLE [stock] ADD FOREIGN KEY ([sucursal_id]) REFERENCES [dim_sucursales] ([id])
GO

ALTER TABLE [stock] ADD FOREIGN KEY ([articulo_id]) REFERENCES [dim_articulos] ([id])
GO

ALTER TABLE [stock] ADD FOREIGN KEY ([dia_id]) REFERENCES [dim_dias] ([id])
GO

ALTER TABLE [articulos_sin_movimientos] ADD FOREIGN KEY ([articulo_id]) REFERENCES [dim_articulos] ([id])
GO

ALTER TABLE [articulos_sin_movimientos] ADD FOREIGN KEY ([dia_id]) REFERENCES [dim_dias] ([id])
GO
