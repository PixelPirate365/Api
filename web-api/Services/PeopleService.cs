using web_api.Models;

namespace web_api.Services {
    public class PeopleService : IPeopleService {
        private List<Person> data;

        public PeopleService() {
            data = new List<Person> {
                new Person() { Id = 1, Name = "orj", Pwd = "12354" }
            };
        }

        public List<Person> GetAll() {
            return data;
        }

        public Person GetOne(int id) {
            return data.FirstOrDefault(x => x.Id == id);
        }

        public Person Add(Person person) {
            person.Id = 1 + data.Max(x => x.Id);
            data.Add(person);
            return person;
        }

        public Person Update(int id, Person person) {
            var existingPerson = data.FirstOrDefault(x => x.Id == id);
            if (existingPerson != null) {
                data.Remove(existingPerson);
                person.Id = id;
                data.Add(person);
            }
            return person;
        }

        public void DeleteOne(int id) {
            var personToRemove = data.FirstOrDefault(x => x.Id == id);
            if (personToRemove != null) {
                data.Remove(personToRemove);
            }
        }
    }
}
