using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace DW
{
    public enum TypePaquet
    {
        Data,
        Command,
        Chat
    }

    [Serializable] // Pour que la classe soit sérialisable
    public class Packet //Une superclasse pour les paquets
    {
        public TypePaquet Type { get; protected set; }

        public Packet(TypePaquet Type)
        {
            this.Type = Type;
        }

        //Méthode statique pour l'envoi et la réception
        public static void Send(Packet paquet, UdpClient link, IPEndPoint ie = null)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, paquet);
            byte[] dtmp = ms.ToArray();
            // Console.WriteLine("before " + dtmp.Length);
            byte[] dgram = Zip.Compress(dtmp);
            //Console.WriteLine("after " + dgram.Length);
            if (dgram.Length > 65507)
            {
                Console.WriteLine("Packet trop volumineux");
            }
            link.Send(dgram, dgram.Length);
        }

        public static Packet Receive(UdpClient link)
        {
            IPEndPoint i = null;
            byte[] dtmp = link.Receive(ref i);
            byte[] tmp = Zip.Decompress(dtmp);
            try
            {
                // convert byte array to memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(tmp);

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // set memory stream position to starting point
                _MemoryStream.Position = 0;


                return (Packet)_BinaryFormatter.Deserialize(_MemoryStream);
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine(_Exception.ToString());
            }
            return null;
        }

        //Méthode statique pour l'envoi et la réception
        public static void Send(Packet paquet, NetworkStream stream)
        {
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(stream, paquet);
            stream.Flush();
        }

        public static Packet Receive(NetworkStream stream)
        {
            Packet p = null;

            BinaryFormatter bf = new BinaryFormatter();
            p = (Packet)bf.Deserialize(stream);

            return p;
        }
    }
}