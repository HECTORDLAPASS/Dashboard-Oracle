Imports Oracle.ManagedDataAccess.Client

Public Class ConnectionOracle
    Private connectionString As String
    Private connection As OracleConnection

    Public Sub New()
        connectionString = "User Id=HECTOR;Password=ADMIN;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-3AGDMLR)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));"
        connection = New OracleConnection(connectionString)
    End Sub

    Public Sub Connect()
        Try
            connection.Open()
            Console.WriteLine("Conexión exitosa a la base de datos Oracle!")
        Catch ex As Exception
            MessageBox.Show("Error al conectar: " & ex.Message)
        End Try
    End Sub

    Public Sub Disconnect()
        Try
            If connection.State = ConnectionState.Open Then
                connection.Close()
                Console.WriteLine("Conexión cerrada correctamente.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error al desconectar de la base de datos: " & ex.Message)
        End Try
    End Sub

    Public Function SelectQuery(query As String) As DataTable
        Dim dt As New DataTable
        Try
            Connect()
            Using cmd As New OracleCommand(query, connection)
                Using da As New OracleDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        Catch ex As Exception
            ' MessageBox.Show("Error al ejecutar la consulta: " & ex.Message)
        Finally
            Disconnect()
        End Try
        Return dt
    End Function
    Public Function ExecuteScalar(query As String) As Object
        Dim result As Object = Nothing
        Try
            Connect()
            Using cmd As New OracleCommand(query, connection)
                result = cmd.ExecuteScalar()
            End Using
        Catch ex As Exception
            ' MessageBox.Show("Error al ejecutar la consulta: " & ex.Message)
        Finally
            Disconnect()
        End Try
        Return result
    End Function

End Class