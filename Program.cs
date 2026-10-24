var jobs = new List<Job>();

Console.Write("Ingrese la cantidad de tareas (2 a 10): ");
int jobsAmount;
while (!int.TryParse(Console.ReadLine(), out jobsAmount) || jobsAmount is < 2 or > 10) {
    Console.Write("Valor inválido. Ingresa un número de 2 a 10: ");
}

Console.WriteLine("A continuación ingresa los datos de las tareas (tiempo es en minutos)");

for (int i = 1; i <= jobsAmount; i++) {
    Console.Write($"Nombre de la tarea {i}: ");
    string? name = Console.ReadLine();
    while (string.IsNullOrWhiteSpace(name)) {
        Console.Write("El nombre no puede estar vacío. Intenta de nuevo: ");
        name = Console.ReadLine();
    }

    Console.Write($"Duración de la tarea {i}: ");
    double duration;
    while (!double.TryParse(Console.ReadLine(), out duration) || !double.IsFinite(duration) || duration <= 0) {
        Console.Write("\nDuración inválida (mayor a cero). Intenta de nuevo: ");
    }

    jobs.Add(new Job(name.Trim(), duration));
}

var bestOrder = GeneratePermutations(jobs, []).MinBy(CalculateTotalWait)!;

Console.WriteLine("\n****** RESULTADO ******");
Console.WriteLine($"Cantidad de órdenes: {ExplainFactorial(jobsAmount)}");
Console.WriteLine($"Mejor orden: {string.Join(" -> ", bestOrder.Select(job => job.Name))}");
Console.WriteLine($"Espera total acumulada: {CalculateTotalWait(bestOrder):F2} min");

if (!Console.IsInputRedirected) {
    Console.WriteLine("\nPresiona una tecla para salir...");
    Console.ReadKey();
}

static IEnumerable<List<Job>> GeneratePermutations(List<Job> remaining, List<Job> current) {
    if (remaining.Count == 0) {
        yield return current;
        yield break;
    }

    for (int i = 0; i < remaining.Count; i++) {
        var others = remaining.Where((_, index) => index != i).ToList();
        var next = current.Append(remaining[i]).ToList();

        foreach (var order in GeneratePermutations(others, next))
            yield return order;
    }
}

static double CalculateTotalWait(List<Job> order) {
    double elapsed = 0;
    double totalWait = 0;

    foreach (var job in order) {
        totalWait += elapsed;
        elapsed += job.Duration;
    }

    return totalWait;
}

static string ExplainFactorial(int n) {
    var factors = Enumerable.Range(1, n).Reverse().ToList();
    return $"{n}! = {string.Join(" x ", factors)} = {factors.Aggregate((a, b) => a * b)}";
}

public record Job(string Name, double Duration);
