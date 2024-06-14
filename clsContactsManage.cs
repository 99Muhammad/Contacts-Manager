using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static Lab3_ConactsManager.clsContactsManage;

namespace Lab3_ConactsManager
{

    public class clsContactsManage
    {
        public struct stContactInfo
        {
            public string Category;
            public string FirstName;
            public string LastName;
            public string PhoneNumber;

        };

        public static List<stContactInfo> lstContactInfo = new List<stContactInfo>();
        public static List<string> lstContactsNames = new List<string>();
        public static stContactInfo stContact;
        public static ushort ChoiseNumber = 0;
        public static bool isPhoneNumber = false;
        public static bool isExist = false;
        public static bool isRepeateContactAdd = false;
        public static string ContactName = "";


        public static void PrintContactInfo()
        {
            int NumberContact = 1;
            var ContactsByCategory = lstContactInfo.GroupBy(c => c.Category.ToUpper());
            foreach (var category in ContactsByCategory)
            {
                NumberContact = 1;
                Console.WriteLine($"\nContacts in the [ {category.Key} ] category");
                foreach (var contact in category.OrderBy(c => c.Category))
                {
                    Console.WriteLine($"\nContact: {NumberContact++}\n");
                    Console.WriteLine($"First Name: {contact.FirstName}");
                    Console.WriteLine($"Last Name: {contact.LastName}");
                    Console.WriteLine($"Phone Number: {contact.PhoneNumber}");
                }
            }
        }

        public static bool IsContactExist(string ContactName)
        {
            return lstContactsNames.Any(n => string.Concat(n.Trim().Split(new[] { ' ' },
             StringSplitOptions.RemoveEmptyEntries))
             .Equals(ContactName, StringComparison.OrdinalIgnoreCase));
        }
        public static void ContactInfoScreen()
        {
            Console.Clear();
            Console.WriteLine("\t\t----------------------------------------------");
            Console.WriteLine("\t\t\t\tContact Screen");
            Console.WriteLine("\t\t----------------------------------------------");

        }
        public static void ReadContactInfo()
        {

            bool isYes = false;
            do
            {
                ContactInfoScreen();

                Console.WriteLine("What category of your contact ,[Family,Work,Frindly,etc]?");
                string CategoryType = ValidateValuesWhenAddContact();
                clsContactsManage.stContact.Category = CategoryType;

                Console.WriteLine("Enter First Name for contact ?");
                clsContactsManage.stContact.FirstName = ValidateValuesWhenAddContact();

                Console.WriteLine("Enter Last Name for contact ?");
                stContact.LastName = ValidateValuesWhenAddContact();

                ContactName = stContact.FirstName + " " + stContact.LastName;
                ContactName = string.Concat(ContactName.Trim().Split(new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries));


                Console.WriteLine("Enter Phone Number for contact ?");
                isPhoneNumber = true;
                clsContactsManage.stContact.PhoneNumber = ValidateValuesWhenAddContact();

            } while (isYes);




        }

        public static List<string> AddContact()
        {
            ContactName = stContact.FirstName + " " + stContact.LastName;
            ContactName = string.Concat(ContactName.Trim().Split(new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries));

            if (!IsContactExist(ContactName))
            {
                isRepeateContactAdd = false;
                lstContactInfo.Add(stContact);

                lstContactsNames = lstContactInfo.Select(c => $"{c.FirstName} {c.LastName}").ToList();
                return lstContactsNames;

            }

            Console.WriteLine("The contact that you tried to add it is existed,press any key to go back :-(");

            //Console.ReadKey();
            isRepeateContactAdd = true;
            return lstContactsNames;

        }

        public static bool CheckingYesOrNoInputs()
        {


            string ExpectedValue = (Console.ReadLine());

            while (true)
            {
                if (char.TryParse(ExpectedValue, out char UserInput))
                {
                    if (ExpectedValue.ToString().Length == 1 && ExpectedValue.ToString().ToLower() == "y")
                    {
                        return true;

                    }
                    else if (ExpectedValue.ToString().Length == 1 && ExpectedValue.ToString().ToLower() == "n")
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid {Y/N}.");
                    ExpectedValue = (Console.ReadLine());
                }
            }

        }

