﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LVMS.Zipato;

namespace ConsoleApplication1
{
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
            Console.Write("User name: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Connecting...");

            var client = new ZipatoClient();
            await client.LoginAsync(username, password);

            var endpoints = client.GetEndpointsAsync();
        }
    }
}
