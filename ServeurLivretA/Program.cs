using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServeurLivretA
{
    class Program
    {
        static void Communication(Object arg) // arg est sockClient
        {
            byte[] b_clientAnnee = new byte[4], b_clientSommeInit = new byte[8];
            Socket mySock = (Socket)arg;

            try
            {
                mySock.Receive(b_clientAnnee);
                mySock.Receive(b_clientSommeInit);

                mySock.Send(CalculGain(b_clientAnnee, b_clientSommeInit));
            }
            catch (Exception ex)
            {

                Console.WriteLine("/!\\ " + ex.Message + "/!\\");
            }
        }

        static byte[] CalculGain(byte[] year, byte[] sommeDepart)
        {
            byte[] b_sommeFinale;
            int i_annee;
            double d_somme, d_sommeFinale = 0;

            try
            {
                i_annee = BitConverter.ToInt32(year, 0);
                d_somme = BitConverter.ToDouble(sommeDepart, 0);

                for (int i = 0; i < i_annee; i++)
                {
                    d_sommeFinale = d_sommeFinale + (0.01 * d_sommeFinale);
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            b_sommeFinale = BitConverter.GetBytes(d_sommeFinale);

            return b_sommeFinale;
        }

        static void Main(string[] args)
        {
            try
            {
                // Création de la socket
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Attachement à un n° port donné
                IPEndPoint iep = new IPEndPoint(IPAddress.Any, 1212);
                sock.Bind(iep);

                // Ouverture du service
                sock.Listen(5);

                // Attente des clients et communication
                while (true)
                {
                    Socket sockClient = sock.Accept(); // On attend qu'un client se connecte. Serveur en pause. 
                                                    //Si client, Accept retourne Socket utilisé pour communiquer avec le client

                    ThreadPool.QueueUserWorkItem(new WaitCallback(Communication), sockClient);
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("/!\\ Erreur : " + ex.Message + "/!\\");
            }
        }
    }
}