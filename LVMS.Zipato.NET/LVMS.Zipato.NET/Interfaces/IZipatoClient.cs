using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Interfaces
{
    interface IZipatoClient
    {
        /// <summary>
        /// Establishes a secure connection with the Zipato API service.
        /// It first calls user/init to get the nonce and then calls user/login
        /// to establish a secure session.
        /// </summary>
        /// <param name="userNameEmail">Username or E-mail address</param>
        /// <param name="password">Plain-text password. Gets SHA1-encrypted before being transmitted over the wire.</param>
        /// <returns><c>true</c> if success, <c>false</c> if authentication failed.</returns>
        Task LoginAsync(string userNameEmail, string password);
        Task<bool> CheckConnection();
    }
}
