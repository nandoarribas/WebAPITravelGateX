Prueba de código travelGatex

Como se puede comprobar, la solución del proyecto de la prueba de código se ha realizado con .net core 3.1

Para la serialización y deserialización pensé en principio usa Newtonsoft, pero dado que .net core proporciona System.Text.Json 
opté por continuar usando este.

Otra cosa a tener en cuenta es que para las operaciones sobre los listados y diccionarios, además de usar algunos bucles foreach se 
ha utilizado Linq para realiar de manera más rápida y eficiente la manipulación de los datos.

El proyecto lo he dividido en las siguientes carpetas

Util
    Para incluir los enums referentes a los mealplan y los roomtype
    Utils.cs donde he metido una función para la obtención de los datos de los proveedores (Atalaya y Resort)
Model
    Carpeta donde contendrá todos los modelos de las llamadas a los endpoints de Atalaya y Resort, así como los modelos de datos de salida de TravelGate
        Dentro de TravelGateModel se puede observar que se ha utilizado la directiva JsonIgnore para reutilizar las clases Room y HotelRoomInfo para los 
        ejercicios 1 y 2. También podría haberse hecho uso de herencia para estos casos.
Methods
    Carpeta donde se realizarán todas las operativas que haya que hacer con los modelos (creación, consultas, actualización...)
    Los métodos públicos serán los que sean llamados desde el Controller

Controller
    Carpeta que contendrán los endpoints de los ejercicios 1 y 2 (HotelList e Itineraries)
    
Mejoras
    Haber usado el patrón factory para la creación de objetos (como es el caso del Create en TravelGateMethods, pero en una clase HotelFactory)
    Control de errores de parámetros a la hora de crear los constructores 
    Usar sentencias try / catch para lanzar posibles excepciones en caso de que alguna acción pudiera producir errores "conocidos"


    Cómo se han planteado los ejercicos?

    Para el ejercicio 1, además de lo comentado brevemente arriba en la descripción de las carpetas del proyecto, para obtener toda la información completa 
    de los hoteles de nuestros proveedores, llamamos por separado a los 2 endpoints de atalaya y resort de obtener los hoteles.
    Una vez tenemos el listado de ambos, para rellenar todas las habitaciones con sus comidas y tarifas, se realiza el método UpdateMealPlanInfo, 
    en el cual se puede observar que para el rellenado del contenido de los hoteles de Atalaya se utilizan los endpoints de habitaciones y comidas juntos, 
    de modo que cuando van extrayéndose los planes de comida, vamos incluyendo el nombre de la habitación.
    Para los hoteles de Resort, con el segundo endpoint de la api es más sencillo este punto.
    *Nota: Dado que en este ejercicio los nodos de nombre de hotel son dinámicos, y no fijos ("acs","hrm"...), podría haber hecho uso de un Converter, 
    una solución que vi en internet que es muy buena para este tipo de problemas, pero finalmente por falta de tiempo decidí mantener mi solución 
    inicial, en la cual manipulo el nodo a mano y saco el nombre y los hijos (Sobre la línea 83 de AtalayaMethodsal ca)

    Para el ejercicio 2, se aprovecha la funcionalidad ya creada del ejercicio 1 para recuperar todos los hoteles y aplicarles el filtro de condiciones 
    recibidos en el json (como el del ejemplo). Una vez realizados los filtros de ciudad, y tipo de plan de comidas, se crean instancias de habitaciones 
    con el valor de la habitación ya multiplicado el número de noches y personas. 
    Se hace uso de un producto cartesiano dinámico para obtener todas las opciones posibles de habitaciones ordenando previamente por plan de comidas y 
    tipo de habitación. Ya una vez que tenemos esos datos, dado que es posible que haya más de un resultado que cuadre con el itinerario propuesto, 
    he realizado una modificación del ejercicio para devolver un conjunto de itinerarios (Se puede ver el modelo en TravelGateModel)

Ejemplo de llamada para el ejercicio 2 
(usando postman)
{
    "budget": 700,
    "DestinationNigths": 
    [
        {
            "destination": "Malaga",
            "nights": 3,
            "mealPlan": "" 
        },
        {
            "destination": "Cancun",
            "nights": 5,
            "mealPlan": "ad" 
        }
    ]
}