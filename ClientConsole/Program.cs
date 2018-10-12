//сервер

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ClientApp
{
	class Program
	{
		static int port = 58930; // server port
		static void Main(string[] args)
		{
			#region ServerPart
			IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, port);

			Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				listenSocket.Bind(iPEndPoint);

				listenSocket.Listen(13);
				Console.WriteLine("Server started. Waiting for messages...");
				while (true)
				{
					Socket handle = listenSocket.Accept();
					Console.WriteLine($"{handle.RemoteEndPoint}");
					StringBuilder stringBuilder = new StringBuilder();
					byte[] buffer = new byte[1024];
					int bytes = 0;
					do
					{
						bytes = handle.Receive(buffer);
						stringBuilder.Append(Encoding.Unicode.GetString(buffer));
					} while (handle.Available > 0);
					Console.WriteLine(DateTime.Now.ToString() + ": " + stringBuilder.ToString());
					string message = "Your message delivered!";
					buffer = Encoding.Unicode.GetBytes(message);
					handle.Send(buffer);

					handle.Shutdown(SocketShutdown.Both);
					handle.Close();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message, "Error");
			}
			#endregion


		}
	}
}

//клиент 

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;

//namespace ClientSenderConsoleApp
//{
//	class Program
//	{
//		static int port = 58930; // server port
//		static string address = "134.249.130.54";
//		static void Main(string[] args)
//		{
//			#region ClientPart
//			IPAddress ip = IPAddress.Parse(address);
//			IPEndPoint ep = new IPEndPoint(ip, port);
//			Socket s = new Socket(AddressFamily.InterNetwork,
//			SocketType.Stream, ProtocolType.Tcp);

//			try
//			{
//				s.Connect(ep);
//				while (true)
//				{
//					Console.WriteLine("Type a message: ");
//					string message = Console.ReadLine();
//					byte[] data = Encoding.Unicode.GetBytes(message);
//					if (s.Connected)
//					{
//						s.Send(data);
//						StringBuilder builder = new StringBuilder();
//						int bytes = 0;
//						do
//						{
//							bytes = s.Receive(data);
//							builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
//						} while (bytes > 0);
//						Console.WriteLine($"Server answer: " + builder.ToString());
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine(ex.Message);
//			}
//			finally
//			{
//				try
//				{
//					s?.Shutdown(SocketShutdown.Both);
//					s?.Close();
//				}
//				catch (Exception ex)
//				{
//					Console.WriteLine(ex.Message);
//				}
//			}
//			Console.ReadKey();
//			#endregion
//		}
//	}
//}