        public static string ValidateValuesWhenAddContact()
        {
            string RemovingBy = "";
            bool isRepeate = false;
            do
            {

                RemovingBy = Convert.ToString(Console.ReadLine());

                //To Confirm there is not invalid input and it is not number 
                if (!string.IsNullOrWhiteSpace(RemovingBy) && !Int32.TryParse(RemovingBy, out Int32 PhoneNumber) & !isPhoneNumber)
                {
                    return RemovingBy;

                }
                //To Confirm there is not invalid input and it is a number
                else if (!string.IsNullOrWhiteSpace(RemovingBy) && Int32.TryParse(RemovingBy, out PhoneNumber) && isPhoneNumber)
                {
                    isPhoneNumber = false;
                    return RemovingBy;
                }
                else
                {
                    Console.WriteLine("You enter invalid data,press any key to try again ");
                    isRepeate = true;
                    Console.ReadKey();
                }


            } while (isRepeate);

            return RemovingBy;
        }

        public static void RemoveScreen()
        {
            Console.Clear();
            Console.WriteLine("\t\t----------------------------------------------");
            Console.WriteLine("\t\t\t\tRemove Contact Screen");
            Console.WriteLine("\t\t----------------------------------------------");

            Console.WriteLine("You can remove any contact by :\n" +
                       "Full Name [ (First & Last) Name]\n");
        }
        public static void ReadInputToRemoveContact(ref string RemovingBy)
        {
            RemoveScreen();

            RemovingBy = Convert.ToString(Console.ReadLine());

            RemovingBy = string.Concat(RemovingBy.Trim().Split(new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries));

            do
            {
                if (!string.IsNullOrWhiteSpace(RemovingBy) && !Int32.TryParse(RemovingBy, out Int32 PhoneNumber))
                {
                    if (!IsContactExist(RemovingBy))
                    {
                        Console.WriteLine("This contact is NOT Exist ,try again! :-(");
                        ReadInputToRemoveContact(ref RemovingBy);
                    }
                    else
                        break;
                }
                else
                {
                    Console.WriteLine("You entered invalid data , press any key to try again ");
                    Console.ReadKey();
                }

            } while (true);
        }

        public static List<string> RemoveContact(string RemovingBy)
        {

            string FullName = "";
            foreach (stContactInfo item in lstContactInfo)
            {
                FullName = item.FirstName + item.LastName;

                if (FullName == RemovingBy)
                {
                    lstContactInfo.Remove(item);
                    lstContactsNames.Remove(FullName);
                    Console.WriteLine($"The contact is deleted successfully");
                    Console.ReadKey();
                    //break;

                    lstContactsNames = lstContactInfo.Select(c => $"{c.FirstName} {c.LastName}").ToList();
                    return lstContactsNames;

                }
            }
            return lstContactsNames;

        }

        public static void ViewAllContactsScreen()
        {
            Console.Clear();
            Console.WriteLine("\t\t----------------------------------------------");
            Console.WriteLine("\t\t\tView All Contacts Screen");
            Console.WriteLine("\t\t----------------------------------------------");
        }
        public static List<string> ViewAllContacts()
        {
            //ViewAllContactsScreen();
            lstContactsNames = lstContactInfo.Select(c => $"{c.FirstName} {c.LastName}").ToList();
            return lstContactsNames;

        }

