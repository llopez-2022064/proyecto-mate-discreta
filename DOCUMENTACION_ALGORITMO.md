# Planificador de tareas por permutaciones

**Objetivo:** encontrar el orden de atención de un grupo de tareas que haga esperar lo menos posible, en total, a todas ellas.

---

## 1. Resumen

El programa pide una lista de tareas (nombre y duración en minutos), prueba todos los órdenes posibles en que se pueden realizar y muestra cuál es el mejor, es decir, el que genera menos espera acumulada.

---

## 2. Problema que resuelve

Cuando hay un solo recurso (una máquina, un técnico, una impresora, un procesador) y varias tareas por hacer, el orden importa. Si se atiende primero una tarea larga, todas las demás esperan mucho. Si se atienden primero las cortas, la espera total baja.

El programa busca ese orden de forma sencilla: revisa todas las opciones y elige la mejor.

### Supuestos

- Todas las tareas están disponibles desde el inicio.
- La duración de cada tarea se conoce de antemano.
- No hay prioridades ni dependencias entre tareas.
- Solo se hace una tarea a la vez, y cada una termina antes de empezar la siguiente.

---

## 3. Caso de aplicación en ingeniería: taller de mantenimiento

Un taller tiene **un solo técnico** y cinco órdenes de trabajo pendientes. Mientras un equipo espera su reparación, está parado y la empresa pierde producción. El objetivo es atender las órdenes en el orden que reduzca el tiempo total que los equipos pasan esperando.

| Orden | Duración (min) |
| --- | --- |
| A | 90 |
| B | 20 |
| C | 45 |
| D | 30 |
| E | 60 |

| Estrategia | Orden | Espera total | Espera promedio |
| --- | --- | --- | --- |
| Por orden de llegada | A → B → C → D → E | 540 min | 108 min |
| Sugerido por el programa | B → D → C → E → A | 320 min | 64 min |

Con el mismo técnico y las mismas reparaciones, la espera total baja alrededor de **41 %**. Mejora el servicio sin contratar personal ni comprar equipo.

El mismo problema aparece en otros ámbitos de la ingeniería:

- Procesos esperando turno en el procesador de una computadora.
- Trabajos en una cola de impresión.
- Pedidos que pasan por una misma máquina en una línea de producción.

---

## 4. Entradas y salidas

### Entradas

| Dato | Regla |
| --- | --- |
| Cantidad de tareas | Número entero de 2 a 10 |
| Nombre de cada tarea | No puede estar vacío ni ser solo espacios |
| Duración de cada tarea | Número mayor a cero, en minutos (se aceptan decimales) |

Si un dato no cumple la regla, el programa lo vuelve a pedir hasta recibir uno válido.

Los nombres pueden repetirse: cada tarea ingresada se trata como una tarea distinta.

### Salidas

- **Cantidad de órdenes posibles**, expresada como factorial. Por ejemplo: `3! = 3 x 2 x 1 = 6`.
- **Mejor orden** encontrado, por ejemplo: `B -> C -> A`.
- **Espera total acumulada** de ese orden, en minutos con dos decimales.

---

## 5. ¿Cómo se decide cuál orden es mejor?

### La espera de una tarea

Es el tiempo que pasa desde el inicio hasta que la tarea empieza. Equivale a la suma de las duraciones de las tareas que van antes. La primera tarea siempre espera 0 minutos.

### La espera total

Es la suma de las esperas de todas las tareas. Esta es la cifra que el programa intenta minimizar. Si se divide entre la cantidad de tareas, se obtiene la **espera promedio**, que es útil para comparar casos con distinta cantidad de tareas.

### Un detalle importante

La espera total **no es** lo que tarda en terminar todo el trabajo. El tiempo en que terminan todas las tareas es la suma de sus duraciones y es el mismo en cualquier orden, así que no sirve para elegir. Lo que cambia entre un orden y otro es cuánto esperó cada tarea antes de empezar.

### Empates

Si dos órdenes tienen la misma espera total, el programa se queda con el primero que generó.

---

## 6. Cómo funciona el programa

1. Pide y valida la cantidad de tareas.
2. Pide y valida el nombre y la duración de cada tarea.
3. Genera todos los órdenes posibles (permutaciones).
4. Calcula la espera total de cada orden.
5. Se queda con el orden de menor espera total.
6. Muestra la cantidad de órdenes, el mejor orden y su espera total.

