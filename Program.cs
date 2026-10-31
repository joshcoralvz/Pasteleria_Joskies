using ProgramaPasteleria.Models;
using ProgramaPasteleria.Services;
using Spectre.Console;

class Program {
    // Clase para control de opciones en el bucle
    public static bool correr = true;
    
    // Arreglo para opciones corregido
    public static string[] opciones = [
        "1. Mostrar productos",
        "2. Agregar producto",
        "3. Eliminar producto",
        "4. Salir"
    ];
 
    public static PastelService pastelService = new();

    public static void Main() { // Inicio del programa (método)
        // Invocación de método de Spectre.Console
        AnsiConsole.MarkupLine("[yellow] - BIENVENIDOS A D'JOSKIES PASTERIA -[/]");
        
        // Bucle de control para el menú de opciones
        while (correr) {
            // Instrucciones
            var seleccionar = new SelectionPrompt<string>()
                .Title("[DarkRed_1] ¿QUÉ DESEA HACER?[/]")
                .HighlightStyle("DeepPink2")
                .AddChoices(opciones);
                
            var opcion = AnsiConsole.Prompt(seleccionar);
            
            // Mostrar la lista de pasteles del archivo PastelService.cs
            List<Pastel> pasteles = pastelService.FindAll();
            
            // Evaluar opción seleccionada
            switch (opcion) {
                case "1. Mostrar productos":
                    AnsiConsole.Clear();
                    var table = new Table();
                    table.AddColumn("Id");
                    table.AddColumn("Nombre");
                    table.AddColumn("Descripcion");
                    table.AddColumn("Precio");

                    // Insertar valores
                    foreach (Pastel pastel in pasteles) {
                        table.AddRow(
                            pastel.Id.ToString(),
                            pastel.Nombre,
                            pastel.Descripcion,
                            pastel.Precio.ToString("C") // Formato de moneda opcional
                        );
                    }

                    // Mostrar tabla
                    AnsiConsole.Write(table);
                    
                    // Pausa para que el usuario pueda ver la tabla antes de volver al menú
                    AnsiConsole.MarkupLine("\n[grey]Presiona una tecla para continuar...[/]");
                    Console.ReadKey();
                    AnsiConsole.Clear();
                    break;

                case "2. Agregar producto":
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[bold green]-- REGISTRA UN NUEVO PASTEL --[/]");
    
                // Solicitar datos usando Spectre.Console
                var nombre = AnsiConsole.Ask<string>("Ingrese el [yellow]nombre[/] del pastel:");
                var descripcion = AnsiConsole.Ask<string>("Ingrese la [yellow]descripción[/]:");
                var precio = AnsiConsole.Ask<decimal>("Ingrese el [yellow]precio[/]:");

                // Al tener la propiedad 'required', pasamos el Id en 0 (el servicio le asignará el correcto)
                Pastel nuevoPastel = new Pastel {
                     Id = 0, 
                     Nombre = nombre,
                     Descripcion = descripcion,
                     Precio = precio
                };

                // Guardar usando el servicio
                pastelService.Add(nuevoPastel);

                AnsiConsole.MarkupLine("[green]Producto agregado!![/]");
                Thread.Sleep(1500);
                AnsiConsole.Clear();
                break;

                case "3. Eliminar producto":
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine("[bold red]-- ELIMINAR PRODUCTO --[/]");

                    if (pasteles.Count == 0) {
                        AnsiConsole.MarkupLine("[yellow]No hay productos registrados para eliminar.[/]");
                    } else {
                        // Creamos una lista de opciones basadas en los nombres o IDs de los pasteles existentes
                        var pastelASeleccionar = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Seleccione el producto que desea eliminar:")
                                .AddChoices(pasteles.Select(p => $"{p.Id} - {p.Nombre}").Append("Cancelar"))
                        );

                        if (pastelASeleccionar != "Cancelar") {

                            var idString = pastelASeleccionar.Split(' ')[0];
                            if (int.TryParse(idString, out int idEliminar)) {
                                pastelService.Delete(idEliminar); 
                                AnsiConsole.MarkupLine("[red]Se eliminó el producto[/]");
                            }
                        }
                    }

                    Thread.Sleep(1500);
                    AnsiConsole.Clear();
                    break;

                case "4. Salir":
                    correr = false;
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine("[red]Cerrando programa...[/]");
                    break;
            }
        }
    }
}