        public static void IsIntegerValue()
        {

            while (true)
            {
                Console.Write("Enter a number: ");
                if (ushort.TryParse(Console.ReadLine(), out ChoiseNumber))
                {
                    //This condition to include case when user want to remove contact
                    if (ChoiseNumber >= 1 && ChoiseNumber <= 3)
                    {
                        break;
                    }
                    //This condition to include case when user enter number from main screen 
                    else if (ChoiseNumber >= 1 && ChoiseNumber <= 4)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"You entered: {ChoiseNumber} you can just write " +
                            $"[1,2,3]");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
        }

        public static void PrintNamesForContacts()
        {
            int ContactNumber = 1;
            var ContactsByCategory = lstContactInfo.GroupBy(c => c.Category.ToUpper());
            foreach (var category in ContactsByCategory)
            {
                ContactNumber = 1;
                Console.WriteLine($"Contacts in the [ {category.Key} ] category:\n");

                // Get the sorted list of contact names for the current category
                var contactNames = category.Select(c => $"{c.FirstName} {c.LastName}")
                                           .OrderBy(n => n)
                                           .ToList();


                foreach (var name in contactNames)
                {
                    Console.WriteLine($"Contact {ContactNumber++} : " + name + "\n");
                }

                Console.WriteLine("");
            }
        }
        public static void PrintScreen()
        {
            Console.Clear();
            Console.WriteLine("\t\t----------------------------------------------");
            Console.WriteLine("\t\t\t\tPrint Screen");
            Console.WriteLine("\t\t----------------------------------------------");


            Console.WriteLine("\t\t\t\t[1] Print Contact's Name\n\t\t\t\t[2] Print All Contact's Information \n" +
                        "\t\t\t\t---> Press [1] or [2]");

        }
        public static void CheckTheWayUserWantToPrintContacts()
        {

            PrintScreen();

            IsIntegerValue();
            if (ChoiseNumber == 1)
                PrintNamesForContacts();

            else if (ChoiseNumber == 2)
                PrintContactInfo();

            else if (ChoiseNumber == 3)
            {
                Console.WriteLine("You Entered 3 ,it is invalid");
                CheckingYesOrNoInputs();
            }


        }
        public static void MainMenueScreen()
        {
            Console.Clear();
            Console.WriteLine("\t\t----------------------------------------------");
            Console.WriteLine("\t\t\t\tMain Menu Screen");
            Console.WriteLine("\t\t----------------------------------------------");

            Console.WriteLine("\t\t\t\t[1] Add new Contact ");
            Console.WriteLine("\t\t\t\t[2] Remove a Contact ");
            Console.WriteLine("\t\t\t\t[3] View All Contacts ");
            Console.WriteLine("\t\t\t\t[4] Exit ");
            Console.WriteLine("\t\t\t\t{You can choose from 1 to 4 }");
        }
        public static void ContactManager()
        {

            List<stContactInfo> lstcontactInfo = new List<stContactInfo>();

            string RemovingBy = "";
            do
            {
                MainMenueScreen();
                IsIntegerValue();

                if (ChoiseNumber == 1)
                {
                    isRepeateContactAdd = true;

                    while (isRepeateContactAdd)
                    {
                        ReadContactInfo();
                        lstContactsNames = AddContact();

                        if (isRepeateContactAdd)
                            continue;

                        Console.WriteLine("\nContat added succesfully ... ");
                        Console.WriteLine("\nDo you want to add another contact (y/n)?");

                        isRepeateContactAdd = CheckingYesOrNoInputs();
                    }

                    CheckTheWayUserWantToPrintContacts();

                    ChoiseNumber = 0;
                    Console.WriteLine("Press any key to go back to main menue screen");
                    isRepeateContactAdd = false;
                    Console.ReadKey();

                }
                if (ChoiseNumber == 2)
                {
                    ReadInputToRemoveContact(ref RemovingBy);

                    lstContactsNames = RemoveContact(RemovingBy);

                    if (lstContactInfo.Count != 0)
                        CheckTheWayUserWantToPrintContacts();
                    else
                        Console.WriteLine("\nNo any contact is found\n");

                    Console.WriteLine("Press any key to go back to main menue screen");

                    Console.ReadKey();
                }
                if (ChoiseNumber == 3)
                {
                    ViewAllContactsScreen();
                    lstContactsNames = ViewAllContacts();

                    CheckTheWayUserWantToPrintContacts();

                    Console.WriteLine("Press any key to go back to main menue screen");
                    Console.ReadKey();
                }

                if (ChoiseNumber == 4)
                {
                    Console.WriteLine("You press 4 to exit from program");
                    return;
                }

            } while (ChoiseNumber != 4);
        }

    }
}
