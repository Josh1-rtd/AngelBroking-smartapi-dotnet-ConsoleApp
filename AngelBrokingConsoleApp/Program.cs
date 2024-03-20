using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OtpNet;
using System.Threading;
using System.IO;
using System.Linq;

namespace AngelBroking
{
    class Program
    {

        public static string CurrPath = AppDomain.CurrentDomain.BaseDirectory;
        public static AngelToken sagr = new AngelToken();
        public static bool loggedin =false;
        public static string uid;// = "A5234435";  //YOUR CLIENT CODE
        public static string pwd;//  = "2323";    //YOUR MPIN
        public static string apikey; // = "asu72736scb";   
        public static string jwtToken = "";  // optional
        public static string refreshToken = ""; // optional
        public static string feedToken = "";
        public static bool dontexit;
        public static string TOTPkey;//  = "30Character TOTPkey";
        public static SmartApi sApi; //  = new SmartApi(apikey, jwtToken, refreshToken);
        public static OutputBaseClass obj = new OutputBaseClass();
        public static string broker = "Angelone";

        static void Main(string[] args)
        {

            if (broker == "Angelone")
               
                if (File.Exists(@"user.txt"))
                {
                    string[] lines = File.ReadAllLines(@"user.txt");
                    uid = lines[0]; 
                    pwd = lines[1];
                    apikey = lines[2]; 
                    TOTPkey = lines[3];

                }else { Console.WriteLine("User Credentials not found"); }
            sApi = new SmartApi(apikey, jwtToken, refreshToken);
            if (File.Exists(@"Angelone.txt"))
                {
                    sagr.jwtToken = File.ReadLines(@"Angelone.txt").First();
                    sagr.refreshToken = File.ReadLines(@"Angelone.txt").Skip(1).First();
                    sagr.feedToken = File.ReadLines(@"Angelone.txt").Skip(2).First();

                    //Get Profile
                    obj = sApi.GetProfile(sagr);
                    GetProfileResponse gpi = new GetProfileResponse();
                    gpi = obj.GetProfileResponse;

                    if (gpi?.status != true)
                    {
                        Console.WriteLine("Session Expired Login again? \n" + gpi?.message +
                            "\nThis will generate new Session", "Confirmation");

                    }
                    else
                    {
                        loggedin = true;
                        Console.WriteLine("Previous Session token active");
                    }

                }

                if (loggedin != true)
                {
                    // Compute TOTP code
                    byte[] array = Base32Encoding.ToBytes(TOTPkey); // convert string to byte array    
                    var totp = new Totp(array, step: 30);
                    var totpCode = totp.ComputeTotp();
                    Console.WriteLine("Logging in");
                    //Login by client code and password
                    obj = sApi.GenerateSession(uid, pwd, totpCode);
                    Thread.Sleep(1000);
                    AngelToken agr = obj.TokenResponse;

                    Console.WriteLine("------GenerateSession call output-------------");
                    //Console.WriteLine(JsonConvert.SerializeObject(agr));
                    Console.WriteLine("jwtoken- " + agr.jwtToken.ToString());
                    Console.WriteLine("refreshtoken - " + agr.refreshToken.ToString());
                    Console.WriteLine("feedtoken - " + agr.feedToken.ToString());
                    Console.WriteLine("----------------------------------------------");

                    //Get Token
                    obj = sApi.GenerateToken();
                    sagr = obj.TokenResponse;

                    Console.WriteLine("------GenerateToken call output-------------");
                    //Console.WriteLine(JsonConvert.SerializeObject(sagr));
                    Console.WriteLine("jwtoken- " + sagr.jwtToken.ToString());
                    Console.WriteLine("refreshtoken - " + sagr.refreshToken.ToString());
                    Console.WriteLine("feedtoken - " + sagr.feedToken.ToString());

                    if (agr.jwtToken != sagr.jwtToken)
                    {
                        Console.WriteLine("agr.jwtToken != sagr.jwtToken");
                    }
                    if (agr.refreshToken != sagr.refreshToken)
                    {
                        Console.WriteLine("agr.refreshToken != sagr.refreshToken");
                    }
                    if (agr.feedToken != sagr.feedToken)
                    {
                        Console.WriteLine("agr.feedToken != sagr.feedToken");
                    }
                    Console.WriteLine("----------------------------------------------");

                    //Get Profile
                    obj = sApi.GetProfile(sagr);
                    GetProfileResponse gp = obj.GetProfileResponse;

                    if (gp.status != true)
                    {
                        Console.WriteLine("Error conncting- " + gp.errorcode + " , " + gp.message);
                        Console.Read();
                    }
                    else
                    {
                        Console.WriteLine("------GetProfile call output-------------");
                        Console.WriteLine(JsonConvert.SerializeObject(gp));
                        Console.WriteLine("----------------------------------------------");
                        //Program.getJKey = sagr.jwtToken;
                        //Program.RefreshToken = sagr.refreshToken;
                        //Program.feedtoken = sagr.feedToken;
                        //
                        string[] lines = { sagr.jwtToken.ToString(), sagr.refreshToken.ToString(), sagr.feedToken.ToString() };
                        File.WriteAllLines(@"Angelone.txt", lines);
                        lines = File.ReadAllLines(@"Angelone.txt");
                        if (lines[0].Contains(sagr.jwtToken) == false | lines[1].Contains(sagr.refreshToken) == false | lines[2].Contains(sagr.feedToken) == false)
                        {
                            Console.WriteLine("Error: Could not write Session token to file.\n Check your rights");

                        }//
                        loggedin = true;
                    }

                }

                if (loggedin == true)
                {
                    Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    {"Authorization", sagr.jwtToken},
                    {"x-api-key", apikey},
                    {"x-client-code", uid},
                    {"x-feed-token", sagr.feedToken}
                };
                    sApi.ConnectAngelWebsocket(headers);
                    // Create a list of exchanges for testing
                    List<SmartApi.Exchange> tokenList = new List<SmartApi.Exchange>();
                    tokenList.Add(new SmartApi.Exchange { exchangeType = 2, tokens = new List<string> { "36612" , "36613" } });
                    tokenList.Add(new SmartApi.Exchange { exchangeType = 1, tokens = new List<string> { "26009", "26000" } });
                    tokenList.Add(new SmartApi.Exchange { exchangeType = 13, tokens = new List<string> { "2388", "2234" } });
                    tokenList.Add(new SmartApi.Exchange { exchangeType = 5, tokens = new List<string> { "426248", "260606" } });

                    // Call the Subscribe function with the correlationID, action, mode and token list
                    sApi.Subscribe("abcde12345", 1, 1, tokenList);

                    Console.WriteLine("Do you want to Logout?\n Y N");
                    string Logout =   Console.Read().ToString();
                    if (Logout.ToUpper() == "Y")
                    {
                       obj = sApi.LogOut(uid);
                        Console.WriteLine(obj.http_error.ToString());
                    }
                    Console.Read();
                }

            }

        }

  
        
 

    }







