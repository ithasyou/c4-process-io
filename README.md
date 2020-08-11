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


## Otros Scripts incluidos

* **closemvnversion.sh**
Se encarga de cerrar la versión actual y generar una nueva versión del repositorio.
Se ejecuta en el repositorio y recibe como parámetro la versión actual y siguiente
`closemvnversion.sh 1.0.0 1.0.1`

* **deploy-to-desa.sh**
Ejecuta el comando `mvn install` y copia los paquetes al servidor de desarrollo.

* **install.sh**
Para ejecutar dentro de la máquina de desarrollo `ld6mk02`. Se copia dentro de la carpeta con el mismo nombre que el repositorio, busca el paquete en la raíz y lo instala.

* **deploy.sh**
Auxiliar, para ejecutar dentro de la máquina de desarrollo `ld6mk02`. Necesario para el script `install.sh`.

* **sshdesa.sh**
Se conecta vía ssh a DESA.

* **mvnversion.sh**
Actualiza la versión del repositorio actual mediante comandos de maven

* **pydeploy.sh**
* Para ejecutar dentro de la máquina de desarrollo `ld6mk02`, desde el directorio raíz. Recibe como parámetro el nombre del repositorio y se encarga de instalarlo.

* **pydeploy-to-desa.sh**
* Ejecuta el comando `gradlew build` y copia los paquetes al servidor de desarrollo.



