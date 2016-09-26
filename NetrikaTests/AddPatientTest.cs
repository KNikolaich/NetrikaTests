using System;
using NUnit.Framework;
using NetrikaServices.PixService;
using System.ServiceModel;

namespace NetrikaTests
{
    [TestFixture]
    public class AddPatientTest
    {
        private const string idLpu = "1.2.643.5.1.13.3.25.78.118";
        private const string guid = "8CDE415D-FAB7-4809-AA37-8CDD70B1B46C";
        PixServiceClient client = new PixServiceClient();  
        
        private PatientDto GetPatient()
        {
            var patient = new PatientDto()
            {
                GivenName = "Андрей",
                FamilyName = "Лобов",
                BirthDate = new DateTime(1989, 11, 6),
                IdPatientMIS = Guid.NewGuid().ToString(),
                Sex = 1
            };

            return patient;
        }

        private DocumentDto GetDocument()
        {
            var document = new DocumentDto()
            {
                IdDocumentType = 2,
                DocS = "AAAA",
                DocN = "123456",
                ProviderName = "Provider"
            };

            return document;
        }

        private AddressDto GetAddress()
        {
            var address = new AddressDto()
            {
                IdAddressType = 1,
                StringAddress = "SomeAddress"
            };
            return address;
        }

        private BirthPlaceDto GetBirthPlace()
        {
            var birthPlace = new BirthPlaceDto()
            {
                Country = "SomeCountry",
                Region = "SomeRegion",
                City = "SomeCity"
            };
            return birthPlace;
        }

        private JobDto GetJob()
        {
            var job = new JobDto() { CompanyName = "SomeCompany" };
            return job;
        }

        private ContactDto GetContact()
        {
            var contact = new ContactDto()
            {
                IdContactType = 1,
                ContactValue = "Contact"
            };
            return contact;
        }

        private string TryAddPatient(string guid, string idLpu, PixServiceClient clietn, PatientDto patient)
        {
            var errorMessage = string.Empty;
            try
            {
                client.AddPatient(guid, idLpu, patient);
            }
            catch (FaultException fe)
            {
                errorMessage = fe.Message;
            }

            return errorMessage;
        }
       
        [Test]
        public void Invalid_Guid()
        {
            var expectedError = "Неправильный идентификатор системы";
            var invalidGuid = "1";
            var patient = GetPatient();

            var error = TryAddPatient(invalidGuid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));                           
            
        }

