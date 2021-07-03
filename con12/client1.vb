Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Text.RegularExpressions

Module client1
	'ソケット通信(クライアント側)
	Sub Main()
		Dim host1 As IPAddress = IPAddress.Parse("127.0.0.1")
		Dim port1 As Integer = 8765
		Dim ipe1 As IPEndPoint = New IPEndPoint(host1, port1)
		Dim line As String = Nothing
		Dim buf1(1024) As Byte
		Dim buf2(1024) As Byte
		Dim reg As Regex = New Regex("\0")
		Try
			Using client = New TcpClient()
				client.Connect(ipe1)
				Using stream = client.GetStream()
					While (line <> "bye")
						' 標準入力からデータを取得
						Console.WriteLine("--------------------------")
						Console.WriteLine("偶数の数値を入力して下さい")
						Console.WriteLine("--------------------------")
						' サーバに送信
						line = Console.ReadLine()
						buf1 = Encoding.UTF8.GetBytes(line)
						stream.Write(buf1, 0, buf1.Length)
						' サーバから受信
						stream.Read(buf2, 0, buf2.Length)
						Console.WriteLine(
							reg.Replace(Encoding.UTF8.GetString(buf2), ""))
					End While
				End Using
			End Using
		Catch ex As Exception
			Console.WriteLine(ex.Message)
		Finally
			Console.WriteLine("クライアント側終了です")
		End Try
	End Sub
End Module
