using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Async
{
    class Program
    {
        static void Main(string[] args)
        {
            run();
        }

        public static void run()
        {
            Console.WriteLine("Insert email: ");
            string email = Console.ReadLine();
            getPerson(email);
        }
        public static async void getPerson(string email)
        {
            var client = new FullContactClient();
            var person = await client.LookupPersonByEmailAsync(email);
            if (person != null)
            {
                printPerson(person);
            }
        }

        public static void printPerson(FullContactPerson person)
        {
            Console.WriteLine("------ Fullcontact API ------");
            Console.WriteLine("Likelihood:" + person.Likelihood);
            Console.WriteLine("------ Contact info ------");
            Console.WriteLine("Familyname: " + person.ContactInfo.FamilyName);
            Console.WriteLine("Fullname: " + person.ContactInfo.FullName);
            Console.WriteLine("Givenname: " + person.ContactInfo.GivenName);
            Console.WriteLine("Websites: ");
            foreach (Website ws in person.ContactInfo.Websites)
            {
                Console.WriteLine(" Url: " + ws.Url);
            }
            Console.WriteLine("Chats:");
            foreach (Chat chat in person.ContactInfo.Chats)
            {
                Console.WriteLine(" Client: " + chat.Client);
                Console.WriteLine(" Handle: " + chat.Handle);
                Console.WriteLine();
            }
            Console.WriteLine("------ SocialProfiles ------");
            Console.WriteLine("SocialProfiles: ");
            foreach (SocialProfile sp in person.SocialProfiles)
            {
                Console.WriteLine(" Bio: " + sp.Bio);
                Console.WriteLine(" Type: " + sp.Type);
                Console.WriteLine(" TypeId: " + sp.TypeId);
                Console.WriteLine(" TypeName: " + sp.TypeName);
                Console.WriteLine(" Url: " + sp.Url);
                Console.WriteLine(" UserName: " + sp.UserName);
                Console.WriteLine();
            }

        }
    }

    public interface IFullContactApi
    {
        Task<FullContactPerson> LookupPersonByEmailAsync(string email);
    }

    public class FullContactClient : IFullContactApi
    {
        private const string api = "&apiKey=";
        private const string url = "https://api.fullcontact.com/v2/person.json?email=";

        public async Task<FullContactPerson> LookupPersonByEmailAsync(string email)
        {
            FullContactPerson fcp = null;
            String accessURL = url + email + api;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(accessURL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(accessURL).Result;
            if (response.IsSuccessStatusCode)
            {
                fcp = await response.Content.ReadAsAsync<FullContactPerson>();
            }
            else
            {
                Console.WriteLine("Not successful!");
            }

            return fcp;
        }
    }

    public class FullContactPerson
    {
        public float Likelihood { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public List<SocialProfile> SocialProfiles { get; set; }
    }

    public class SocialProfile
    {
        public string Bio { get; set; }
        public string Type { get; set; }
        public string TypeId { get; set; }
        public string TypeName { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
    }

    public class ContactInfo
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string FullName { get; set; }
        public List<Website> Websites { get; set; }
        public List<Chat> Chats { get; set; }
    }

    public class Chat
    {
        public string Client { get; set; }
        public string Handle { get; set; }
    }

    public class Website
    {
        public string Url { get; set; }
    }

}

