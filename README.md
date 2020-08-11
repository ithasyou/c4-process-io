# Descripción

Análisis de los procesos Scala y Python para generar un csv con las tablas que lee y escribe cada uno de ellos.

# Ejecución

* El proceso está desarrollado con `.NET CORE`.
* Ejecución del comando `dotnet run`. 
* El proceso busca de forma recursiva en el folder `/home/malobato/proyectos/`, esto se puede cambiar en el proceso.
* El proceso escribe el resultado en el fichero `/home/malobato/proyectos/io.csv`.

## Fichero de salida

El fichero de salida tiene las siguientes columnas:
* **Repository**: repositorio del proyecto
* **Utils**: Versión de útiles
* **Process**: Proceso del repositorio
* **Tipo**: Tipo de tabla, Kudu o HIVE
* **io**: Operación, escritura, lectura o ambas.
* **Schema**: Esquema de la tabla
* **TableName**: Nombre de la tabla
* **FullTableName**: Esquema y nombre de la tabla
* **Tipo**: Proceso Python o Scala.

## Requisitos del código

**Scala**
El proceso busca los nombres de tabla mediante expresión regular en el fichero `entities.scala`. A continuación, analiza las entidades y detecta si son de tipo HIVE/KUDU, R/W y el nombre de la entidad.

**Python**
* Se consideran procesos los ficheros `.py` existentes en el folder `lib`. 
* Las lecturas se harán en este proceso y no fuera de él. 
* Los nombres de las tablas serán en mayúsculas y empezarán con `TABLENAME_`
* Las relaciones entre el nombre de la constante y el nombre de la tabla la busca en el fichero `share/tags` o `share/constants`.