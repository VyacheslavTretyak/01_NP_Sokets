using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreetServer
{	
	class StreetDBServer
	{
		
		private delegate void Connect(Socket socket, string cond);
		public StreetDBServer(int port)
		{			
			Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPAddress ip = IPAddress.Any;
			IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
			try
			{
				s.Bind(ep);
				s.Listen(10);
				Console.WriteLine("Server started");
				while (true)
				{
					Socket ns = s.Accept();
					Console.WriteLine(ns.RemoteEndPoint.ToString());
					StringBuilder strBuilder = new StringBuilder();
					byte[] buff = new byte[1024];
					int l = 0;					
					do
					{
						l = ns.Receive(buff);
						strBuilder.Append(Encoding.Unicode.GetString(buff));
					} while (ns.Available > 0);
					Connect conn = new Connect(SendResult);
					string str = strBuilder.ToString().TrimEnd('\0');
					conn.BeginInvoke(ns,str , null, null);
					Console.WriteLine($"Reveived : {strBuilder.ToString()}");
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				s?.Shutdown(SocketShutdown.Both);
				s?.Close();
				s = null;
			}
		
		}
		private void SendResult(Socket socket, string condition)
		{
			List<string> result = new List<string>();
			using (DataModel db = new DataModel())
			{
				result = db.StreetSet.Where(s => s.Zip == condition).Select(s => s.Street).ToList();
			}
			var binForm = new BinaryFormatter();
			var stream = new MemoryStream();
			binForm.Serialize(stream, result);
			socket.Send(stream.ToArray());
			socket.Shutdown(SocketShutdown.Both);
			socket.Close();
		}
	}
}
