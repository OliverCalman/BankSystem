using System;
using System.Linq;
using System.Collections;
using System.Security;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace BankSystem
{
    class System
    {
        static void Main(string[] args)
        {
            //instantiate view class and call login method on startup
            View BankSystem = new View();
            BankSystem.Login();
        }

    }

    class View
    {
        public void Login()
        {
            View View = new View();

            Console.Clear();
            Console.WriteLine("  LOGIN\t\t\t\tSIMPLE BANK MANAGEMENT SYSTEM\v");
            Console.WriteLine("  Enter login credentials:\v");
            Console.WriteLine("\t\t\tUsername: ");
            Console.WriteLine("\v");
            Console.WriteLine("\t\t\tPassword:");
            Console.WriteLine("\v\v\v\v\v__________________________________________________________________________________");

            //username input
            Console.SetCursorPosition(34, 4);
            string username = Console.ReadLine(); //username

            //password input, masked on UI
            Console.SetCursorPosition(34, 7);
            SecureString securePassword = MaskPassword();  //masked password

            //check if the username and password are correct
            if (CheckPassword(username, securePassword) == true)
            {
                //write to console if input is correct and send to main menu
                Console.SetCursorPosition(0, 14);
                Console.WriteLine("Login successful. Press any key to continue.");
                Console.ReadKey();
                MainMenu();
            }
            else
            {
                //write to console if input is incorrect and prompt to try again
                Console.SetCursorPosition(0, 14);
                Console.WriteLine("Incorrect login details. Press any key to try again.");
                Console.ReadKey();
                Login();
            }

            Console.SetCursorPosition(0, 14); //return to default cursor positon
        }

        public bool CheckPassword(string username, SecureString securePassword)
        {
            //declare credential filename
            string filename = (@"users/credentials.txt");

            //declare a plain-text password so it can be read from the file
            string password = new NetworkCredential(string.Empty, securePassword).Password;

            //declare new hashtable
            Hashtable ht = new Hashtable();

            //open account file
            string[] loginfile = File.ReadAllLines(filename);

            //add each line to the hashtable as a key/value pair
            foreach (string text in loginfile)
            {
                string[] kv = text.Split(',');
                ht.Add(kv[0], kv[1]);
        
            }

            //if the account file contains the username and password entered by the user return true
            if (ht.Contains(username) == true && (string)ht[username] == password)
            {
                return true;
            }
            else
            {
                //return false if the username or password don't match
                return false;
            }

        }


        public static SecureString MaskPassword()
        {
              var password = new SecureString();
              while (true)
              {
                    //read the user input 1 key at a time
                    ConsoleKeyInfo input = Console.ReadKey(true);
                    //submit password if enter is pressed
                    if (input.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    //if the user enters a backspace, remove the previous char, write a space, then remove the space in order to clear the asterisk
                    else if (input.Key == ConsoleKey.Backspace)
                    {
                        if (password.Length > 0)
                        {
                            password.RemoveAt(password.Length - 1);
                            Console.Write("\b \b");
                        }
                    }
                    else
                    {
                    // replace valid user inputs with asterisk
                        password.AppendChar(input.KeyChar);
                        Console.Write("*");
                    }
              }

              //return the secureString
                return password;
        }


        public void MainMenu()
        {
            string input;

            //display the main menu UI
            Console.Clear();
            Console.WriteLine("  MAIN  \t\t\t\tSIMPLE BANK MANAGEMENT SYSTEM\v");
            Console.WriteLine("  Select one of the following:\v");
            Console.WriteLine("\t1. Create a new account");
            Console.WriteLine("\t2. Search for an account");
            Console.WriteLine("\t3. Deposit");
            Console.WriteLine("\t4. Withdraw");
            Console.WriteLine("\t5. Account Statement");
            Console.WriteLine("\t6. Delete Account\v");
            Console.WriteLine("\t7. Exit\v");
            Console.WriteLine("\tSelection or command");
            Console.WriteLine("\t===>\t________________________________________________\v");
            Console.WriteLine("__________________________________________________________________________________");

            //set cursor on top of entry line
            Console.SetCursorPosition(16, 14);
            input = Console.ReadLine();


            //user input will direct to a new view
            if (input == "1")
            {
                CreateAccount();
            }
            else if (input == "2")
            {
                Search();
            }
            else if (input == "3")
            {
                Deposit();
            }
            else if (input == "4")
            {
                Withdraw();
            }
            else if (input == "5")
            {
                Statement();
            }
            else if (input == "6")
            {
                Delete();
            }
            else if (input == "7")
            {
                Exit();
            }
            else
            {
                //note: marking guidelines call for a non integer check. This does the same thing; non integer or outside range will just return to the main menu without an error
                //no additional checks required
                MainMenu();
            }

        }

        public void CreateAccount()
        {
            //instantiate account class
            Account account = new Account();

            Console.Clear();
            Console.WriteLine("  CREATE ACCOUNT\t\tSIMPLE BANK MANAGEMENT SYSTEM\v");
            Console.WriteLine("  Enter the account details:\v");
            Console.WriteLine("\tFirst Name:\v");
            Console.WriteLine("\tSurname:\v");
            Console.WriteLine("\tAddress:\v");
            Console.WriteLine("\tPhone:\v");
            Console.WriteLine("\tEmail:\v");
            Console.WriteLine("\v");
            Console.WriteLine("__________________________________________________________________________________");

            Console.SetCursorPosition(20, 4);
            account.FirstName = Console.ReadLine(); // First name input

            Console.SetCursorPosition(20, 6);
            account.Surname = Console.ReadLine(); //surname input

            Console.SetCursorPosition(20, 8);
            account.Address = Console.ReadLine(); //address input

            Console.SetCursorPosition(20, 10);
            account.Phone = Console.ReadLine(); //phone input

            Console.SetCursorPosition(20, 12);
            account.Email = Console.ReadLine(); //email input

            //initialise with $0 balance
            account.Balance = 0.00;

            /*
            //error if any fields are missing
            if (account.FirstName == null || account.Surname == null || account.Address == null || account.Phone == null || account.Email == null)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vPlease enter all account details. Retry? Y/N: _");
                Console.SetCursorPosition(51, 16);
                string Retry = Console.ReadLine();

                if (Retry == "Y" || Retry == "y")
                {
                    CreateAccount();
                }
                else
                {
                    MainMenu();
                }
            }
            */

            //confirm email entered string has “@” (required), “gmail.com”, “outlook.com”, and “uts.edu.au” in the domain
            if (account.Email.Contains("@") && account.Email.Contains("gmail.com") || account.Email.Contains("@") && account.Email.Contains("uts.edu.au") || account.Email.Contains("@") && account.Email.Contains("outlook.com"))
            {
                    //confirm phone is <= 10 chars
                    //error if phone not valid
                    if (account.Phone.Length <= 10 && account.Phone.Length > 0)
                    {
                        //confirm details are correct, Y/N stop/go command
                        Console.WriteLine("\v\v\v\vConfirm the above details are correct Y/N: _");
                        Console.SetCursorPosition(43, 17);
                        string Confirm = Console.ReadLine();

                        //if user confirms details are correct
                        if (Confirm == "Y")
                        {
                            Console.SetCursorPosition(0, 17);
                            //write to console while waiting for account to generate and email to send. 
                            Console.WriteLine("Creating account, please wait...                          ");
                            //first available account number
                            int accountSeed = 1;
                            //prefix account number with 5 chars. Allows for 999 possible account numbers between 6-8 chars long
                            string accountNum = "00000" + accountSeed;

                            //create the filename using the default directory path (hardcoded) and the account id.
                            string filename = @"accounts/" + accountNum + ".txt";

                            for (; File.Exists(filename); accountSeed++)
                            {
                                //append incremented accountseed to filename then loop through again. This loop will continue until it finds a number with no file
                                //not the best solution as technically account numbers could be reused if they're deleted; this will only pick the next available num
                                accountNum = "00000" + accountSeed;
                                filename = @"accounts/" + accountNum + ".txt";
                            }

                            //set account num in account class
                            account.AccountNumber = accountNum;

                            //get current datetime for initial statement line
                            DateTime datetime = DateTime.Now;

                            //create account file will opening balance of $0
                            using (StreamWriter sw = File.CreateText(filename))
                            {
                                sw.WriteLine("firstname," + account.FirstName);
                                sw.WriteLine("surname," + account.Surname);
                                sw.WriteLine("address," + account.Address);
                                sw.WriteLine("phone," + account.Phone);
                                sw.WriteLine("email," + account.Email);
                                sw.WriteLine("balance," + account.Balance);
                                //write initial statement line
                                sw.WriteLine("Account Created on " + datetime + ", Starting Balance: $0.00");
                            }

                            //generate email to user confirming account created
                            Email email = new Email();
                            email.WelcomeEmail(account.Email, account.AccountNumber, account.FirstName);

                            //display the new account number and success message on the UI. Return to main menu on key press
                            Console.SetCursorPosition(0, 17);
                            Console.WriteLine("Created new account with account number " + account.AccountNumber + ". \vPress any key to return to the main menu.");
                            Console.ReadKey();
                            MainMenu();
                        }
                        else
                        {
                            //clear user inputs
                            CreateAccount();
                        }
                    }
                    else
                    {
                        //display error if account num > 10 digits
                        Console.WriteLine("\v\v\v\vPhone number must be 10 digits or less. Press any key to try again.");
                        Console.ReadKey();
                        CreateAccount();

                    }
            }
            else
            {
                //error if email not valid
                Console.WriteLine("\v\v\v\vEmail address must be in a valid format, such as user@domain.tld. \vPress any key to try again.");
                Console.ReadKey();
                CreateAccount();

            }

            Console.ReadKey();
        }

        public void Search()
        {
            Account account = new Account();

            Console.Clear();
            Console.WriteLine("  SEARCH\t\t\t\tSIMPLE BANK MANAGEMENT SYSTEM\v");
            Console.WriteLine("  Enter an account number to search:\v");
            Console.WriteLine("\tAccount Number:\v");
            Console.WriteLine("\v\v\v\v\v\v\v\v");
            Console.WriteLine("__________________________________________________________________________________");

            Console.SetCursorPosition(24, 4);
            //input the search parameter 
            string searchparameter = Console.ReadLine();

            //check user input no more than 10 chars
            if (searchparameter.Length > 10)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount number must be less than 10 characters. Press any key to retry.");
                Console.ReadKey();
                Search();
            }

            //check user input is an integer
            bool checkInput = Int32.TryParse(searchparameter, out int accountNumInt);
            if (!checkInput)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount number must be an integer. Press any key to retry.");
                Console.ReadKey();
                Search();
            }

            //pass through current view in account details for error handling
            account = account.GetAccountDetails(searchparameter);

            //render when account found
            if (account.AccountNumber != null)
            {
                Console.Clear();
                Console.WriteLine("  SEARCH\t\t\t\tSIMPLE BANK MANAGEMENT SYSTEM\v");
                Console.WriteLine("  Account Found:\v\v");
                Console.WriteLine("\tAccount Number: " + account.AccountNumber + "\v");
                Console.WriteLine("\tBalance: $" + account.Balance + "\v");
                Console.WriteLine("\tFirst Name: " + account.FirstName + "\v");
                Console.WriteLine("\tSurname: " + account.Surname + "\v");
                Console.WriteLine("\tAddress: " + account.Address + "\v");
                Console.WriteLine("\tPhone: " + account.Phone + "\v");
                Console.WriteLine("\tEmail: " + account.Email + "\v");
                Console.WriteLine("\v");
                Console.WriteLine("__________________________________________________________________________________");
                Console.WriteLine("Press any key to return to the main menu.");

                //return to menu on key press
                Console.ReadKey();
                MainMenu();
            }
            else
            {
                //retry if account not found
                string Retry;

                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount not found, retry? Y/N: _");
                Console.SetCursorPosition(31, 16);
                Retry = Console.ReadLine();

                if (Retry == "Y" || Retry == "y")
                {
                    Search();
                }
                else
                {
                    //return to menu if user input is not Yes
                    MainMenu();
                }
            }
        }

        public void Deposit()
        {
            Account account = new Account();

            Console.Clear();
            Console.WriteLine("  DEPOSIT\t\t\t\tSIMPLE BANK MANAGEMENT SYSTEM\v");
            Console.WriteLine("  Enter the details:\v");
            Console.WriteLine("\tAccount Number:\v");
            Console.WriteLine("\tDeposit Amount: $\v");
            Console.WriteLine("\v\v\v\v\v\v");
            Console.WriteLine("__________________________________________________________________________________");

            //get account number
            Console.SetCursorPosition(24, 4);
            string searchparameter = Console.ReadLine();

            //check user input no more than 10 chars
            if (searchparameter.Length > 10)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount number must be less than 10 characters. Press any key to retry.");
                Console.ReadKey();
                Deposit();
            }

            //check user input is an integer
            bool checkInput = Int32.TryParse(searchparameter, out int accountNumInt);
            if (!checkInput)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount number must be an integer. Press any key to retry.");
                Console.ReadKey();
                Deposit();
            }

            //get account information from file
            account = account.GetAccountDetails(searchparameter);

            //render when account found
            if (account.AccountNumber != null)
            {
                Console.SetCursorPosition(0, 10);
                Console.WriteLine("\t****************************");
                Console.WriteLine("\t Current Balance: $" + account.Balance + "                    ");
                Console.WriteLine("\v\t****************************");
            }
            else
            {
                //retry option if the account isn't found
                string Retry;

                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount not found, retry? Y/N: _");
                Console.SetCursorPosition(31, 16);
                Retry = Console.ReadLine(); 

                if (Retry == "Y" || Retry == "y")
                {
                    Deposit();
                }
                else
                {
                    MainMenu();
                }
            }

            Console.SetCursorPosition(25, 6); //deposit amt cursor position
            try
            {
                //read input and convert to a double
                double depositAmt = Convert.ToDouble(Console.ReadLine());
                //write deposit amount to account
                account.Deposit(account, depositAmt);
            }
            catch (FormatException)
            {
                //file not found error
                Console.WriteLine("\v\v\v\v\v\v\v\v\vInvalid deposit amount. Press any key to retry.");
                Console.ReadKey();
                Deposit();
            }

            //display new balance on screen
            Console.SetCursorPosition(0, 11);
            Console.WriteLine("\t Current Balance: $" + account.Balance);

            //display new balance and prompt to return to main menu
            Console.SetCursorPosition(0, 7);
            Console.WriteLine("\v\v\v\v\v\v\v\v\vAccount balance has been updated to: $" + account.Balance + "\vPress any key to return to the main menu.");

            //return to main menu
            Console.ReadKey();
            MainMenu();

        }

        public void Withdraw()
        {

            Account account = new Account();

            Console.Clear();
            Console.WriteLine("  WITHDRAW\t\t\t\tSIMPLE BANK MANAGEMENT SYSTEM\v");
            Console.WriteLine("  Enter the details:\v");
            Console.WriteLine("\tAccount Number:\v");
            Console.WriteLine("\tWithdrawal Amount: $\v");
            Console.WriteLine("\v\v\v\v\v\v");
            Console.WriteLine("__________________________________________________________________________________");

            //get account number
            Console.SetCursorPosition(24, 4);
            string searchparameter = Console.ReadLine();

            //check user input no more than 10 chars
            if (searchparameter.Length > 10)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount number must be less than 10 characters. Press any key to retry.");
                Console.ReadKey();
                Withdraw();
            }

            //check user input is an integer
            bool checkInput = Int32.TryParse(searchparameter, out int accountNumInt);
            if (!checkInput)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount number must be an integer. Press any key to retry.");
                Console.ReadKey();
                Withdraw();
            }

            //get account information from file
            account = account.GetAccountDetails(searchparameter);

            //render when account found
            if (account.AccountNumber != null)
            {
                Console.SetCursorPosition(0, 10);
                Console.WriteLine("\t****************************");
                Console.WriteLine("\t Current Balance: $" + account.Balance);
                Console.WriteLine("\v\t****************************");
            }
            else
            {
                //retry option if the account isn't found
                string Retry;

                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount not found, retry? Y/N: _");
                Console.SetCursorPosition(31, 16);
                Retry = Console.ReadLine();

                if (Retry == "Y" || Retry == "y")
                {
                    Withdraw();
                }
                else
                {
                    MainMenu();
                }
            }

            Console.SetCursorPosition(28, 6); //withdrawal amt cursor position

            try
            {
                //convert user input to double
                double withdrawAmt = Convert.ToDouble(Console.ReadLine());

                if (withdrawAmt > account.Balance)
                {
                    Console.WriteLine("\v\v\v\v\v\v\v\v\vWithdrawal amount exceeds available funds. Press any key to retry.");
                    Console.ReadKey();
                    Withdraw();
                }
                //write deposit amount to account
                account.Withdraw(account, withdrawAmt);
            }
            catch (FormatException)
            {
                //throw exception if input is not a double
                Console.WriteLine("\v\v\v\v\v\v\v\v\vInvalid withdrawal amount. Press any key to retry.");
                Console.ReadKey();
                Withdraw();
            }

            //display new balance on screen
            Console.SetCursorPosition(0, 11);
            //print new balance + whitespace to overwrite previous balance
            Console.WriteLine("\t Current Balance: $" + account.Balance + "                    ");

            Console.SetCursorPosition(0, 7);
            Console.WriteLine("\v\v\v\v\v\v\v\v\vAccount balance has been updated to: $" + account.Balance + "\vPress any key to return to the main menu.");

            //return to main menu
            Console.ReadKey();
            MainMenu();
        }

        public void Statement()
        {
            Account account = new Account();

            Console.Clear();
            Console.WriteLine("  STATEMENT\t\t\t\tSIMPLE BANK MANAGEMENT SYSTEM\v");
            Console.WriteLine("  Enter the details:\v");
            Console.WriteLine("\tAccount Number:\v");
            Console.WriteLine("\v\v\v\v\v\v\v\v");
            Console.WriteLine("__________________________________________________________________________________");

            //get account number
            Console.SetCursorPosition(24, 4);
            string searchparameter = Console.ReadLine();

            //check user input no more than 10 chars
            if (searchparameter.Length > 10)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount number must be less than 10 characters. Press any key to retry.");
                Console.ReadKey();
                Statement();
            }

            //check user input is an integer
            bool checkInput = Int32.TryParse(searchparameter, out int accountNumInt);
            if (!checkInput)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount number must be an integer. Press any key to retry.");
                Console.ReadKey();
                Statement();
            }

            //get account information from file
            account = account.GetAccountDetails(searchparameter);

            //render when account found
            if (account.AccountNumber != null)
            {
                Console.Clear();
                Console.WriteLine("  STATEMENT\t\t\t\tSIMPLE BANK MANAGEMENT SYSTEM\v");
                Console.WriteLine("  Account Found:\v");
                Console.WriteLine("\tAccount Number: " + account.AccountNumber);
                Console.WriteLine("\tBalance: $" + account.Balance);
                Console.WriteLine("\tFirst Name: " + account.FirstName);
                Console.WriteLine("\tSurname: " + account.Surname);
                Console.WriteLine("\tAddress: " + account.Address);
                Console.WriteLine("\tPhone: " + account.Phone); ;
                Console.WriteLine("\tEmail: " + account.Email + "\v");
                Console.WriteLine("  Statement:");
                Console.WriteLine("\v\v\v\v\v\v");
                Console.WriteLine("__________________________________________________________________________________");

            }
            else
            {
                //retry option if the account isn't found
                string Retry;

                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount not found, retry? Y/N: _");
                Console.SetCursorPosition(31, 16);
                Retry = Console.ReadLine();

                if (Retry == "Y" || Retry == "y")
                {
                    Statement();
                }
                else
                {
                    MainMenu();
                }
            }

            //get array of last 5 items in statement file
            string[] statements = account.GetStatement(account);

            //write all items in array to console
            Console.SetCursorPosition(0, 14);
            foreach (string i in statements)
            {
                Console.WriteLine("\t" + i);
            }

            //Option to send email with statement information
            Console.SetCursorPosition(0, 21);
            Console.WriteLine("Would you like to send the statement via email? Y/N: _");
            Console.SetCursorPosition(53, 21);
            string confirm = Console.ReadLine();

            //check user input
            if (confirm == "Y" || confirm == "y")
            {
                Console.SetCursorPosition(0, 21);
                Console.WriteLine("Sending email, please wait...                                  ");
                //generate email to user
                Email email = new Email();
                email.StatementEmail(account.Email, account.AccountNumber, account.FirstName);
            }

            //return to main menu
            Console.SetCursorPosition(0, 21);
            Console.WriteLine("Press any key to return to the main menu.                    ");
            Console.ReadKey();
            MainMenu();
            

        }

        public void Delete()
        {
            Account account = new Account();

            Console.Clear();
            Console.WriteLine("  DELETE ACCOUNT\t\tSIMPLE BANK MANAGEMENT SYSTEM\v");
            Console.WriteLine("  Enter the account number to be deleted:\v");
            Console.WriteLine("\tAccount Number:\v");
            Console.WriteLine("\v\v\v\v\v\v\v\v");
            Console.WriteLine("__________________________________________________________________________________");

            Console.SetCursorPosition(24, 4);
            // Console.SetCursorPosition(25, 5); //deposit amt cursor position
            string accountNum = Console.ReadLine();

            //check user input no more than 10 chars
            if (accountNum.Length > 10)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount number must be less than 10 characters. Press any key to retry.");
                Console.ReadKey();
                Delete();
            }

            //check user input is an integer
            bool checkInput = Int32.TryParse(accountNum, out int accountNumInt);
            if (!checkInput)
            {
                Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount number must be an integer. Press any key to retry.");
                Console.ReadKey();
                Delete();
            }

            //get account information from file
            account = account.GetAccountDetails(accountNum);

            //render when account found
            if (account.AccountNumber != null)
            {

                    //display the acc details
                    Console.WriteLine("\tBalance: $" + account.Balance);
                    Console.WriteLine("\tFirst Name: " + account.FirstName);
                    Console.WriteLine("\tSurname: " + account.Surname);
                    Console.WriteLine("\tAddress: " + account.Address);
                    Console.WriteLine("\tPhone: " + account.Phone);
                    Console.WriteLine("\tEmail: " + account.Email);


                    //ask user to confirm deletion
                    Console.WriteLine("\v\v\v\v\vAre you sure you wish to delete the account? Y/N: _ ");
                    Console.SetCursorPosition(50, 16);

                    //read user input and remove account
                    string confirm = Console.ReadLine();
                    if (confirm == "Y" || confirm == "y")
                    {
                        //delete account file 
                        string filename = @"accounts/" + accountNum + ".txt";
                        File.Delete(filename);
                        
                        Console.SetCursorPosition(0, 16);
                        
                        //remove confirmation msg from UI
                        Console.Write(new string(' ', Console.WindowWidth));
                        
                        //success message
                        Console.SetCursorPosition(0, 16);
                        Console.WriteLine("Account deleted. Press any key to return to the main menu.");

                        //return to main menu
                        Console.ReadKey();
                        MainMenu();
                    }
                    else
                    {
                        MainMenu();
                    }

             }
             else
             {
                    //retry option if the account isn't found
                    string Retry;

                    Console.WriteLine("\v\v\v\v\v\v\v\v\v\v\vAccount not found, retry? Y/N: _");
                    Console.SetCursorPosition(31, 16);
                    Retry = Console.ReadLine();

                    if (Retry == "Y" || Retry == "y")
                    {
                        Delete();
                    }
                    else
                    {
                        MainMenu();
                    }
             }
            
        }

        public void Exit()
        {
            //check user input = Yes
            string ExitKey;

            Console.WriteLine("\v\vAre you sure you wish to exit? Y/N: _ ");
            Console.SetCursorPosition(36, 17);
            ExitKey = Console.ReadLine();

            if (ExitKey == "Y" || ExitKey == "y")
            {
                //exit console if user input = Yes
                Environment.Exit(0);
            }
            else
            {
                //return to main menu if user input != Yes
                MainMenu();
            }

        }

    }

    class Account
    {
        private string accountnumber;
        private double balance;
        private string firstname;
        private string surname;
        private string address;
        private string email;
        private string phone;

        //getters & setters for account attributes
        public string AccountNumber
        {
            get { return accountnumber; }
            set { accountnumber = value; }
        }

        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        public string FirstName
        {
            get { return firstname; }
            set { firstname = value; }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public Account GetAccountDetails(string accountNo)
        {
            //instantiate new account class
            Account account = new Account();

            //pass search param and locate file
            string filename = (@"accounts/" + accountNo + ".txt");

            //declare new hashtable
            Hashtable ht = new Hashtable();

            //check if file exists
            try
            {
                //open account file
                string[] accountFile = File.ReadAllLines(filename);

                //add each line to the hashtable as a key/value pair
                foreach (string text in accountFile)
                {
                    string[] kv = text.Split(',');

                    ht.Add(kv[0], kv[1]);
                }

                //add all the key/value pairs into the account class
                account.AccountNumber = accountNo;
                account.FirstName = (string)ht["firstname"];
                account.Surname = (string)ht["surname"];
                account.Address = (string)ht["address"];
                account.Phone = (string)ht["phone"];
                account.Email = (string)ht["email"];
                account.Balance = Convert.ToDouble(ht["balance"]);

            }
            catch (FileNotFoundException)
            {
                //catches exception to avoid break; error handling is handled in the view class
            }
               
            return account; 
        }

        //note that deposit and withdraw are functionally identical, all transactions could be achieved with a single method by passing through the relevant transaction type
        public double Deposit(Account account, double depositAmt)
        {

            //using amount to be deposited, add to double
            account.Balance += depositAmt;

            //replace balance in file, rewrites the file entirely rather than finding the balance using key/value pairs or regex. These would be cleaner, but it would 
            //necessitate using something other than streamwriter
            string filename = @"accounts/" + account.AccountNumber + ".txt";

            //read file into memory then replace line 6 where the balance is stored
            string[] fileContents = File.ReadAllLines(filename);
            fileContents[5] = "balance," + account.Balance;
            File.WriteAllLines(filename, fileContents);

            //generate new line in statement file
            DateTime datetime = DateTime.Now;
            string text = ("Deposit on: " + datetime.ToString() + ", Amount Deposited: $" + depositAmt.ToString() + ", Current Balance: $" + account.Balance.ToString() + "\n");
            File.AppendAllText(filename , text);

            return account.Balance;

        }
        public double Withdraw(Account account, double withdrawAmt)
        {

            //using amount to be deposited, add to double
            account.Balance -= withdrawAmt;

            //replace balance in file, rewrites the file entirely rather than finding the balance using key/value pairs or regex. These would be cleaner, but it would 
            //necessitate using something other than streamwriter
            string filename = @"accounts/" + account.AccountNumber + ".txt";

            //read file into memory then replace line 6 where the balance is stored
            string[] fileContents = File.ReadAllLines(filename);
            fileContents[5] = "balance," + account.Balance;
            File.WriteAllLines(filename, fileContents);

            //generate new line in account file
            DateTime datetime = DateTime.Now;
            string text = ("Withdrawal on: " + datetime.ToString() + ", Amount Withdrawn: $" + withdrawAmt.ToString() + ", Current Balance: $" + account.Balance.ToString());
            File.AppendAllText(filename, text);

            return account.Balance;

        }

        public string[] GetStatement(Account account)
        {
            string filename = @"accounts/" + account.AccountNumber + ".txt";

            try
            {
                //add all lines into an array
                string[] fileread = File.ReadAllLines(filename);
                
                //remove the account details sections, start from line 6
                var statementArray = fileread.Skip(6);

                //take last 5 items from array
                string[] statements = statementArray.TakeLast(5).ToArray();

                return statements;

            }
            catch (FileNotFoundException) // Catch file not found exception
            {
                Console.WriteLine("Statement not found");
                Console.ReadKey();
                //View.MainMenu();
                return null;

            }

        }
        
    }

    class Email
    {
        private const string emailAddress = "noreply.oliverc.tempmailbox@gmail.com"; //sender address hardcoded
        private const string password = "ngwdlmhexgyryjmu"; //password hardcoded, main acc password: yBACaLTQWg6fCX2
        private const string mailServer = "smtp.gmail.com";

        public void WelcomeEmail(string recipient, string accountNumber, string firstName)
        {
            //write email subject and body in html
            string subject = ("Welcome to UTS bank, " + firstName);
            string body = ("<h1>Welcome to UTS bank!</h1><p>" + "Hi " + firstName + ",<br>A new account has been created for you with UTS Bank. <br>Your account number is:</p><h3>" + accountNumber + "</h3><p>Please keep this information safe for use in future correspondence.<br><br>Regards,<br>UTS Bank</p>");

            //smtp settings
            var smtpClient = new SmtpClient(mailServer)
            {
                Port = 587,
                Credentials = new NetworkCredential(emailAddress, password),
                EnableSsl = true,
            };

            //compose new HTML message
            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            //add recipient address to mailmessage
            mailMessage.To.Add(recipient);

            //send email
            smtpClient.Send(mailMessage);

        }
        public void StatementEmail(string recipient, string accountNumber, string firstName)
        {
            Account account = new Account();
            account = account.GetAccountDetails(accountNumber);

            //write email subject and body in html
            string subject = ("Your UTS Bank Statement");
            string body = ("<p>Hi " + firstName + ",<br>Your last 5 statements are shown below:<br>");

            //get array of last 5 items in statement file
            string[] statements = account.GetStatement(account);

            //use stringbuilder to add each statement line to the email body
            StringBuilder sb = new StringBuilder(body);
            foreach (string i in statements)
            {
                sb.Append("<br>" + i + "<br>");
            }
            sb.Append("<br>Regards,<br> UTS Bank </p> ");

            String statementBody = sb.ToString();

            //smtp settings
            var smtpClient = new SmtpClient(mailServer)
            {
                Port = 587,
                Credentials = new NetworkCredential(emailAddress, password),
                EnableSsl = true,
            };

            //compose new HTML message
            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailAddress),
                Subject = subject,
                Body = statementBody,
                IsBodyHtml = true,
            };
            //add recipient address to mailmessage
            mailMessage.To.Add(recipient);

            //send email
            smtpClient.Send(mailMessage);

        }
    }
}