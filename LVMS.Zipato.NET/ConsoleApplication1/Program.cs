using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LVMS.Zipato;
using LVMS.Zipato.Enums;
using LVMS.Zipato.Model;

namespace LVMS.Zipato.TestClient
{
    /// <summary>
    /// Test application for testing the C# Zipato library.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Task t = Connect();
            t.Wait();
        }

        static async Task Connect()
        {
            Console.WriteLine("Zipato API Test Client");

            var credentials = GetCredentials();
            Console.WriteLine("Connecting...");

            var client = new ZipatoClient();
            await client.LoginAsync(credentials.UserName, credentials.Password);
            Console.WriteLine("Connected.");

            var offlineDevices = await client.GetDevicesOfflineAsync();


            var onOffEndpoints = await client.GetEndpointsWithOnOffAsync();
            

            var partitions = await client.GetAlarmPartitionsAsync();
            var partition = await client.GetAlarmPartitionAsync(partitions[0].Uuid);

            await client.SetAlarmModeAsync(partition, "0000", Enums.AlarmArmMode.DISARMED);

            while (!await client.IsAlarmPartitionReady(partition.Uuid))
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            await client.SetAlarmModeAsync(partition, "0000", Enums.AlarmArmMode.AWAY);
            await Task.Delay(TimeSpan.FromSeconds(5));
            await client.SetAlarmModeAsync(partition, "0000", Enums.AlarmArmMode.DISARMED);

            var alarmReady = await client.IsAlarmPartitionReady(partition.Uuid);
            if (alarmReady)
            {
                await client.SetAlarmModeAsync(partition, "0000", Enums.AlarmArmMode.AWAY);
                await Task.Delay(TimeSpan.FromSeconds(5));
                await client.SetAlarmModeAsync(partition, "0000", Enums.AlarmArmMode.DISARMED);
            }
            //await client.SetOnOffState("Kantoorverlichting", true);

            //var state = await client.GetAttributeValueAsync<int>("Rolluik Zolder", Enums.CommonAttributeNames.POSITION);
            //await client.SetPositionAsync("Rolluik Zolder", 0);

            //var rollruikEndpoint = await client.GetEndpointAsync("Rolluik Beneden Links");

            //

            
            //var rooms = await client.GetContactsAsync();
            
        }

        /// <summary>
        /// Get plain-text credentials that are needed for connecting with the API.
        /// Credentials are loaded from a text file or from a prompt.
        /// </summary>
        /// <returns></returns>
        private static System.Net.NetworkCredential GetCredentials()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.CredentialsFile) && File.Exists(
                    Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFile)))
                {
                    var lines = File.ReadLines(Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFile)).ToArray();
                    return new System.Net.NetworkCredential(lines[0], lines[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't read credentials from file. Error: " + ex.Message);
            }
            return GetCredentialsViaPrompt();
        }

        /// <summary>
        /// Prompt the user for a user name and password.
        /// </summary>
        /// <returns></returns>
        private static System.Net.NetworkCredential GetCredentialsViaPrompt()
        {
            Console.WriteLine("Please enter your Zipato credentials.");
            Console.Write("User name (e-mail address): ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = ReadPassword();
            Console.WriteLine();

            return new System.Net.NetworkCredential(username, password);
        }

        // Taken from http://stackoverflow.com/a/7049688/393367

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <param name="mask">a <c>char</c> representing your choice of console mask</param>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword(char mask)
        {
            const int ENTER = 13, BACKSP = 8, CTRLBACKSP = 127;
            int[] FILTERED = { 0, 27, 9, 10 /*, 32 space, if you care */ }; // const

            var pass = new Stack<char>();
            char chr = (char)0;

            while ((chr = System.Console.ReadKey(true).KeyChar) != ENTER)
            {
                if (chr == BACKSP)
                {
                    if (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (chr == CTRLBACKSP)
                {
                    while (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (FILTERED.Count(x => chr == x) > 0) { }
                else
                {
                    pass.Push((char)chr);
                    System.Console.Write(mask);
                }
            }

            System.Console.WriteLine();

            return new string(pass.Reverse().ToArray());
        }

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword()
        {
            return ReadPassword('*');
        }
    }
}
