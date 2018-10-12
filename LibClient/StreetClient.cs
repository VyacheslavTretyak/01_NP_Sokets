using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StreetClient
{
	class StreetDBClient
	{		
		public StreetDBClient(string ipStr, int port, ListBox tb, string value)
		{
			IPAddress ip = IPAddress.Parse(ipStr);			
			IPEndPoint ep = new IPEndPoint(ip, port);
			Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			List<byte> data = new List<byte>();
			try
			{
				s.Connect(ep);
				if (s.Connected)
				{
					string strSend = value;
					s.Send(Encoding.Unicode.GetBytes(strSend));
					byte[] buff = new byte[1024];
					int l = 0;
					do
					{
						l = s.Receive(buff);
						data.AddRange(buff);
					} while (l > 0);
					
				}
				else
				{
					throw new Exception("error connection!");
				}
			}
			catch (Exception ex)
			{
				tb.Items.Add(ex.Message);
			}
			finally
			{
				s?.Shutdown(SocketShutdown.Both);
				s?.Close();
			}

			var stream = new MemoryStream();
			var binFormatter = new BinaryFormatter();

			// Where 'objectBytes' is your byte array.
			stream.Write(data.ToArray(), 0, data.Count);
			stream.Position = 0;

			var list = binFormatter.Deserialize(stream) as List<string>;
			foreach (var val in list)
			{
				tb.Items.Add(val.ToString());
			}
		}
	}
}
