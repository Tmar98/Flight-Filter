using System;
using Gridnine.FlightCodingTest;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FlightFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Write all flights

            FlightBuilder flightBuilder = new FlightBuilder();

            var flights = flightBuilder.GetFlights();

            foreach(var flight in flights )
            {
                Console.WriteLine(FlightOutputString(flight));
            }
            #endregion

            DateTime timeNow = DateTime.Now;

            #region First task
            Console.WriteLine("\nСписок авиаперелетов с вылетом после текущего момента: {0}\n" , timeNow);

            foreach(var flight in flights)
            {

                var seg = flight.Segments.FirstOrDefault();
                if(seg.DepartureDate > timeNow)
                {
                    Console.WriteLine(FlightOutputString(flight));
                }
                
            }
            #endregion

            #region Second task
            Console.WriteLine("\nСписок авиаперелетов без сегментов у которых дата прилета раньше даты вылета\n");

            foreach (var flight in flights)
            {
                bool datebool = true;
                foreach (var segment in flight.Segments)
                {
                    if (segment.ArrivalDate < segment.DepartureDate)//проверка на правильность дат
                    {
                        datebool = false;
                    }
                    
                }
                if (datebool)//вывод всех правильных дат
                {
                    Console.WriteLine(FlightOutputString(flight));
                }
            }
            #endregion


            #region Third task
            Console.WriteLine("\nСписок авиаперелетов чье общее время, проведённое на земле, превышает два часа\n");

            foreach (var flight in flights)
            {
                if (flight.Segments.Count == 1)//полеты с одним сегментом на земле не ждут, поэтому сразу выводятся
                {
                    Console.WriteLine(FlightOutputString(flight));
                }
                else  //если более 1го сегмента
                {
                    int secondsOnLand = 0;
                    for (int i = 0; flight.Segments.Count > i+1; i++)
                    {
                        TimeSpan inequality = flight.Segments[i+1].DepartureDate.Subtract(flight.Segments[i].ArrivalDate);//различие во времени между посадкой и взлетом

                        if(inequality.TotalSeconds>0)
                        {
                            secondsOnLand += Convert.ToInt32(inequality.TotalSeconds);//секунд проведенных на земле
                        }
                        else  // отрицательное время не может быть
                        {
                            secondsOnLand = 10000;
                        }
                    }
                    if(secondsOnLand < 7200)//проверка на нахождение на земле больше двух часов 
                    {
                        Console.WriteLine(FlightOutputString(flight));
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Сoncatenation segments to output string
        /// </summary>
        /// <param name="flight"></param>
        /// <returns></returns>
        public static string FlightOutputString(Flight flight)
        {
            string flight_string = "";
            foreach (var segment in flight.Segments)
            {
                flight_string += segment.DepartureDate + " " + segment.ArrivalDate + " ";
            }
            return flight_string;
        }
    }
}