```text
Inicio
    Pedir la cantidad de tareas y validarla.
    Pedir nombre y duración de cada tarea y validarlos.
    Generar todos los órdenes posibles.
    Para cada orden:
        Calcular la espera de cada tarea.
        Sumar las esperas.
    Elegir el orden con la menor suma.
    Mostrar el resultado.
Fin
```

---

## 7. El código por partes

El programa está escrito en C#.

| Parte | Qué hace |
| --- | --- |
| `Job` | Guarda los datos de una tarea: su nombre y su duración. |
| `GeneratePermutations` | Arma todos los órdenes posibles. Toma una tarea como primera, y con las que quedan repite el proceso hasta acomodarlas todas. Entrega cada orden conforme lo arma (con `yield return`), sin guardarlos todos a la vez. |
| `CalculateTotalWait` | Recorre un orden, va llevando la cuenta del tiempo transcurrido y suma cuánto esperó cada tarea. |
| `MinBy` | Es una función que ya trae .NET (LINQ). Compara todos los órdenes y devuelve el que tiene menor espera total. |
| `ExplainFactorial` | Muestra de forma legible cuántos órdenes existen, por ejemplo `3! = 3 x 2 x 1 = 6`. |

---

## 8. Ejemplo paso a paso

**Entrada:** A = 3 min, B = 1 min, C = 2 min.

Con 3 tareas hay 3! = 6 órdenes posibles:

| Orden | Esperas individuales (min) | Espera total (min) |
| --- | --- | --- |
| A → B → C | 0, 3, 4 | 7 |
| A → C → B | 0, 3, 5 | 8 |
| B → A → C | 0, 1, 4 | 5 |
| **B → C → A** | **0, 1, 3** | **4** |
| C → A → B | 0, 2, 5 | 7 |
| C → B → A | 0, 2, 3 | 5 |

**Resultado:** el mejor orden es **B → C → A**, con **4 minutos** de espera total. Todas las tareas terminan en 6 minutos en cualquier orden, y esa cifra es distinta de la espera total.

---

## 9. Pruebas realizadas

| Caso | Entrada | Resultado esperado |
| --- | --- | --- |
| Orden óptimo | A: 3, B: 1, C: 2 | B → C → A, espera total 4.00, 6 órdenes |
| Mínimo de tareas | A: 5, B: 2 | B → A, espera total 2.00, 2 órdenes |
| Empate | A: 2, B: 2 | A → B, espera total 2.00 |
| Duraciones con decimales | A: 2.5, B: 1.5 | B → A, espera total 1.50 |
| Nombres repetidos | A: 2, A: 1 | A → A (primero la de 1 min), espera total 1.00 |
| Cantidad inválida | 1, 11 o texto | Vuelve a pedir la cantidad |
| Nombre vacío | Espacios o cadena vacía | Vuelve a pedir el nombre |
| Duración inválida | 0, negativo, texto, NaN o infinito | Vuelve a pedir la duración |

### Evidencias

![Figura 1: ejecución del caso de orden óptimo (A: 3, B: 1, C: 2)](image.png)

![Figura 2: ejecución con el mínimo de tareas (2)](image-1.png)

![Figura 3: caso de empate](image-2.png)

![Figura 4: validación de datos inválidos](image-3.png)

---

## 10. Rendimiento y limitaciones

El número de órdenes crece muy rápido con cada tarea que se agrega:

| Tareas | Órdenes a revisar |
| --- | --- |
| 3 | 6 |
| 5 | 120 |
| 8 | 40 320 |
| 10 | 3 628 800 |

Por eso el programa solo acepta hasta **10 tareas**: con más, el tiempo de espera y el uso de memoria se volverían demasiado grandes.

---

## 11. Observación importante

Para este problema existe un atajo: **el mejor orden siempre es de la tarea más corta a la más larga.** Basta con ordenar las tareas por duración, sin revisar todas las combinaciones. En ingeniería esto se conoce como el criterio *SPT* (*Shortest Processing Time*); en sistemas operativos, como *SJF* (*Shortest Job First*).

Este programa usa permutaciones porque permite **comprobar** ese resultado revisando todas las opciones. En un caso real con muchas tareas, ordenar sería muchísimo más rápido. Como ejemplo, en el caso del taller, el orden sugerido (B → D → C → E → A) es justamente el de menor a mayor duración (20, 30, 45, 60, 90).

---

## 12. Conclusión

El programa encuentra el orden que reduce la espera total revisando todas las opciones posibles. Es fácil de entender y sirve para comprobar que atender primero lo más corto es la mejor estrategia. Aplicado a un caso de ingeniería, como un taller de mantenimiento, permite reducir los tiempos de espera sin invertir en recursos adicionales.