        [Test]
        public void Invalid_IdLpu()
        {
            
            var expectedError = "Неверный идентификатор ЛПУ";
            var invalidIdLpu = "1";
            var patient = GetPatient();

            var error = TryAddPatient(guid, invalidIdLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Successfull_AddPatient()
        {
            var expectedError = string.Empty;
            var patient = GetPatient();
            
            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_BirthDate_IsNull()
        {
            var expectedError = "Поле BirthDate не может быть пустым";            
            PatientDto patient = new PatientDto()
                                    {
                                        GivenName = "Андрей",
                                        FamilyName = "Лобов",                
                                        IdPatientMIS = Guid.NewGuid().ToString(),
                                        Sex = 1                
                                    };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_BirthDate_Is_Bigger_Then_DeathTime()
        {
            var expectedError = "Поле BirthDate не может быть больше чем поле DeathTime";
            var patient = GetPatient();
            patient.DeathTime = new DateTime(1988,11,6);

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_GivenName_IsNull()
        {
            var expectedError = "Поле  GivenName не может быть пустым";
            var patient = GetPatient();
            patient.GivenName = null;

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_FamilyName_IsNull()
        {
            var expectedError = "Поле FamilyName не может быть пустым";
            var patient = GetPatient();
            patient.FamilyName = null;

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_IdBloodType_Is_Out_Of_Reference()
        {
            var expectedError = "Значение IdBloodType не соответствует коду группы крови (OID справочника: 1.2.643.2.69.1.1.1.3)";
            var patient = GetPatient();
            patient.IdBloodType = 255;

            var error = TryAddPatient(guid, idLpu, client, patient);
            
            Assert.That(error, Is.EqualTo(expectedError));
        }


        [Test]
        public void Patients_IdLivingAreaType_Is_Out_Of_Reference()
        {
            var expectedError = "Значение IdLivingAreaType не соответствует коду типа места жительства (Классификатор жителя города или села, OID 1.2.643.5.1.13.2.1.1.573)";
            var patient = GetPatient();
            patient.IdLivingAreaType = 255;

            var error = TryAddPatient(guid, idLpu, client, patient);
                        
            Assert.That(error, Is.EqualTo(expectedError));
        }


        [Test]
        public void Patients_Sex_Is_Null()
        {
            var expectedError = "Значение поля Sex не должно быть пустым";
            PatientDto patient = new PatientDto()
                                    {
                                        GivenName = "Андрей",
                                        FamilyName = "Лобов",
                                        BirthDate = new DateTime(1989, 11, 6),
                                        IdPatientMIS = Guid.NewGuid().ToString()                                     
                                    };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }
        

        [Test]
        public void Patients_Sex_Is_Out_Of_Reference()
        {
            var expectedError = "Значение Sex не соответствует коду пола (Классификатор половой принадлежности, OID 1.2.643.5.1.13.2.1.1.156)";
            var patient = GetPatient();
            patient.Sex = 10;

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_SocialGroup_Is_Out_Of_Reference()
        {
            var expectedError = "Значение SocialGroup не соответствует коду социальной группы (OID справочника: 1.2.643.2.69.1.1.1.4)";
            var patient = GetPatient();
            patient.SocialGroup = 255;

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_SocialStatus_Is_Out_Of_Reference()
        {
            var expectedError = "Значение SocialStatus не соответствует коду социального статуса пациента (OID справочника: 1.2.643.2.69.1.1.1.5)";
            var patient = GetPatient();
            patient.SocialStatus = "Мажор";

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Successfull_Patient_WithDocuments_Add()
        {
            var expectedError = string.Empty;
            var patient = GetPatient();
            var document = GetDocument();
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }



        [Test]
        public void Patients_Document_IdDocumentType_Is_Out_Of_Reference()
        {
            var expectedError = "Значение IdDocumentType не соответствует коду типа документа (OID справочника: 1.2.643.2.69.1.1.1.6)";
            var patient = GetPatient();
            var document = GetDocument();
            document.IdDocumentType = 255;
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }


        [Test]
        public void Patients_Document_IdProvider_Is_Out_Of_Reference()
        {
            var expectedError = "Значение IdProvider не соответствует коду организации, выдавшей документ (Реестр страховых медицинских организаций (ФОМС), OID справочника: 1.2.643.5.1.13.2.1.1.635)";
            var document = GetDocument();
            document.IdProvider = "Provider";
            var patient = GetPatient();
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_Document_Medical_Insurance_Must_Have_IdProvider()
        {
            var expectedError = "Для медицинских полисов необходимо указать страховую организацию";
            var patient = GetPatient();
            var document = GetDocument();
            //Точное значение IdDocumentType соответствующее страховому полюсу
            document.IdDocumentType = 255;
            document.IdProvider = null;
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        public void Patients_Document_Not_Medical_Insurance_Must_Have_Not_IdProvider()
        {
            var expectedError = "Страховая организация указывается только страхового полиса";
            var patient = GetPatient();
            var document = GetDocument();
            //Точное значение IdDocumentType не соответствующее страховому полюсу
            document.IdDocumentType = 255;
            document.IdProvider = "Provider";
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }


        [Test]
        public void Patients_Document_Must_Have_Series()
        {
            var expectedError = "Для данного типа документа необходимо указать серию";
            var patient = GetPatient();
            var document = GetDocument();
            //Точное значение IdDocumentType соответствующее документу имеющему серию
            document.IdDocumentType = 255;
            document.DocS = null;
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        public void Patients_Document_Must_Have_Not_Series()
        {
            var expectedError = "Для данного типа документа серия не указывается";
            var patient = GetPatient();
            var document = GetDocument();
            //Точное значение IdDocumentType не соответствующее документу имеющему серию
            document.IdDocumentType = 255;
            document.DocS = "Series";
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_Document_DocN_Separators_Not_Allowed()
        {
            var expectedError = "Для поля DocN разделители недопустимы";
            var patient = GetPatient();
            var document = GetDocument();
            document.DocN = "AAA-AAA";
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_Document_IdDocumentType_IsNull()
        {
            var expectedError = "Значение поля IdDocumentType не может быть пустым";
            var patient = GetPatient();
            DocumentDto document = new DocumentDto()
                                    {                                        
                                        DocN = "123456",
                                        ProviderName = "Provider"
            };
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_Document_DocN_IsNull()
        {
            var expectedError = "Значение поля DocN не может быть пустым";
            var patient = GetPatient();
            var document = GetDocument();
            document.DocN = null;
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_Document_ProvierName_IsNull()
        {
            var expectedError = "Значение поля DocN не может быть пустым";
            var patient = GetPatient();
            var document = GetDocument();
            document.ProviderName = null;
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_Document_IssuedDate_IsBigger_Then_ExpiredDate()
        {
            var expectedError = "Значение поля IssuedDate не может быть больше чем поле ExpiredDate";
            var patient = GetPatient();
            var document = GetDocument();
            document.IssuedDate = new DateTime(2016, 1, 1);
            document.ExpiredDate = new DateTime(2016, 1, 1);
            patient.Documents = new DocumentDto[] { document };

            var error = TryAddPatient(guid, idLpu, client, patient);

            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Successfull_Patient_With_Addres_Add()
        {
            var expectedError = string.Empty;
            var patient = GetPatient();
            var address = GetAddress();
            patient.Addresses = new AddressDto[] { address};


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_Addresses_idAddressType_Is_Out_Reference()
        {
            var expectedError = "Значение idAddressType не соответствует идентификатору типа адреса (OID справоч-ника: 1.2.643.2.69.1.1.1.28)";
            var patient = GetPatient();
            var address = GetAddress();
            address.IdAddressType = 255;
            patient.Addresses = new AddressDto[] { address };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_Addresses_idAddressType_Is_Null()
        {
            var expectedError = "Значение поля idAddressType не может быть пустым";
            var patient = GetPatient();
            AddressDto address = new AddressDto() { StringAddress = "SomeAddress" };
            patient.Addresses = new AddressDto[] { address };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_Addresses_StringAddress_Is_Null()
        {
            var expectedError = "Значение поля StringAddress не может быть пустым";
            var patient = GetPatient();
            var address = GetAddress();
            address.StringAddress = null;
            patient.Addresses = new AddressDto[] { address };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Successfull_Patient_With_BirthPlace_Add()
        {
            var expectedError = string.Empty;
            var patient = GetPatient();
            var birthPlace = GetBirthPlace();
            patient.BirthPlace = birthPlace;


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_BirthPlace_Country_IsNull()
        {
            var expectedError = "Значение поля Country не может быть пустым";
            var patient = GetPatient();
            var birthPlace = GetBirthPlace();
            birthPlace.Country = null;
            patient.BirthPlace = birthPlace;


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_BirthPlace_Region_IsNull()
        {
            var expectedError = "Значение поля Region не может быть пустым";
            var patient = GetPatient();
            var birthPlace = GetBirthPlace();
            birthPlace.Region = null;
            patient.BirthPlace = birthPlace;


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_BirthPlace_City_IsNull()
        {
            var expectedError = "Значение поля City не может быть пустым";
            var patient = GetPatient();
            var birthPlace = GetBirthPlace();
            birthPlace.City = null;
            patient.BirthPlace = birthPlace;


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }


        [Test]
        public void Successfull_Patient_With_Contacts_Add()
        {
            var expectedError = string.Empty;
            var patient = GetPatient();
            var contact = GetContact();
            patient.Contacts = new ContactDto[] { contact};


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_Contacts_idContactType_Is_Out_Reference()
        {
            var expectedError = "Значение IdContactType не соответствует идентификатору типа контакта (OID справочника: 1.2.643.2.69.1.1.1.27)";
            var patient = GetPatient();
            var contact = GetContact();
            contact.IdContactType = 255;
            patient.Contacts = new ContactDto[] { contact };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }


        [Test]
        public void Patient_Contacts_idContactType_IsNull()
        {
            var expectedError = "Значение поля IdContactType не может быть пустым";
            var patient = GetPatient();
            ContactDto contact = new ContactDto() { ContactValue = "Contact" };
            patient.Contacts = new ContactDto[] { contact };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_Contacts_ContactValue_IsNull()
        {
            var expectedError = "Значение поля ContactValue не может быть пустым";
            var patient = GetPatient();
            var contact = GetContact();
            contact.ContactValue = null;
            patient.Contacts = new ContactDto[] { contact };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Successfull_Patient_With_Job_Add()
        {
            var expectedError = string.Empty;
            var patient = GetPatient();
            var job = GetJob();
            patient.Job =  job ;


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }



        [Test]
        public void Patient_Job_CompanyName_IsNull()
        {
            var expectedError = "Значение поля CompanyName не может быть пустым";
            var job = GetJob();
            var patient = GetPatient();
            job.CompanyName = null;
            
            patient.Job = job;


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }


        [Test]
        public void Patient_Job_Sphere_Is_Out_Reference()
        {
            var expectedError = "Значение Sphere не соответствует коду наименования отрасли (Общероссийский классификатор видов экономической деятельности, OID 1.2.643.5.1.13.2.1.1.62)";
            var patient = GetPatient();
            var job = GetJob();
            //Любое значение отсутствующее в ОКВЭД
            job.Sphere = "AAA";
            patient.Job = job;


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_Job_DateStart_IsBigger_Then_DateEnd()
        {
            var expectedError = "Значение поля DateStart не может быть больше чем поле DateEnd";
            var patient = GetPatient();
            var job = GetJob();
            job.DateStart = new DateTime(2016, 1,1);
            job.DateEnd = new DateTime(2015, 1, 1);
            patient.Job = job;


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }


        [Test]
        public void Successfull_Patient_With_Privelege_Add()
        {
            var expectedError = string.Empty;
            var patient = GetPatient();
            patient.Privilege = new PrivilegeDto()
                                    {
                                        DateStart = new DateTime(2016, 1,1),
                                        DateEnd = new DateTime(2017, 1,1),
                                        IdPrivilegeType = 10
                                    };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_Privelege_DateStart_IsNull()
        {
            var expectedError = "Значение поля DateStart не может быть пустым";
            var patient = GetPatient();
            patient.Privilege = new PrivilegeDto()
                                    {               
                                        DateEnd = new DateTime(2017, 1, 1),
                                        IdPrivilegeType = 10
                                    };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_Privelege_DateEnd_IsNull()
        {
            var expectedError = "Значение поля DateEnd не может быть пустым";
            var patient = GetPatient();
            patient.Privilege = new PrivilegeDto()
                                {
                                    DateStart = new DateTime(2016, 1, 1),
                                    IdPrivilegeType = 10
                                };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_Privelege_IdPrivelege_IsNull()
        {
            var expectedError = "Значение поля IdPrivelegeType не может быть пустым";
            var patient = GetPatient();
            patient.Privilege = new PrivilegeDto()
                                    {
                                        DateStart = new DateTime(2016, 1, 1),
                                        DateEnd = new DateTime(2017, 1, 1)
                                    };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patients_Privilege_DateStart_IsBigger_Then_DateEnd()
        {
            var expectedError = "Значение поля DateStart не может быть больше чем поле DateEnd";
            var patient = GetPatient();
            patient.Privilege = new PrivilegeDto()
                                    {
                                        DateStart = new DateTime(2017, 1, 1),
                                        DateEnd = new DateTime(2016, 1, 1),
                                        IdPrivilegeType = 10
                                    };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test]
        public void Patient_Privilege_IdPrivilegeType_Is_Out_Reference()
        {
            var expectedError = "Значение IdPrivelegeType не соответствует идентификатору категории льготности (OID справочника: 1.2.643.2.69.1.1.1.7)";
            var patient = GetPatient();
            patient.Privilege = new PrivilegeDto()
                                {
                                    DateStart = new DateTime(2017, 1, 1),
                                    DateEnd = new DateTime(2016, 1, 1),
                                    IdPrivilegeType = 10
                                };


            var error = TryAddPatient(guid, idLpu, client, patient);


            Assert.That(error, Is.EqualTo(expectedError));
        }

    }
}
