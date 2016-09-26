using NetrikaServices.PixService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetrikaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const string idLpu = "1.2.643.5.1.13.3.25.78.118";
            const string guid = "8CDE415D-FAB7-4809-AA37-8CDD70B1B46C";
            Console.WriteLine("Run program");
            PixServiceClient client = new PixServiceClient();
            client.InnerChannel.Faulted += InnerChannel_Faulted;
            PatientDto patient = new PatientDto();
            patient.GivenName = "Андрей";
            patient.MiddleName = "Викторович";
            patient.FamilyName = "Лобов";
            patient.BirthDate = new DateTime(1989, 11, 6);
            patient.IdPatientMIS = "1000000";
            patient.Sex = 0;

            var testTaskGuid = "8CDE415D­FAB74809­AA37­8CDD70B1B46C".ToCharArray();
            var developersLetterGuid = "8CDE415D-FAB7-4809-AA37-8CDD70B1B46C".ToCharArray();
            foreach (var item in testTaskGuid)
            {
                Console.Write(item);
            }
            Console.WriteLine();

            foreach (var item in developersLetterGuid)
            {
                Console.Write(item);
            }
            Console.WriteLine();
            ////var result = a + b;
            ////Assert.Equals(result, expected);
            ////client.Open();
            try
            {
                //client.InnerChannel.Faulted += InnerChannel_Faulted;
                client.AddPatient(guid, "", patient);
            }
            catch (FaultException e)
            {
                
                Console.WriteLine(e.Message);               
            }
            
            
            Console.WriteLine(Guid.NewGuid().ToString());
            Console.WriteLine("End program");
            Console.ReadLine();
        }


        private static void InnerChannel_Faulted(object sender, EventArgs e)
        {
            var requestFault = (RequestFault)sender;
            Console.WriteLine(requestFault.ErrorCode.ToString());
            //throw new Exception(requestFault.ErrorCode.ToString());
        }
    }
}
