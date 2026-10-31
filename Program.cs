
using ProgramaPasteleria.Models;
using ProgramaPasteleria.Services;
using Spectre.Console;

class Program {
    //clase para control de opciones en el bucle
    public static bool correr = true;
    //arreglo para opcioness
    public static string[] opciones = [
        "1. Mostrar lista de productos",
        "2. Agregar producto",
        "3. Eliminar producto",
        "4. Salir"
    ];

    public static PastelService pastelService = new();

    public static void Main() { //inicio del programa (metodo)
        //invoacion de metodo de Spectre.Console
        AnsiConsole.MarkupLine("[yellow] - BIENVENIDOS A D'JOSKIES PASTERIA -[/]");

        //bucle de control para el munu de opcione
        while (correr) {
            //intrucciones
            var seleccionar = new SelectionPrompt<string>()
                .Title("[DarkRed_1] ¿QUÉ DESEA HACER?[/]")
                .HighlightStyle("DeepPink2")
                .AddChoices(opciones);

            var opcion = AnsiConsole.Prompt(seleccionar);

            //mostrar la lista de pastees del archivo PastelService.cs
            List<Pastel> pasteles = pastelService.FindAll();

            //evaluar opcion seleccionada
            switch (opcion) {
                case "1. Mostrar lista de productos":
                    AnsiConsole.Clear();

                    var table = new Table();
                    table.AddColumn("Id");
                    table.AddColumn("Nombre");
                    table.AddColumn("Descripcion");
                    table.AddColumn("Precio");

                    //insertar valores
                    foreach (Pastel pastel in pasteles) {
                        table.AddRow(
                            pastel.Id.ToString(),
                            pastel.Nombre,
                            pastel.Descripcion,
                            pastel.Precio.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)
                        );
                    }

                    //mostrra tablaaa
                    AnsiConsole.Write(table);

                    break;

                case "2. Agregar producto":
                    AnsiConsole.Clear();

                    //solicitar los datos del nuevo pastel
                    string nombre = AnsiConsole.Ask<string>("[yellow]Ingrese el nombre del pastel:[/]");
                    string descripcion = AnsiConsole.Ask<string>("[yellow]Añade un descripcion:[/]");
                    decimal precio = AnsiConsole.Ask<decimal>("[yellow]¿Cuál será su precio?:[/]");

                    //crea nuevo pastel
                    Pastel nuevoPastel = new Pastel {
                        Id = 0,
                        Nombre = nombre,
                        Descripcion = descripcion,
                        Precio = precio
                    };

                    //invoca un metodo para agregar el pastel
                    pastelService.Add(nuevoPastel);

                    AnsiConsole.MarkupLine("[green]Pastel agregado correctamente.[/]");

                    break;

                case "3. Eliminar producto":
                    AnsiConsole.Clear();

                    //muestra los pasteles disponibles
                    if (pasteles.Count == 0) {
                        AnsiConsole.MarkupLine("[red]No hay pasteles para eliminar.[/]");
                        break;
                    }

                    //seleccionar el pastel que desea eliminar
                    var pastelSeleccionado = AnsiConsole.Prompt(
                        new SelectionPrompt<Pastel>()
                            .Title("[yellow]Selecciona el pastel que desea eliminar:[/]")
                            .UseConverter(p => $"{p.Id}. {p.Nombre}")
                            .AddChoices(pasteles)
                    );

                    //invocar metodo para eliminar el pastel
                    pastelService.Delete(pastelSeleccionado.Id);

                    AnsiConsole.MarkupLine("[red]Pastel eliminado.[/]");

                    break;

                case "4. Salir":
                    correr = false;
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine("[red] Cerrando programa...[/]");
                    correr = false;
                    break;
            }
        }
    }
}
