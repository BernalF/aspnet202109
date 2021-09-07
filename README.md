# ASP.NET Core MVC
Material relacionado con el curso de ASP.NET Core MVC impartido en UCenfotec (modalidad virtual) en Setiembre 2021.

### Requerimientos de software
1.	Requisitos mínimos del hardware que ocupamos. 
	https://docs.microsoft.com/en-us/visualstudio/releases/2019/system-requirements
	
2.	Última versión del Microsoft .NET Core SDK
	https://www.microsoft.com/net/download/windows,  el de 64bits aquí, 
	https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.402-windows-x64-installer
	
	Utilizar el Windows Installer de acuerdo con la versión de Windows que se esté utilizando.
	Se hace efectuar una ejecución inicial para descargar los paquetes iniciales. 
	Estos pasos crean una pequeña aplicación de línea de comandos que imprime la palabra “Hello World” en la consola. 
	
	Ejecutar desde la línea de comandos: 
	
	mkdir t1<br/>
	cd t1<br/>
	dotnet new console

	En este paso puede aparecer un mensaje que señala que se están descargando los paquetes iniciales de .NET Core. 
	Esperar a que se complete la descarga.
		
	dotnet build<br/>
	dotnet run

3.	Última versión del .NET Core Hosting Bundle 
	https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-aspnetcore-5.0.9-windows-hosting-bundle-installer

4.	Microsoft Visual Studio Code 
	https://code.visualstudio.com/
	Instalar o actualizar a la última versión.
	
5.	Microsoft SQL Server 2008 R2 o superior. 
	https://www.microsoft.com/en-us/sql-server/sql-server-downloads
	Se acostumbra a utilizar la edición Express, en SQL Server 2017 para desarrollo es posible utilizar la edición Developer.	
	
6.	Microsoft Visual Studio 2019 (edición Community o superior) 
	https://www.visualstudio.com/downloads/
	https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2019#install-workloads
	
	Aquí se documenta como obtener los instaladores para la instalación local, 
	https://docs.microsoft.com/en-us/visualstudio/install/create-a-network-installation-of-visual-studio?view=vs-2019, 
	esto baja todos los “workloads” pero al momento de instalar no se deben de instalar todos.
	
	Si el Visual Studio 2019 ya se encuentra instalado se puede utilizar el Visual Studio Installer, 
	para efectuar la actualización.

	Se deben instalar al menos los “workloads”: 
	- Windows 
		+ .NET desktop development
	- Web & Cloud 
		+ ASP.NET and web development
		+ Data storage and processing
	- Other Toolsets 
		+ .NET Core cross-platform development
		
	En caso de contar con una instalación del Visual Studio 2019, proceder con la actualización a la última versión, 
	y confirmar que se tengan instalados los “workloads” señalados en el punto anterior. Esto se hace ejecutando el 
	Visual Studio Installer, y aplicar en el equipo la actualización cuando aparece el botón “Update”, es solo de 
	aplicarlo y esperar que finalice.
 
	Se puede confirmar el resultado con el “Acerca de” de Visual Studio 2019.
	
7.	Internet Information Services habilitado 
	http://technet.microsoft.com/en-us/library/cc731911.aspx
	
8.	Web Deploy 3.6 
	http://www.iis.net/downloads/microsoft/web-deploy.  El enlace del instalador se encuentra en la parte inferior 
	de la página.
	
9.	Navegadores Web actualizados a la última versión. 
	https://www.mozilla.org/en-US/firefox/
	https://www.google.com/chrome/index.html
	https://www.microsoft.com/en-us/edge

10. Postman
	https://www.getpostman.com/apps	
	
De ser posible efectuar la instalación de las versiones del software con el idioma inglés, para unificar con la configuración utilizada por el profesor.

### Bases de datos de ejemplo
* https://github.com/Microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs
