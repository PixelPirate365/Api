using web_api.Models;

namespace web_api.Services {
    public interface IPeopleService {
        Person Add(Person person);
        void DeleteOne(int id);
        List<Person> GetAll();
        Person GetOne(int id);
        Person Update(int id, Person person);
    }
}