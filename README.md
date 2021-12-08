# NotificacionesSDG
## Introducción
Esta prueba consiste en la creación del backend para un sistema de notificaciones.

En la aplicación dispondremos de 2 actores:
- El **lector**: Se considera lector a aquel que se puede anotar y recibir notificaciones.
- El **publicador**: Se considera publicador a aquel que puede enviar notificaciones y
obtener información de lectores.

## Requisitos
- Un lector puede anotarse o desanotarse a los mensajes suministrados por la
aplicación.
- Proveer de una API Rest que permita al publicador:
-- Enviar mensajes.
-- Obtener información de los lectores anotados a los mensajes en un momento
dado.
- Los mensajes no deben persistir en ningún sitio, simplemente se enviarán a los
lectores que estén anotados en el momento del envío.
- Log de todas las acciones realizadas por el backend según nivel de criticidad.
- No será necesario implementar seguridad para el acceso de los publicadores y
lectores.

## Instalación
NotificacionesSDG requiere instalar RabbitMQ 3.9.11, Erlang y .NET 6.0

https://www.rabbitmq.com/download.html

https://www.erlang.org/downloads

https://dotnet.microsoft.com/download/dotnet/6.0

Además de restaurar los nugets antes de lanzar la aplicación.

## Ejecución
El servicio de RabbitMQ debe estar levantado. Por defecto en el puerto 5672.

Inicie los siguientes proyectos de la solución. Click derecho -> Propiedades -> Proyectos de inicio multiples, si está trabajando con Visual Studio.
- **LectorPrueba1**: Aplicación de consola para representar a un lector.
- **LectorPrueba2**: Aplicación de consola para representar a un segundo lector.
- **NotificacionesServicio**: API del publicador.

Una vez levantado se mostrarán:
- 2 Aplicaciones de consola con las que se puede interactuar para suscribirse o desuscribirse de los mensajes del publicador.
- 1 Aplicación de consola con el Log del Backend.
- 1 Pestaña del navegador para interactuar como publicador con la API mediante Swagger. (En mi caso: https://localhost:7150/swagger/index.html)

Usos:
- Interactue con las consolas de los lectores para suscribirse o desuscribirse de los mensajes.
- Envie mensajes como publicador desde el Swagger a los lectores suscritos. (Endpoint publicador/enviar)
- Consulte la información de los lectores suscritos en ese momento desde el Swagger. (Endpoint publicador/lectoressuscritos)
- Revise los mensajes de la consola del log.
 
**Nota: Los endpoints que aparecen en Swagger lector/suscribirse y lector/desuscribirse son para la comunicación interna de la aplicación, no deben usarse desde el navegador.
