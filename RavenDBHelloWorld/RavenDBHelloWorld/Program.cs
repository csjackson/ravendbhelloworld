using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Document;

namespace RavenDBHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            string firstName = "";
            while (firstName != "Exit")
          {
            Console.WriteLine("Please enter client's first Name:");
            Console.WriteLine("Or type 'EXIT' to end the program.");
            firstName = Console.ReadLine().Trim();
            if (firstName == "EXIT") return;
            Console.WriteLine("Please enter client's last Name:");
            string lastName = Console.ReadLine().Trim();
            Console.WriteLine("Please enter client's street address:");
            string firstAddress = Console.ReadLine().Trim();
            Console.WriteLine("Please enter client's apartment or suite number, if any:");
            string secondAddress = Console.ReadLine().Trim();
            Console.WriteLine("Please enter client's city:");
            string cityInput = Console.ReadLine().Trim();
            Console.WriteLine("Please enter client's state:");
            string stateInput = Console.ReadLine().Trim();
            Console.WriteLine("Please enter client's zip code");
            string zipCode = Console.ReadLine().Trim();

            using (var store = new DocumentStore { Url = "http://localhost:8080" })
            {
                store.Initialize();
                using (var session = store.OpenSession())
                {
                    Person person;
                    IQueryable<Person> ExistingPerson = from Person in session.Query<Person>()
                                            where (Person.FirstName == firstName && Person.LastName == lastName)
                                            select Person;
                   
                          person = new Person
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            PersonAddress = new Address
                            {
                                zip = zipCode,
                                State = stateInput,
                                City = cityInput,
                                AddressFirstLine = firstAddress,
                                AddressSecondLine = secondAddress
                            }
                        };
                          if (ExistingPerson != null)
                          {
                              person.Id = session.Advanced.GetDocumentId(ExistingPerson);
                          }
                   
                    session.Store(person);
                    session.SaveChanges();
                }
            }
          }
        }

        public class Person
        {
            public string FirstName { get ; set ; }
            public string LastName { get; set ; }
            public string Id { get; set; }
            public Address PersonAddress {get ; set ; }

        }

        public class Address
        {
            public string zip { get; set; }
            public string State { get ; set ; }
            public string City { get; set; }
            public string AddressFirstLine { get; set; }
            public string AddressSecondLine { get; set; }
        }

   

    }
    
}
