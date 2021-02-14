# ATSS

Airline Ticket Sales System

## Inspiracje

Wzorowałem się na Clean Architecture i prezentacji: https://www.youtube.com/watch?v=dK4Yb6-LxAk&t.
Jako punkt wyjścia dla solucji wykorzystałem szablon Clean.Architecture.Solution.Template: https://github.com/jasontaylordev/CleanArchitecture.

## Kilka słów wyjaśnień

Warstwa domenowa jest tutaj rozbita między dwa projekty. `Domain` gdzie mamy logikę `Enterprise` i projekt `Application` gdzie mamy logikę biznesową.
W niektórych miejscach poszedłem na skróty. Przydałoby się więcej testów np. dla walidatorów.
Przy sprawdzaniu kryteriów dla zniżek wykorzystałem Dynamic.Linq (https://dynamic-linq.net/#:~:text=What's%20Dynamic%20LINQ,ParseLambda%20%2C%20Parse%20%2C%20and%20CreateClass%20.)
i obsługuje tylko jeden operator porównania.
Było to dla mnie szybsze do implementacji niż przykładowo wykorzystanie refleksji na obiektach.
FlightId zaimplementowałem jako ValueObject nie wiem czy słusznie ale pomyślałem, że może poszczególne segmenty ID są istotne.
