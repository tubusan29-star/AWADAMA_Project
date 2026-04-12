Imports Npgsql

Public Module DBManager
    Friend Sub getDBConnect(ByRef con As NpgsqlConnection)

        Dim connString As String = "Host=aws-1-ap-northeast-2.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.putjlrckatjibyhtxolw;Password=unidamastadio;SSL Mode=Require;Trust Server Certificate=true"

        con = New NpgsqlConnection(connString)

    End Sub

    Friend Sub exec(ByRef con As NpgsqlConnection, sql As String)
        Try
            Using cmdInsert As New NpgsqlCommand(sql, con)
                Dim rowsAffected = cmdInsert.ExecuteNonQuery()
            End Using
        Catch ex As Exception
        End Try
    End Sub

    Friend Function selectData(ByRef con As NpgsqlConnection, sql As String) As DataTable
        Dim resulet As New DataTable()

        Try
            ' SELECT（読み込み）の実行
            Dim selectSql As String = sql
            Using cmdSelect As New NpgsqlCommand(selectSql, con)
                If cmdSelect.Connection.FullState = System.Data.ConnectionState.Closed Then
                    Return Nothing
                End If
                Using reader = cmdSelect.ExecuteReader()
                    resulet.Load(reader)
                End Using
            End Using
        Catch ex As Exception
            Return Nothing
        End Try

        Return resulet
    End Function
End Module
