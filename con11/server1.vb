Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Text.RegularExpressions

Module Module1
	'ソケット通信(サーバー側)
	Sub Main()
		Dim host1 As IPAddress = IPAddress.Parse("127.0.0.1")
		Dim port1 As Integer = 8765
		Dim ipe1 As IPEndPoint = New IPEndPoint(host1, port1)
		Dim server As TcpListener = Nothing
		Dim recvline As String
		Dim sendline As String = Nothing
		Dim num As Integer
		Dim outflg As Boolean = False
		Dim buf(1024) As Byte
		Dim reg As Regex = New Regex("\0")
		Try
			server = New TcpListener(ipe1)
			Console.WriteLine("クライアントからの入力待ち状態")
			server.Start()
			While (True)
				Using client = server.AcceptTcpClient()
					Using stream = client.GetStream()
						While stream.Read(buf, 0, buf.Length) <> 0
							recvline = reg.Replace(Encoding.UTF8.GetString(buf), "")
							Console.WriteLine("client側の入力文字＝" + recvline)
							If recvline = "bye" Then
								outflg = True
								Exit While
							End If
							Try
								num = Integer.Parse(recvline)
								If num Mod 2 = 0 Then
									sendline = "OKです"
								Else
									sendline = "NGです"
								End If
							Catch ex As Exception
								sendline = "数値を入力して下さい"
							Finally
								buf = Encoding.UTF8.GetBytes(sendline)
								stream.Write(buf, 0, buf.Length)
								Array.Clear(buf, 0, buf.Length)
							End Try
						End While
						If (outflg = True) Then
							Exit While
						End If
					End Using
				End Using
			End While
		Catch ex As Exception
			Console.WriteLine(ex.Message)
		Finally
			server.Stop()
			Console.WriteLine("サーバー側終了です")
		End Try
	End Sub
End Module